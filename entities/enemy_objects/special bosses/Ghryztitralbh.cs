using AscendedZ.battle.battle_state_machine;
using AscendedZ.battle;
using AscendedZ.resistances;
using AscendedZ.skills;
using System.Collections.Generic;
using AscendedZ.entities.battle_entities;
using AscendedZ.game_object;
using System.Linq;

namespace AscendedZ.entities.enemy_objects.special_bosses
{
    public class Ghryztitralbh : Enemy
    {
        private int _index = 0;
        private ISkill[] _wipeScript, _skills;
        private ISkill[] _mainScript;
        private bool _rule1Enforced, _rule2Enforced, _rule3Enforced = false;
        private bool _battleStart, _rule1Cutscene, _rule2Cutscene, _rule3Cutscene = false;

        private readonly string[] _rule1D =
        [
            "I AM THE JUDGE, THE ONE WHO\nPRESIDES OVER THE GAME.",
            "YOU WILL ABIDE BY MY RULES.",
            "ON THE NEXT TURN, I DECREE:",
            "YOU MUST ALLOW ME TO NULL\nONE OF YOUR ATTACKS PER TURN."
        ];

        private readonly string[] _rule2D =
        [
            "I AM THE JUDGE, THE ONE WHO\nPRESIDES OVER THE GAME.",
            "YOU WILL ABIDE BY MY RULES.",
            "THE PREVIOUS RULE IS NO LONGER\nIN EFFECT.",
            "ON THE NEXT TURN, I DECREE:",
            "I WILL INCREASE MY TURN COUNT TO 10.",
            "EACH TIME YOU STRIKE A\nTECHNICAL I WILL LOSE 2 TURNS!"
        ];

        private readonly string[] _rule3D =
        [
            "I AM THE JUDGE, THE ONE WHO\nPRESIDES OVER THE GAME.",
            "YOU WILL ABIDE BY MY RULES.",
            "THE PREVIOUS RULE IS NO LONGER\nIN EFFECT.",
            "ON THE NEXT TURN, I DECREE:",
            "I WILL INCREASE MY TURN COUNT TO 6.",
            "EACH TIME YOU STRIKE A\nTECHNICAL I WILL LOSE 1 TURN!",
            "YOU MUST ALSO ALLOW ME TO\nNULL ONE OF YOUR ATTACKS\nPER TURN."
        ];

        private readonly string[] _death =
        [
            "AS EXPECTED, YOU HAVE FAILED\nTO ABIDE BY MY RULES.",
            "BY THE ORDER OF ASCENSION, I\nDELIVER UNTO YOU YOUR\nPUNISHMENT!"
        ];

        public Ghryztitralbh() : base()
        {
            Name = EnemyNames.Ghryztitralbh;
            Image = CharacterImageAssets.GetImagePath(Name);
            MaxHP = 11000;
            Turns = 3;
            _isBoss = true;
            _battleStart = true;

            Resistances.SetResistance(ResistanceType.Nu, Elements.Light);
            Resistances.SetResistance(ResistanceType.Nu, Elements.Elec);
            Resistances.SetResistance(ResistanceType.Dr, Elements.Fire);
            Resistances.SetResistance(ResistanceType.Dr, Elements.Dark);

            var mgLight = SkillDatabase.PierceLight1;
            var holyLight = SkillDatabase.LightAll;
            var mgElecP = SkillDatabase.ElecMadGodAll;
            var fireP = SkillDatabase.FireAllP;
            var holyGrail = SkillDatabase.AncientChoir;
            var elecP = SkillDatabase.PierceElec1;
            var wipe = SkillDatabase.AlmightyWipe;

            _wipeScript = [SkillDatabase.DragonEye, wipe, wipe, wipe, wipe, wipe, wipe, wipe, wipe, wipe, wipe, wipe];
            _skills = [mgLight, holyLight, mgElecP, fireP, holyGrail, elecP];
            _mainScript = _skills;

            Skills.AddRange([ wipe, mgLight, holyLight, mgElecP, fireP, holyGrail, elecP ]);

            Description = $"{Name}: IN THIS BATTLE, YOU MUST ABIDE BY MY RULES OR PERISH.";
        }

