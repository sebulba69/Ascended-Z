using AscendedZ.battle.battle_state_machine;
using AscendedZ.battle;
using AscendedZ.entities.enemy_objects.enemy_ais;
using AscendedZ.resistances;
using AscendedZ.skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.entities.enemy_objects.bosses
{
    public class EveritPickerin : Enemy
    {
        private List<int> _currentScript;
        private int _currentScriptIndex = 0;
        private int _scriptIndex = 0;

        private readonly List<int>[] _script = 
            [
                [0, 1, 2, 3, 4],
                [0, 1, 2, 5, 6],
                [7, 8, 11],
                [9, 10, 11]
            ];

        public EveritPickerin() : base()
        {
            Name = EnemyNames.Everit_Pickerin;
            Image = CharacterImageAssets.GetImagePath(Name);
            MaxHP = EntityDatabase.GetBossHP(Name);
            Turns = 3;
            _isBoss = true;

            Resistances.SetResistance(ResistanceType.Nu, Elements.Fire);
            Resistances.SetResistance(ResistanceType.Dr, Elements.Dark);
            Resistances.SetResistance(ResistanceType.Wk, Elements.Light);

            Skills.AddRange(
                [
                    SkillDatabase.BeastEye.Clone(), // 0
                    SkillDatabase.DefDebuff.Clone(), // 1
                    SkillDatabase.AtkBuff.Clone(), // 2
                    SkillDatabase.Wind1.Clone(), // 3
                    SkillDatabase.Fire1.Clone(), // 4
                    SkillDatabase.Ice1.Clone(), // 5
                    SkillDatabase.Elec1.Clone(), // 6
                    SkillDatabase.WindAll.Clone(), // 7
                    SkillDatabase.FireAll.Clone(), // 8
                    SkillDatabase.IceAll.Clone(), // 9
                    SkillDatabase.ElecAll.Clone(), // 10
                    SkillDatabase.TechBuff.Clone(), // 11
                ]);

            _currentScript = _script[_currentScriptIndex];

            Description = $"{Name}: Has four scripts it will cycle through (from top to bottom, one for each turn):\n";
            StringBuilder desc = new StringBuilder();
            int sCount = 1;
            foreach (var script in _script)
            {
                var skills = new List<ISkill>();
                for (int i = 0; i < script.Count; i++)
                {
                    skills.Add(Skills[script[i]]);
                }
                desc.Append($"{sCount}: {string.Join(", ", skills)}\n\n");
                sCount++;
            }
            Description += desc.ToString();
        }

        public override EnemyAction GetNextAction(BattleSceneObject battleSceneObject)
        {
            EnemyAction action = new EnemyAction();

            int skillIndex = _currentScript[_scriptIndex];

            action.Skill = Skills[skillIndex];

            if(action.Skill.Id == SkillId.Elemental)
            {
                action.Target = FindElementSkillTarget((ElementSkill)action.Skill, battleSceneObject);
            }
            else
            {
                action.Target = this;
            }

            _scriptIndex++;
            if(_scriptIndex >= _currentScript.Count)
            {
                _scriptIndex = 0;
                skillIndex = _currentScript[_scriptIndex];
                while (Skills[skillIndex].Id != SkillId.Elemental)
                {
                    _scriptIndex++;
                    if(_scriptIndex >= _currentScript.Count)
                        _scriptIndex = 0;

                    skillIndex = _currentScript[_scriptIndex];
                }
            }

            return action;
        }

        public override void ResetEnemyState()
        {
            _currentScriptIndex++;
            if (_currentScriptIndex >= _script.Length)
                _currentScriptIndex = 0;

            _currentScript = _script[_currentScriptIndex];
            _scriptIndex = 0;
        }
    }
}
