using AscendedZ.entities;
using AscendedZ.entities.battle_entities;
using AscendedZ.entities.partymember_objects;
using AscendedZ.skills;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.screens.upgrade_screen.skill_reorder
{
    public class SkillReorderControlObject
    {
        private OverworldEntity _entity;
        private BattlePlayer _bp;
        private List<ISkill> _reorderedSkills;

        public List<ISkill> EntitySkills { get => _bp.Skills; }
        public List<ISkill> DisplaySkills { get; set; }
        public SkillReorderControlObject(OverworldEntity entity)
        {
            _entity = entity;
            _bp = entity.MakeBattlePlayerBase();
            _reorderedSkills = new List<ISkill>();
            DisplaySkills = new();
        }

        public bool IsItemAlreadyAdded(int index)
        {
            return _reorderedSkills.Contains(_entity.Skills[index]);
        }

        public bool CanReorder()
        {
            return _reorderedSkills.Count == _entity.Skills.Count;
        }

        public void ReorderSkills()
        {
            _entity.Skills = new List<ISkill>(_reorderedSkills);
            _reorderedSkills.Clear();
            DisplaySkills.Clear();
            _bp = _entity.MakeBattlePlayerBase();
        }

        public void AddSkill(int index)
        {
            _reorderedSkills.Add(_entity.Skills[index]);
            DisplaySkills.Add(_bp.Skills[index]);
        }

        public void RemoveSkill(int index) 
        {
            _reorderedSkills.Remove(_entity.Skills[index]);
            DisplaySkills.Remove(_bp.Skills[index]);
        }
    }
}