        public override List<ISkill> GetDisplaySkillList()
        {
            var list = base.GetDisplaySkillList();
            list.Add(SkillDatabase.DragonEye);
            return list;
        }

        public override BattleResult ApplyElementSkill(BattleEntity user, ElementSkill skill)
        {
            BattleResult result = base.ApplyElementSkill(user, skill);

            if (_rule1Enforced)
            {
                if (result.ResultType == BattleResultType.Nu)
                    _mainScript = _skills;
            }
            else if (_rule2Enforced)
            {
                if(result.ResultType == BattleResultType.Tech)
                {
                    Turns -= 2;

                    if (Turns <= 0)
                        Turns = 1;
                }
            }
            else if (_rule3Enforced)
            {
                if (result.ResultType == BattleResultType.Nu)
                    _mainScript = _skills;

                if (result.ResultType == BattleResultType.Tech)
                {
                    Turns--;

                    if (Turns <= 0)
                        Turns = 1;
                }
            }

            return result;
        }

        private bool _deathViewed = false;
        public override EnemyAction GetNextAction(BattleSceneObject battleSceneObject)
        {
            HandlePhaseSelect();

            if (!_rule1Cutscene && _rule1Enforced)
            {
                _rule1Cutscene = true;
                PlayDialog?.Invoke(this, _rule1D);
            }

            if (!_rule2Cutscene && _rule2Enforced)
            {
                if(_mainScript == _wipeScript)
                {
                    _mainScript = _skills;
                }
                _rule2Cutscene = true;
                PlayDialog?.Invoke(this, _rule2D);
            }

            if (!_rule3Cutscene && _rule3Enforced)
            {
                if (_mainScript == _wipeScript)
                {
                    _mainScript = _skills;
                }
                _rule3Cutscene = true;
                PlayDialog?.Invoke(this, _rule3D);
            }

            if (_mainScript == _wipeScript && !_deathViewed)
            {
                _deathViewed = true;
                PlayDialog?.Invoke(this, _death);
            }

            var skill = _mainScript[_index];
            var target = FindTarget(skill, battleSceneObject);

            EnemyAction action = new EnemyAction()
            {
                Skill = skill,
                Target = target
            };

            _index++;
            if (_index >= _mainScript.Length)
                _index = 0;

            return action;
        }

        private BattleEntity FindTarget(ISkill skill, BattleSceneObject battleSceneObject)
        {
            BattleEntity target;
            if (skill.Id == SkillId.Elemental)
            {
                target = FindElementSkillTarget((ElementSkill)skill, battleSceneObject);
            }
            else
            {
                if (skill.Id == SkillId.Eye)
                {
                    target = this;
                }
                else
                {
                    target = FindTargetForStatus((StatusSkill)skill, battleSceneObject);
                }
            }

            if (target == null)
                target = this;

            return target;
        }

        private void HandlePhaseSelect()
        {
            int hpThreshold = MaxHP / 3;
            int hpThreshold2x = hpThreshold * 2;

            if (HP <= hpThreshold2x && HP > hpThreshold)
            {
                _rule1Enforced = false;
                _rule2Enforced = true;
                _rule3Enforced = false;
            }
            else if (HP <= hpThreshold)
            {
                _rule1Enforced = false;
                _rule2Enforced = false;
                _rule3Enforced = true;
            }
            else
            {
                _rule1Enforced = true;
                _rule2Enforced = false;
                _rule3Enforced = false;
            }
        }

        public override void ResetEnemyState()
        {
            if (_rule2Enforced)
                _mainScript = _skills;
            else
                _mainScript = _wipeScript;

            if (_rule2Enforced)
                Turns = 10;
            else if (_rule3Enforced)
                Turns = 6;
            else
                Turns = 3;
        }
    }
}
