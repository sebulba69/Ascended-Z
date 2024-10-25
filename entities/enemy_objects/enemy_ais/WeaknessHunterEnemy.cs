using AscendedZ.battle;
using AscendedZ.battle.battle_state_machine;
using AscendedZ.entities.battle_entities;
using AscendedZ.skills;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.entities.enemy_objects.enemy_ais
{
    /// <summary>
    /// An enemy who targets the first instance of a party member with a weakness to a skill it has.
    /// </summary>
    public class WeaknessHunterEnemy : AlternatingEnemy
    {
        public WeaknessHunterEnemy() : base()
        {
            Description = "[WEX] - Weakness Hunter Enemy: Will always pick characters who are weak to a skill they possess. If a weakness isn't present, they will attack randomly.";
        }

        public override EnemyAction GetNextAction(BattleSceneObject battleSceneObject)
        {
            // this is the default action that'll be used if no one has a weakness to anything this enemy has
            EnemyAction action = base.GetNextAction(battleSceneObject);

            List<BattlePlayer> players = battleSceneObject.AlivePlayers;

            foreach(var skill in Skills)
            {
                if(skill.Id == SkillId.Elemental)
                {
                    ElementSkill elementSkill = (ElementSkill)skill;
                    
                    // Agro Status overrides this enemy's AI
                    if (action.Target.StatusHandler.HasStatus(statuses.StatusId.AgroStatus))
                    {
                        if (action.Target.Resistances.IsWeakToElement(elementSkill.Element))
                        {
                            action.Skill = elementSkill;
                            break;
                        }     
                    }
                    else
                    {
                        List<BattlePlayer> weakToElement = players.FindAll(player => player.Resistances.IsWeakToElement(elementSkill.Element));
                        if (weakToElement.Count != 0 && !_isAgroOverride)
                        {
                            int totalTargets = weakToElement.Count;
                            action.Target = weakToElement[_rng.Next(totalTargets)];
                            action.Skill = elementSkill;
                            break;
                        }
                    }
                }
            }

            return action;
        }
    }
}
