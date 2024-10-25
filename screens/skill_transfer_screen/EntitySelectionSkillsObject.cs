using AscendedZ.entities;
using AscendedZ.entities.battle_entities;
using AscendedZ.entities.partymember_objects;
using AscendedZ.skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.screens.skill_transfer_screen
{
    public class EntitySelectionSkillsObject
    {
        private OverworldEntity _entity;

        public EntitySelectionSkillsObject(OverworldEntity entity)
        {
            _entity = entity;
        }

        public List<SkillIndexWrapper> GetSelectedSkills(int[] selected)
        {
            List<SkillIndexWrapper> skills = new List<SkillIndexWrapper>();

            foreach (int index in selected) 
            {
                SkillIndexWrapper wrapper = new SkillIndexWrapper() 
                {
                    Skill = _entity.Skills[index],
                    Index = index
                };

                skills.Add(wrapper);
            }

            return skills;
        }
    }
}
