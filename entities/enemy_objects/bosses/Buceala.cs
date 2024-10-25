using AscendedZ.battle.battle_state_machine;
using AscendedZ.battle;
using AscendedZ.skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AscendedZ.resistances;

namespace AscendedZ.entities.enemy_objects.bosses
{
    public class Buceala : Enemy
    {
        private int _phase;
        private ISkill _beastEye;
        private int _moveIndex;

        private List<ISkill> _currentScript;

        private List<ISkill> _phase1ScriptA, _phase1ScriptB, _phase2ScriptA, _phase2ScriptB, _phase3Script;

        public Buceala() : base()
        {
            Name = EnemyNames.Buceala;
            Image = CharacterImageAssets.GetImagePath(Name);
            MaxHP = EntityDatabase.GetBossHP(Name);
            Turns = 2;
            _isBoss = true;

            var silentPrayer = SkillDatabase.SilentPrayer;
            var wind = SkillDatabase.WindMadGod;
            var wind1 = SkillDatabase.Wind1;
            var windA = SkillDatabase.WindAll;
            var antitichton = SkillDatabase.Antitichton;
            var lusterCandy = SkillDatabase.LusterCandy;
            var wexAlmighty = SkillDatabase.FractalBeam;
            var voidElec = SkillDatabase.VoidElec;
            var torpefy = SkillDatabase.Torpefy;
            var pierceWind = SkillDatabase.PierceWind1;

            _beastEye = SkillDatabase.BeastEye;

            Skills.AddRange([wind, wind1, windA, antitichton, wexAlmighty, pierceWind]);
            _phase1ScriptA = [wind, antitichton];
            _phase1ScriptB = [silentPrayer, antitichton];
            _phase2ScriptA = [lusterCandy, wexAlmighty, wind1];
            _phase2ScriptB = [lusterCandy, _beastEye, wind1, windA];
            _phase3Script = [voidElec, _beastEye, windA, torpefy];

            Resistances.SetResistance(ResistanceType.Wk, Elements.Elec);
            Resistances.SetResistance(ResistanceType.Dr, Elements.Wind);
            Resistances.SetResistance(ResistanceType.Nu, Elements.Ice);

            _currentScript = _phase1ScriptA;

            Description = $"{Name}: Has 3 phases based on 60+%, 60%, 30% HP.\nIn phase 1, it will use Wind and Almighty attacks or will focus on clearing all debuffs if any are inflicted on it.\n\n" +
                $"In phase 2, it will focus on directly applying buffs to itself, then using an Almighty skill that forces weaknesses (if it wasn't debuffed at least once) or using Wind skills.\n\n" +
                $"In phase 3, it will focus on covering its weakness, using Wind attacks, and applying debuffs.";
        }

        public override List<ISkill> GetDisplaySkillList()
        {
            var list = base.GetDisplaySkillList();
            list.Add(_beastEye);
            list.Add(SkillDatabase.SilentPrayer);
            list.Add(SkillDatabase.LusterCandy);
            list.Add(SkillDatabase.Torpefy);
            list.Add(SkillDatabase.VoidElec);
            return list;
        }

        public override EnemyAction GetNextAction(BattleSceneObject battleSceneObject)
        {
            EnemyAction action = new EnemyAction();

            if(_phase == 0 && _currentScript != _phase1ScriptB)
            {
                var atk = StatusHandler.GetStatus(statuses.StatusId.AtkChangeStatus);
                var def = StatusHandler.GetStatus(statuses.StatusId.DefChangeStatus);

                if(atk.GetStacks() < 0 && def.GetStacks() < 0)
                {
                    _currentScript = _phase1ScriptB;
                }
            }

            if (_phase == 1 && _currentScript != _phase2ScriptB)
            {
                var atk = StatusHandler.GetStatus(statuses.StatusId.AtkChangeStatus);
                var def = StatusHandler.GetStatus(statuses.StatusId.DefChangeStatus);

                if (atk.GetStacks() < 0 && def.GetStacks() < 0)
                {
                    _currentScript = _phase2ScriptB;
                }
            }

            if (_moveIndex > _currentScript.Count)
                _moveIndex = _currentScript.Count - 1;

            action.Skill = _currentScript[_moveIndex];
            if (action.Skill.Id == SkillId.Elemental)
            {
                action.Target = FindElementSkillTarget((ElementSkill)action.Skill, battleSceneObject);
            }
            else
            {
                if(action.Skill.Id == SkillId.Eye)
                {
                    action.Target = this;
                }
                else
                {
                    action.Target = FindTargetForStatus((StatusSkill)action.Skill, battleSceneObject);
                }
            }

            _moveIndex++;
            if (_moveIndex > _currentScript.Count)
                _moveIndex = _currentScript.Count-1;

            return action;
        }

        public override void ResetEnemyState()
        {
            int percentage = (int)(((double)HP / MaxHP) * 100);

            if (percentage <= 60 && percentage >= 30)
            {
                _phase = 1;
                _currentScript = _phase2ScriptA;
            }
            else if (percentage < 30)
            {
                _phase = 2;
                _currentScript = _phase3Script;
            }
            else
            {
                _phase = 0;
                _currentScript = _phase1ScriptA;
            }


            _moveIndex = 0;
        }
    }
}
