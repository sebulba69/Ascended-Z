using AscendedZ.battle;
using AscendedZ.entities.sigils;
using AscendedZ.resistances;
using AscendedZ.skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.entities.battle_entities
{
    /// <summary>
    /// We inherit from BattleEntity because we want to have different classes to differentiate our
    /// enemies from our players. It's just cleaner this way.
    /// </summary>
    public class BattlePlayer : BattleEntity
    {
        public BattlePlayer(): base() 
        {
            Type = EntityType.Player;
            Turns = 1;
        }

        public void ApplySigil(Sigil sigil)
        {
            if(sigil.StatIndex == 0)
            {
                MaxHP = Equations.ApplyIntegerBoostPercentage(MaxHP, sigil.BoostPercentage);
            }
            else
            {
                foreach(var skill in Skills)
                {
                    if(skill.Id == SkillId.Elemental)
                    {
                        var element = (ElementSkill)skill;
                        element.ApplySigil(sigil);
                    }
                }
            }
        }
    }
}
