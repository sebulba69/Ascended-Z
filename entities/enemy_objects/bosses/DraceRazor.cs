using AscendedZ.battle;
using AscendedZ.entities.battle_entities;
using AscendedZ.entities.enemy_objects.enemy_ais;
using AscendedZ.resistances;
using AscendedZ.skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Godot.WebSocketPeer;

namespace AscendedZ.entities.enemy_objects.bosses
{
    public class DraceRazor : AlternatingEnemy
    {
        private const int BASE_TURNS = 2;
        private int _additionalTurns = 0;
        
        public DraceRazor() : base()
        {
            _isBoss = true;

            Name = EnemyNames.Drace_Razor;
            MaxHP = EntityDatabase.GetBossHP(Name);
            Image = CharacterImageAssets.GetImagePath(Name);

            Resistances = new ResistanceArray();

            Resistances.SetResistance(ResistanceType.Wk, Elements.Dark);
            Resistances.SetResistance(ResistanceType.Dr, Elements.Light);

            AddNoWexSkills(true);

            Turns = BASE_TURNS;

            Description = $"{Name}: Gains a turn (goes back to normal after its turn) each time you hit its weakness. If its weakness is not hit, it will use all-hit elemental skills. If its weakness is hit, it will use single-hit elemental skills.";
        }

        public override BattleResult ApplyElementSkill(BattleEntity user, ElementSkill skill)
        {
            BattleResult result = base.ApplyElementSkill(user, skill);

            if (result.ResultType == BattleResultType.Wk)
            {
                _additionalTurns++;
                Turns = BASE_TURNS + _additionalTurns;
                AddWexSkills();
            }

            return result;
        }

        private void AddWexSkills()
        {
            Skills.Clear();

            Skills.Add(SkillDatabase.Fire1.Clone());
            Skills.Add(SkillDatabase.Wind1.Clone());
            Skills.Add(SkillDatabase.Elec1.Clone());
            Skills.Add(SkillDatabase.Ice1.Clone());

            int tierBoost = EntityDatabase.GetTierBoost(100);
            Boost(100 + tierBoost, true);
        }

        private void AddNoWexSkills(bool skipLevelUp = false)
        {
            Skills.Clear();

            Skills.Add(SkillDatabase.FireAll.Clone());
            Skills.Add(SkillDatabase.WindAll.Clone());
            Skills.Add(SkillDatabase.ElecAll.Clone());
            Skills.Add(SkillDatabase.IceAll.Clone());

            if (!skipLevelUp)
            {
                int tierBoost = EntityDatabase.GetTierBoost(100);
                Boost(100 + tierBoost, true);
            }
        }

        public override void ResetEnemyState()
        {
            _additionalTurns = 0;
            Turns = BASE_TURNS;
            AddNoWexSkills();
        }
    }
}
