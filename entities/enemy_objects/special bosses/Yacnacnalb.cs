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
    public class Yacnacnalb : Enemy
    {
        private int _turns = 3;
        
        private Elements _currentElement;
        private int _currentMainElementIndex;
        private int _currentScriptIndex;
        private ISkill _voidDark;

        private ISkill[] _phase1A, _phase1B;
        private ISkill[] _currentScript;
        private bool _phase1Watched, _phase2Watched, _wexHit, _phase1Override = false;

        private readonly string[] _phase1Dialog =
        [
            "With the rise of the Labrybuce, came an\nanomaly in the fabric of your universe.",
            "Draco and his crew of failed contestants\nattempted to leverage it for his own gain.",
            "We should have intervened sooner to\nprevent something like this from\nhappening.",
            "Let your death be the resolution\nto this problem."
        ];

        private readonly string[] _phase2Dialog =
        [
            "There was once a being like yourself\nwho wanted nothing more than to\noverthrow us.",
            "In his weakness, the guardians\nwe summoned claimed his life.",
            "Are you perhaps his successor?",
            "Should you not have died much\nthe same as he did?",
            "I suppose I will have to end you\nmyself to find out..."
        ];

        public Yacnacnalb() : base()
        {
            Name = EnemyNames.Yacnacnalb;
            Image = CharacterImageAssets.GetImagePath(Name);
            MaxHP = 7000;
            Turns = 3;
            _isBoss = true;

            Resistances.SetResistance(ResistanceType.Wk, Elements.Dark);
            Resistances.SetResistance(ResistanceType.Dr, Elements.Elec);
            Resistances.SetResistance(ResistanceType.Dr, Elements.Light);
            Resistances.SetResistance(ResistanceType.Dr, Elements.Fire);

            _voidDark = SkillDatabase.VoidDark;
            var mgLight = SkillDatabase.LightMadGod;
            var holyLight = SkillDatabase.HolyLight;
            var mgElecP = SkillDatabase.ElecMadGodAllP;
            var fireP = SkillDatabase.FireAllP;
            var almighty = SkillDatabase.Almighty;
            var fractalBeam = SkillDatabase.Almighty1;
            var techPlus = SkillDatabase.TechBuff;
            var ancientChoir = SkillDatabase.AncientChoir;
            var elecP = SkillDatabase.PierceElec1;

            _currentElement = Elements.Fire;
            _currentScriptIndex = 0;

            _phase1A = [mgElecP, fractalBeam, holyLight, almighty, mgLight, elecP, techPlus]; // non-wex
            _phase1B = [mgLight, elecP, ancientChoir, techPlus]; // wex
  
            _currentScript = _phase1A;

            Skills.AddRange(
            [
                mgLight, holyLight, mgElecP, fireP, almighty, fractalBeam, elecP
            ]);

            Description = $"{Name}: Lesser being, you ought to focus more on the fight rather than searching for answers where there are none.";
        }

        public override List<ISkill> GetDisplaySkillList()
        {
            var list = base.GetDisplaySkillList();

            list.Add(_voidDark);
            list.Add(SkillDatabase.AncientChoir);
            list.Add(SkillDatabase.TechBuff);

            return list;
        }

        public override BattleResult ApplyElementSkill(BattleEntity user, ElementSkill skill)
        {
            BattleResult result = base.ApplyElementSkill(user, skill);

            // if it's a normal damage type, then a resistance was bypassed
            if (result.ResultType != BattleResultType.Wk && result.ResultType != BattleResultType.TechWk)
            {
                Turns++;
            }
            else
            {
                _wexHit = true;
                _phase1Override = true;
            }

            return result;
        }

        public override EnemyAction GetNextAction(BattleSceneObject battleSceneObject)
        {
            if (_phase1Override)
            {
                _currentScript = _phase1B;
            }

            var skill = _currentScript[_currentScriptIndex];
            var target = FindTarget(skill, battleSceneObject);

            EnemyAction action = new EnemyAction()
            {
                Skill = skill,
                Target = target
            };

            if (_wexHit)
            {
                action.Skill = _voidDark;
                action.Target = this;
                _wexHit = false;
            }

            if (!_phase1Watched && !IsBelowHalfHP())
            {
                _phase1Watched = true;
                PlayDialog?.Invoke(this, _phase1Dialog);
            }

            if (!_phase2Watched && IsBelowHalfHP())
            {
                _phase2Watched = true;
                PlayDialog?.Invoke(this, _phase2Dialog);
            }

            _currentScriptIndex++;
            if (_currentScriptIndex >= _currentScript.Length)
                _currentScriptIndex = 0;

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
                target = FindTargetForStatus((StatusSkill)skill, battleSceneObject);
            }

            if (target == null)
                target = this;

            return target;
        }

        private void SetElementalResistances()
        {
            Resistances.ClearResistances();

            Resistances.SetResistance(ResistanceType.Nu, Elements.Fire);
            Resistances.SetResistance(ResistanceType.Nu, Elements.Ice);
            Resistances.SetResistance(ResistanceType.Nu, Elements.Wind);
            Resistances.SetResistance(ResistanceType.Nu, Elements.Elec);
            Resistances.SetResistance(ResistanceType.Dr, Elements.Dark);
            Resistances.SetResistance(ResistanceType.Dr, Elements.Light);
        }

        public override void ResetEnemyState()
        {
            _currentScriptIndex = 0;
            Turns = _turns;
            _wexHit = false;
            _phase1Override = false;
            _currentScript = _phase1A;
        }

        private bool IsBelowHalfHP()
        {
            int hpThreshold = MaxHP / 2;
            return (HP <= hpThreshold);
        }
    }
}
