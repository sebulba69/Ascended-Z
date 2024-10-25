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
    public class AshenAsh : Enemy
    {
        private int _phase;
        private bool _randomAttack;

        // phase, skill
        private Dictionary<int, ElementSkill> elements;

        private readonly Elements[] _weaknesses = { Elements.Wind, Elements.Ice, Elements.Light, Elements.Fire };

        public AshenAsh() : base()
        {
            Name = EnemyNames.Ashen_Ash;
            Image = CharacterImageAssets.GetImagePath(Name);
            MaxHP = EntityDatabase.GetBossHP(Name);
            Turns = 2;
            _isBoss = true;
            _randomAttack = true;

            _phase = 0;

            Skills.Add(SkillDatabase.WindAll.Clone());
            Skills.Add(SkillDatabase.Ice1.Clone());
            Skills.Add(SkillDatabase.LightAll.Clone());
            Skills.Add(SkillDatabase.Fire1.Clone());

            Description = $"{Name}: Each time {Name} makes or receives an attack, its resistances will shift. The element it nulls is always the element it will use the following turn. These resistances shift in the following order: {string.Join(',', _weaknesses)}";
        }

        public override BattleResult ApplyElementSkill(BattleEntity user, ElementSkill skill)
        {
            var result = base.ApplyElementSkill(user, skill);

            IncrementPhase();

            return result;
        }

        public override EnemyAction GetNextAction(BattleSceneObject battleSceneObject)
        {
            EnemyAction action = new EnemyAction { Skill = Skills[_phase] };

            var agro = GetTargetAffectedByAgro(battleSceneObject);

            if (_isAgroOverride)
            {
                action.Target = agro;
            }
            else
            {
                List<BattlePlayer> partyMembers = battleSceneObject.AlivePlayers;
                action.Target = partyMembers[_rng.Next(partyMembers.Count)];
            }

            IncrementPhase();
            return action;
        }

        private void IncrementPhase()
        {
            Resistances.SetResistance(ResistanceType.None, _weaknesses[_phase]);
            Resistances.SetResistance(ResistanceType.None, SkillDatabase.ElementalOpposites[_weaknesses[_phase]]);

            _phase++;
            if (_phase == Skills.Count)
                _phase = 0;

            Resistances.SetResistance(ResistanceType.Nu, _weaknesses[_phase]);
            Resistances.SetResistance(ResistanceType.Wk, SkillDatabase.ElementalOpposites[_weaknesses[_phase]]);
        }

        public override void ResetEnemyState() 
        {
        }
    }
}
