using AscendedZ.battle;
using AscendedZ.battle.battle_state_machine;
using AscendedZ.entities.battle_entities;
using AscendedZ.resistances;
using AscendedZ.skills;
using AscendedZ.statuses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.entities.enemy_objects.bosses
{
    public class SableCraft : Enemy
    {
        private const int DEFAULT_TURNS = 7;
        private const int TECH_TURNS = 4;
        private List<ISkill> _noTech, _techApplied;
        private List<ISkill> _currentScript;
        private int _move;

        public SableCraft() : base()
        {
            Name = EnemyNames.Sable_Craft;
            Image = CharacterImageAssets.GetImagePath(Name);
            MaxHP = EntityDatabase.GetBossHP(Name);
            Turns = DEFAULT_TURNS;
            _isBoss = true;

            Resistances.SetResistance(ResistanceType.Nu, Elements.Ice);
            Resistances.SetResistance(ResistanceType.Nu, Elements.Elec);

            var poison = SkillDatabase.PoisonAll;
            var eliric = SkillDatabase.Eliricpaul;
            var ice = SkillDatabase.Ice1;
            var saradwald = SkillDatabase.PierceIce1;
            var elec = SkillDatabase.Elec1;
            var iceAll = SkillDatabase.IceAll;
            var elecAll = SkillDatabase.ElecAll;
            var almighty = SkillDatabase.Almighty1;

            Skills.AddRange([saradwald, eliric, ice, elec, iceAll, elecAll, almighty]);

            _techApplied = [SkillDatabase.BeastEye, poison, ice, eliric, ice];
            _noTech = [saradwald, eliric, elec, iceAll, SkillDatabase.BeastEye, almighty, elecAll];
            _currentScript = _noTech;
            _move = 0;

            Description = $"{Name}: Will have 4 turns if you land a Weakness Technical/Technical hit, and 7 turns if you don't.";
        }

        public override List<ISkill> GetDisplaySkillList()
        {
            var list = base.GetDisplaySkillList();

            list.Add(SkillDatabase.BeastEye);
            list.Add(SkillDatabase.PoisonAll);

            return list;
        }

        public override BattleResult ApplyElementSkill(BattleEntity user, ElementSkill skill)
        {
            BattleResult result = base.ApplyElementSkill(user, skill);

            if (result.ResultType == BattleResultType.TechWk || result.ResultType == BattleResultType.Tech)
            {
                _currentScript = _techApplied;
                Turns = TECH_TURNS;
            }

            return result;
        }

        public override EnemyAction GetNextAction(BattleSceneObject battleSceneObject)
        {
            EnemyAction action = new EnemyAction();
            action.Skill = _currentScript[_move];
            if (_currentScript[_move].Id == SkillId.Elemental)
            {
                action.Target = FindElementSkillTarget((ElementSkill)action.Skill, battleSceneObject);
            }
            else if (_currentScript[_move].Id == SkillId.Eye)
            {
                action.Target = this;
            }
            else
            {
                if (action.Skill.Name.Contains("Poison"))
                {
                    if(FindPlayersUnaffectedByStatus(battleSceneObject, new PoisonStatus()).Count == 0)
                    {
                        action.Skill = Skills[6];
                        action.Target = FindElementSkillTarget((ElementSkill)action.Skill, battleSceneObject);
                    }
                    else
                    {
                        action.Target = FindTargetForStatus((StatusSkill)action.Skill, battleSceneObject);
                    }
                }
                else
                {
                    action.Target = FindTargetForStatus((StatusSkill)action.Skill, battleSceneObject);
                }
            }

            _move++;
            if (_move >= _currentScript.Count)
            {
                _move = 0;
            }

            return action;
        }

        public override void ResetEnemyState()
        {
            _currentScript = _noTech;
            _move = 0;
            Turns = DEFAULT_TURNS;
        }
    }
}
