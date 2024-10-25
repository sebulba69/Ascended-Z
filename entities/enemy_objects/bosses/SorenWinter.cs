using AscendedZ.battle.battle_state_machine;
using AscendedZ.battle;
using AscendedZ.entities.battle_entities;
using AscendedZ.skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Godot.WebSocketPeer;
using AscendedZ.resistances;

namespace AscendedZ.entities.enemy_objects.bosses
{
    public class SorenWinter : Enemy
    {
        private Dictionary<Elements, List<ISkill>> _scripts;
        private List<ISkill> _currentScript;
        private int _move;

        public SorenWinter() : base()
        {
            Name = EnemyNames.Soren_Winter;
            Image = CharacterImageAssets.GetImagePath(Name);
            MaxHP = EntityDatabase.GetBossHP(Name);
            Turns = 3;
            _isBoss = true;

            Resistances.SetResistance(ResistanceType.Wk, Elements.Light);
            Resistances.SetResistance(ResistanceType.Wk, Elements.Wind);
            Resistances.SetResistance(ResistanceType.Nu, Elements.Dark);
            Resistances.SetResistance(ResistanceType.Nu, Elements.Elec);

            var elecAll = SkillDatabase.ElecAll;
            var elecS = SkillDatabase.Elec1;
            var darkAll = SkillDatabase.DarkAll;
            var darkS = SkillDatabase.Dark1;
            var almighty = SkillDatabase.Almighty;
            var almightyS = SkillDatabase.Almighty1;
            var techUp = SkillDatabase.TechBuff;

            _scripts = new() 
            {
                { Elements.Wind, [almighty, darkS, elecAll]},
                { Elements.Light, [almighty, elecS, darkAll]},
                { Elements.Almighty, [almightyS, almighty, techUp]}
            };

            Skills.AddRange([elecAll, elecS, darkAll, darkS, almighty, almightyS, techUp ]);

            _currentScript = _scripts[Elements.Almighty];
            _move = 0;

            Description = $"{Name}: Will change its script based on which one of its weaknesses you hit:\n\n" +
                $"Wind: {string.Join(", ", _scripts[Elements.Wind])}\n\n" +
                $"Wind: {string.Join(", ", _scripts[Elements.Light])}\n\n" +
                $"Default: {string.Join(", ", _scripts[Elements.Almighty])}\n\n";
        }

        public override BattleResult ApplyElementSkill(BattleEntity user, ElementSkill skill)
        {
            BattleResult result = base.ApplyElementSkill(user, skill);

            if (result.ResultType == BattleResultType.Wk)
            {
                if (_scripts.ContainsKey(skill.Element))
                {
                    _currentScript = _scripts[skill.Element];
                }
                else
                {
                    _currentScript = _scripts[Elements.Almighty];
                }
            }

            return result;
        }

        public override EnemyAction GetNextAction(BattleSceneObject battleSceneObject)
        {
            EnemyAction action = new EnemyAction();
            action.Skill = _currentScript[_move];
            if (_currentScript[_move].Id != SkillId.Elemental)
            {
                action.Target = this;
            }
            else
            {
                action.Target = FindElementSkillTarget((ElementSkill)action.Skill, battleSceneObject);
            }
            _move++;

            if(_move >= _scripts.Count)
            {
                _move = 0;
            }

            return action;
        }

        public override void ResetEnemyState()
        {
            _currentScript = _scripts[Elements.Almighty];
            _move = 0;
        }
    }
}
