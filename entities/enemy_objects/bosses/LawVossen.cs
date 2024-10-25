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

namespace AscendedZ.entities.enemy_objects.bosses
{
    public class LawVossen : Enemy
    {
        private int _scriptIndex, _moveIndex;
        private List<List<ISkill>> _scripts;

        public LawVossen() : base()
        {
            Name = EnemyNames.Law_Vossen;
            Image = CharacterImageAssets.GetImagePath(Name);
            MaxHP = EntityDatabase.GetBossHP(Name);
            Turns = 3;
            _isBoss = true;

            var stun = SkillDatabase.Stun;
            var poisonAll = SkillDatabase.PoisonAll;
            var lusterCandy = SkillDatabase.LusterCandy;
            var techPlus = SkillDatabase.TechBuff;

            var fire1 = SkillDatabase.Fire1;
            var elec1 = SkillDatabase.Elec1;
            var almighty = SkillDatabase.Almighty;
            var heal = SkillDatabase.Heal1;

            Resistances.SetResistance(ResistanceType.Nu, Elements.Elec);
            Resistances.SetResistance(ResistanceType.Dr, Elements.Fire);
            Resistances.SetResistance(ResistanceType.Wk, Elements.Ice);

            _scripts = new();

            Skills.AddRange([fire1, elec1, almighty, heal]);

            List<ISkill> turn1 = [stun, stun, poisonAll];
            List<ISkill> turn2 = [lusterCandy, lusterCandy, techPlus];
            List<ISkill> turn3 = [fire1, techPlus, elec1, techPlus, almighty, heal];

            _scripts.AddRange([turn1, turn2, turn3]);

            _scriptIndex = 0;
            _moveIndex = 0;

            Description = $"{Name}: Has three scripts it will cycle through (from top to bottom, one for each turn):\n";
            StringBuilder desc = new StringBuilder();
            int sCount = 1;
            foreach (var script in _scripts)
            {
                desc.Append($"{sCount}: {string.Join(", ", script)}\n\n");
                sCount++;
            }
            Description += desc.ToString();
        }

        public override List<ISkill> GetDisplaySkillList()
        {
            var list = base.GetDisplaySkillList();

            list.Add(SkillDatabase.LusterCandy);
            list.Add(SkillDatabase.TechBuff);
            list.Add(SkillDatabase.Stun);
            list.Add(SkillDatabase.PoisonAll);

            return list;
        }

        public override EnemyAction GetNextAction(BattleSceneObject battleSceneObject)
        {
            EnemyAction action = new EnemyAction();

            action.Skill = _scripts[_scriptIndex][_moveIndex];
            if (action.Skill.Id == SkillId.Elemental)
            {
                action.Target = FindElementSkillTarget((ElementSkill)action.Skill, battleSceneObject);
            }
            else if(action.Skill.Id == SkillId.Healing)
            {
                action.Target = this;
            }
            else
            {
                var statusSkill = (StatusSkill)action.Skill;
                if (action.Skill.Name.Contains("Stun"))
                {
                    var targets = FindPlayersUnaffectedByStatus(battleSceneObject, statusSkill.Status);
                    action.Target = targets[_rng.Next(targets.Count)];
                }
                else
                {
                    action.Target = FindTargetForStatus(statusSkill, battleSceneObject);
                }
                
            }

            _moveIndex++;
            if (_moveIndex > _scripts[_scriptIndex].Count)
                _moveIndex = 0;

            return action;
        }

        public override void ResetEnemyState()
        {
            _moveIndex = 0;
            _scriptIndex++;
            if (_scriptIndex >= _scripts.Count)
            {
                _scriptIndex = 0;
            }
        }
    }
}
