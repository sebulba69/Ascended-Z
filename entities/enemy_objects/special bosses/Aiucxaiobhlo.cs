using AscendedZ.battle;
using AscendedZ.battle.battle_state_machine;
using AscendedZ.entities.battle_entities;
using AscendedZ.resistances;
using AscendedZ.skills;
using AscendedZ.statuses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.entities.enemy_objects.special_bosses
{
    public class Aiucxaiobhlo : Enemy
    {
        private int _turnCount;
        private int _index = 0;
        private bool _massDeathCast;
        private ISkill _markOfDeath;
        public Aiucxaiobhlo() : base()
        {
            Name = EnemyNames.Aiucxaiobhlo;
            Image = CharacterImageAssets.GetImagePath(Name);
            MaxHP = 12000;
            _isBoss = true;
            Turns = 4;
            _turnCount = 3;
            Resistances.SetResistance(ResistanceType.Wk, Elements.Fire);

            Resistances.SetResistance(ResistanceType.Dr, Elements.Wind);
            Resistances.SetResistance(ResistanceType.Dr, Elements.Ice);
            Resistances.SetResistance(ResistanceType.Dr, Elements.Elec);

            _markOfDeath = SkillDatabase.MarkOfDeathSingle;

            var mgElec = SkillDatabase.ElecMadGod;
            var mgIce = SkillDatabase.IceMadGod;
            var mgWind = SkillDatabase.DarkMadGod;
            var ancientChoir = SkillDatabase.AncientChoir;

            Skills.AddRange([mgElec, mgIce, mgWind, ancientChoir]);

            Description = $"{Name}: Oh-ho-ho! Caught red-handed, are we? Trying to peek behind the curtain of my little act, eh? Tsk, tsk! You thought you could cheat the jester's dance? Well, no spoilers here, dear Ascended. Prepare to learn by the sting of the blade and the whimsy of chance!";
        }

        public override List<ISkill> GetDisplaySkillList()
        {
            var list = base.GetDisplaySkillList();
            list.Add(SkillDatabase.AncientChoir);
            list.Add(_markOfDeath);
            return list;
        }

        public override EnemyAction GetNextAction(BattleSceneObject battleSceneObject)
        {
            EnemyAction action;
            if (_turnCount >= 3)
            {
                if (_index == 1)
                    _turnCount = 0;

                var unaffected = FindPlayersUnaffectedByStatus(battleSceneObject, new MarkOfDeathStatus());

                if(unaffected.Count > 0)
                {
                    action = new EnemyAction()
                    {
                        Skill = _markOfDeath,
                        Target = unaffected[_rng.Next(unaffected.Count)]
                    };
                }
                else
                {
                    action = new EnemyAction()
                    {
                        Skill = Skills[_index],
                        Target = FindTarget(Skills[_index], battleSceneObject)
                    };

                }
            }
            else
            {
                action = new EnemyAction()
                {
                    Skill = Skills[_index],
                    Target = FindTarget(Skills[_index], battleSceneObject)
                };
            }

            _index++;
            if (_index >= Skills.Count)
                _index = 0;

            return action;
        }

        private BattleEntity FindTarget(ISkill skill, BattleSceneObject battleSceneObject)
        {
            BattleEntity target;
            if (skill.Id == SkillId.Elemental)
            {
                target = FindElementSkillTarget((ElementSkill)skill, battleSceneObject);
            }
            else
            {
                if (skill.Id == SkillId.Eye)
                {
                    target = this;
                }
                else
                {
                    target = FindTargetForStatus((StatusSkill)skill, battleSceneObject);
                }
            }

            if (target == null)
                target = this;

            return target;
        }

        public override void ResetEnemyState()
        {
            _index = 0;
            _turnCount++;
        }
    }
}