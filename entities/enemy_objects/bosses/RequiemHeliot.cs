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
using AscendedZ.statuses;

namespace AscendedZ.entities.enemy_objects.bosses
{
    public class RequiemHeliot : Enemy
    {
        private List<ISkill> _currentScript, _singleScript, _multiScript;
        private int _move;

        public RequiemHeliot() : base()
        {
            Name = EnemyNames.Requiem_Heliot;
            Image = CharacterImageAssets.GetImagePath(Name);
            MaxHP = EntityDatabase.GetBossHP(Name);
            Turns = 3;
            _isBoss = true;

            Resistances.SetResistance(ResistanceType.Wk, Elements.Fire);
            Resistances.SetResistance(ResistanceType.Nu, Elements.Wind);
            Resistances.SetResistance(ResistanceType.Nu, Elements.Elec);
            Resistances.SetResistance(ResistanceType.Dr, Elements.Ice);


            var elecS = SkillDatabase.Elec1;
            var windS = SkillDatabase.Wind1;
            var iceS = SkillDatabase.Ice1;

            var elecAll = SkillDatabase.ElecAll;
            var windAll = SkillDatabase.WindAll;
            var iceAll = SkillDatabase.IceAll;

            Skills.AddRange([elecS, windS, iceS, elecAll, windAll, iceAll ]);

            _singleScript = [windS, iceS, elecS];
            _multiScript = [windAll, iceAll, elecAll];

            _currentScript = _singleScript;
            _move = 0;

            Description = $"{Name}: If you deal over 50,000 damage, it will use multi-hit elemental skills instead of single-hit elemental skills.";
        }

        public override List<ISkill> GetDisplaySkillList()
        {
            var list = base.GetDisplaySkillList();
            list.Add(SkillDatabase.Jyndesdarth);
            return list;
        }

        public override EnemyAction GetNextAction(BattleSceneObject battleSceneObject)
        {
            EnemyAction action = new EnemyAction();

            if(_move == 0 && !StatusHandler.HasStatus(StatusId.Jyndesdarth))
            {
                action.Skill = SkillDatabase.Jyndesdarth;
                action.Target = this;
            }
            else
            {
                if (StatusHandler.HasStatus(StatusId.Jyndesdarth))
                {
                    var jyndes = StatusHandler.GetStatus(StatusId.Jyndesdarth);
                    if(jyndes.Active)
                    {
                        jyndes.ClearStatus();
                        _currentScript = _multiScript;
                    }
                }

                action.Skill = _currentScript[_move];
                action.Target = FindElementSkillTarget((ElementSkill)action.Skill, battleSceneObject);
            }

            _move++;
            if(_move >= _currentScript.Count)
            {
                _move = 0;
            }

            return action;
        }

        public override void ResetEnemyState()
        {
            _currentScript = _singleScript;
            _move = 0;
        }
    }
}
