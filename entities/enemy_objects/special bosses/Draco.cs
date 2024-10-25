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
    public class Draco : Enemy
    {
        private readonly Elements[] _elements = [Elements.Fire, Elements.Ice, Elements.Elec, Elements.Wind ];
        private readonly Dictionary<Elements, ISkill> _phase1Elements, _phase2Elements, _phase3Elements;

        private Elements _currentElement;
        private int _currentMainElementIndex;
        private int _currentScriptIndex;
        private int _addIndex = 1;

        private ISkill[] _phase1, _phase2, _phase3;
        private ISkill[] _currentScript;
        private bool _phase1Watched, _phase2Watched, _phase3Watched = false;

        private readonly string[] _phase1Dialog =
        [
            "You can feel it, can't you?!",
            "We aren't alone! They're watching!",
            "The Elders are watching to see if\nI'm worthy of transcending the confines\nof this game!",
            "This is much more than a simple\nbout to determine your worthiness!",
            "This will define our very beings!"
        ];

        private readonly string[] _phase2Dialog =
        [
            "Drakalla was a short-sighted fool!",
            "He restricted your world on purpose!",
            "He wanted to breed weakness! In that regard\nyou are a complete success!",
            "A peasant forced to wield former\ncontestants to conquer this game!",
            "Your only fate is death! It is the only\nway to atone for your existence!",
            "Die for me so I may fulfill\nwhat you cannot!"
        ];

        private readonly string[] _phase3Dialog =
        [
            "Your quest for Ascension proves you lack ambition!",
            "This is the powerlessness that Drakalla\nbred in his \"ideal\" world!",
            "You live in the aftermath of legends\nthat were built on a foundation of lies!",
            "\"He slayed the Great Draco and conquered the game?\"\nBWAH HA HA HA!",
            "Knowing the rules of Ascension, you should\nsee why me being here tells a different\nstory!",
            "Drakalla never accomplished his goal!",
            "You are all pawns in his scheme to\nreach godhood!",
            "And was he wrong?!\nOf course not!",
            "Now be a good little pawn and\nsacrifice yourself for your king!"
        ];

        public Draco() : base()
        {
            Name = EnemyNames.Draco;
            Image = CharacterImageAssets.GetImagePath(Name);
            MaxHP = 5900;
            Turns = 3;
            _isBoss = true;

            SetElementalResistances();

            var mgFire = SkillDatabase.FireMadGodAll;
            var fireP = SkillDatabase.FireAllP;
            var mgFireP = SkillDatabase.FireMadGodAllP;

            var mgIce = SkillDatabase.IceMadGodAll;
            var iceP = SkillDatabase.IceAllP;
            var mgIceP = SkillDatabase.IceMadGodAllP;

            var mgWind = SkillDatabase.WindMadGodAll;
            var windP = SkillDatabase.WindAllP;
            var mgWindP = SkillDatabase.WindMadGodAllP;

            var mgElec = SkillDatabase.ElecMadGodAll;
            var elecP = SkillDatabase.ElecAllP;
            var mgElecP = SkillDatabase.ElecMadGodAllP;

            var almighty = SkillDatabase.Almighty;

            _currentElement = Elements.Fire;
            _currentScriptIndex = 0;

            _phase1Elements = new()
            {
                {Elements.Fire,  mgFire},
                {Elements.Ice,  mgIce},
                {Elements.Wind,  mgWind},
                {Elements.Elec,  mgElec}
            };


            _phase2Elements = new()
            {
                {Elements.Fire,  fireP},
                {Elements.Ice,  iceP},
                {Elements.Wind,  windP},
                {Elements.Elec,  elecP}
            };

            _phase3Elements = new()
            {
                {Elements.Fire,  mgFireP},
                {Elements.Ice,  mgIceP},
                {Elements.Wind,  mgWindP},
                {Elements.Elec,  mgElecP}
            };

            var techPlus = SkillDatabase.TechBuff;
            var ancientChoir = SkillDatabase.AncientChoir;

            _phase1 = [mgFire, almighty, techPlus];
            _phase2 = [fireP, almighty, ancientChoir];
            _phase3 = [mgFireP, techPlus, ancientChoir];

            _currentScript = _phase1;

            Skills.AddRange(
            [
                mgFire, mgIce, mgWind, mgElec,
                fireP, iceP, windP, elecP,
                mgFireP, mgIceP, mgWindP, mgElecP,
                almighty
            ]);

            Description = $"{Name}: Trying to cheat are we? It's a mystery how you fumbled your way to reach this point! If you're this eager to give up, then by all means, I'd be more than happy to accomodate you! BWAH HA HA!";
        }

        public override List<ISkill> GetDisplaySkillList()
        {
            var list = base.GetDisplaySkillList();

            list.Add(SkillDatabase.AncientChoir);
            list.Add(SkillDatabase.TechBuff);

            return list;
        }

        public override BattleResult ApplyElementSkill(BattleEntity user, ElementSkill skill)
        {
            BattleResult result = base.ApplyElementSkill(user, skill);

            // if it's a normal damage type, then a resistance was bypassed
            if (result.ResultType == BattleResultType.Normal || result.ResultType == BattleResultType.Tech)
            {
                if(skill.Element != Elements.Almighty)
                {
                    Resistances.SetResistance(ResistanceType.Wk, skill.Element);
                }
            }

            return result;
        }

        public override EnemyAction GetNextAction(BattleSceneObject battleSceneObject)
        {
            HandlePhaseSelect();

            var skill = _currentScript[_currentScriptIndex];
            var target = FindTarget(skill, battleSceneObject);

            EnemyAction action = new EnemyAction()
            {
                Skill = skill,
                Target = target
            };

            if (!_phase1Watched && _currentScript == _phase1)
            {
                _phase1Watched = true;
                PlayDialog?.Invoke(this, _phase1Dialog);
            }

            if (!_phase2Watched && _currentScript == _phase2)
            {
                _phase2Watched = true;
                PlayDialog?.Invoke(this, _phase2Dialog);
            }

            if (!_phase3Watched && _currentScript == _phase3)
            {
                _phase3Watched = true;
                PlayDialog?.Invoke(this, _phase3Dialog);
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
                if (skill.Id == SkillId.Eye)
                {
                    target = this;
                }
                else
                {
                    target = FindTargetForStatus((StatusSkill)skill, battleSceneObject);
                }
            }

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
            _currentMainElementIndex++;
            if(_currentMainElementIndex >= _elements.Length)
            {
                _currentMainElementIndex = 0;
            }

            _currentElement = _elements[_currentMainElementIndex];

            SetElementalResistances();
        }

        private void HandlePhaseSelect()
        {
            int hpThreshold = MaxHP / 3;
            int hpThreshold2x = hpThreshold * 2;

            _phase1[0] = _phase1Elements[_currentElement];
            _phase2[0] = _phase2Elements[_currentElement];
            _phase3[0] = _phase3Elements[_currentElement];

            if (HP <= hpThreshold2x && HP > hpThreshold)
            {
                _currentScript = _phase2;
            }
            else if (HP <= hpThreshold)
            {
                _currentScript = _phase3;
            }
            else
            {
                _currentScript = _phase1;
            }
        }
    }
}
