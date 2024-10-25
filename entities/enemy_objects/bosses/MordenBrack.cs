using AscendedZ.battle.battle_state_machine;
using AscendedZ.battle;
using AscendedZ.resistances;
using AscendedZ.skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.entities.enemy_objects.bosses
{
    public class MordenBrack : Enemy
    {
        private int _scriptIndex, _moveIndex;
        private List<List<ISkill>> _scripts;
        public MordenBrack() : base()
        {
            Name = EnemyNames.Morden_Brack;
            Image = CharacterImageAssets.GetImagePath(Name);
            MaxHP = EntityDatabase.GetBossHP(Name);
            Turns = 3;
            _isBoss = true;
            _scripts = new List<List<ISkill>>();

            var sealAll = SkillDatabase.SealAll.Clone();
            var bindAll = SkillDatabase.BindAll.Clone();
            var debilitate = SkillDatabase.Debilitate.Clone();
            var lusterCandy = SkillDatabase.LusterCandy.Clone();
            var antitichton = SkillDatabase.Antitichton.Clone();
            var almightyAll = SkillDatabase.Almighty.Clone();
            var windAll = SkillDatabase.WindAll.Clone();
            var darkAll = SkillDatabase.DarkAll.Clone();

            Resistances.SetResistance(ResistanceType.Wk, Elements.Light);
            Resistances.SetResistance(ResistanceType.Dr, Elements.Dark);
            Resistances.SetResistance(ResistanceType.Dr, Elements.Wind);

            Skills.AddRange([antitichton, almightyAll, windAll, darkAll]);

            List<ISkill> turn1 = [sealAll, debilitate, antitichton];
            List<ISkill> turn2 = [almightyAll, windAll, darkAll];
            List<ISkill> turn3 = [lusterCandy, lusterCandy, bindAll];

            _scripts.AddRange([ turn1, turn2, turn3, turn2 ]);

            _scriptIndex = 0;
            _moveIndex = 0;

            Description = $"{Name}: Has four scripts it will cycle through (from top to bottom, one for each turn):\n";
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
            list.Add(SkillDatabase.Debilitate);
            list.Add(SkillDatabase.SealAll);
            list.Add(SkillDatabase.BindAll);

            return list;
        }

        public override EnemyAction GetNextAction(BattleSceneObject battleSceneObject)
        {
            EnemyAction action = new EnemyAction();

            action.Skill = _scripts[_scriptIndex][_moveIndex];
            if(action.Skill.Id == SkillId.Elemental)
            {
                action.Target = FindElementSkillTarget((ElementSkill)action.Skill, battleSceneObject);
            }
            else
            {
                action.Target = FindTargetForStatus((StatusSkill)action.Skill, battleSceneObject);
            }

            _moveIndex++;
            if(_moveIndex > _scripts[_scriptIndex].Count)
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
