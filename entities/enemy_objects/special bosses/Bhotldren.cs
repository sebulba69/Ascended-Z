using AscendedZ.battle;
using AscendedZ.battle.battle_state_machine;
using AscendedZ.entities.battle_entities;
using AscendedZ.resistances;
using AscendedZ.skills;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.entities.enemy_objects.special_bosses
{
    public class Bhotldren : Enemy
    {
        private HashSet<string> _players;
        private ISkill _confuse;
        private BattleEntity[] _targets;
        private int _index = 0;

        public Bhotldren() : base()
        {
            Name = EnemyNames.Bhotldren;
            Image = CharacterImageAssets.GetImagePath(Name);
            MaxHP = 9000;
            _isBoss = true;
            Turns = 4;

            Resistances.SetResistance(ResistanceType.Wk, Elements.Light);
            
            Resistances.SetResistance(ResistanceType.Dr, Elements.Dark);
            Resistances.SetResistance(ResistanceType.Dr, Elements.Elec);
            Resistances.SetResistance(ResistanceType.Nu, Elements.Wind);
            Resistances.SetResistance(ResistanceType.Nu, Elements.Ice);

            _confuse = SkillDatabase.Confusion;
            var mgDark = SkillDatabase.DarkMadGodAllP;
            var mgElec = SkillDatabase.ElecMadGodAllP;
            var mgIce = SkillDatabase.IceMadGodAll;
            var mgWind = SkillDatabase.WindMadGodAll;

            _players = new HashSet<string>();

            Skills.AddRange([mgDark, mgElec, mgIce, mgWind]);

            Description = $"{Name}: ...";
        }

        public override List<ISkill> GetDisplaySkillList()
        {
           var list= base.GetDisplaySkillList();
            list.Add(_confuse);
            return list;
        }

        public override BattleResult ApplyElementSkill(BattleEntity user, ElementSkill skill)
        {
            BattleResult result = base.ApplyElementSkill(user, skill);

            if(result.ResultType != BattleResultType.Nu && result.ResultType != BattleResultType.Dr)
            {
                if (!_players.Contains(user.BaseName))
                {
                    _players.Add(user.BaseName);
                }
            }

            return result;
        }

        public override EnemyAction GetNextAction(BattleSceneObject battleSceneObject)
        {
            var players = battleSceneObject.Players;
            EnemyAction action = null;

            if (_index < players.Count)
            {
                if (_players.Contains(players[_index].BaseName) 
                    && players[_index].HP > 0 
                    && !players[_index].StatusHandler.HasStatus(statuses.StatusId.Confusion)) 
                {
                    action = new EnemyAction()
                    {
                        Skill = _confuse,
                        Target = players[_index]
                    };
                }
                else
                {
                    action = new EnemyAction()
                    {
                        Skill = Skills[_index],
                        Target = FindTarget(Skills[_index], battleSceneObject)
                    };
                }
            }
            else
            {
                action = new EnemyAction()
                {
                    Skill = Skills[_index],
                    Target = FindTarget(Skills[_index], battleSceneObject)
                };
            }

            _index++;
            if (_index >= Skills.Count)
            {
                _index = 0;
            }

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

        public override void ResetEnemyState()
        {
            _index = 0;
            _players.Clear();
        }
    }
}
