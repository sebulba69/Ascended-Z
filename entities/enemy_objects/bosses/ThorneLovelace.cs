using AscendedZ.battle.battle_state_machine;
using AscendedZ.battle;
using AscendedZ.resistances;
using AscendedZ.skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AscendedZ.entities.battle_entities;

namespace AscendedZ.entities.enemy_objects.bosses
{
    public class ThorneLovelace : Enemy
    {
        private int _move;

        public ThorneLovelace() : base()
        {
            Name = EnemyNames.Thorne_Lovelace;
            Image = CharacterImageAssets.GetImagePath(Name);
            MaxHP = EntityDatabase.GetBossHP(Name);
            Turns = 9;
            _isBoss = true;

            var elec1 = SkillDatabase.Eliricpaul;
            var dark1 = SkillDatabase.OblivionsEmbrace;
            var elec2 = SkillDatabase.Elec1;
            var dark2 = SkillDatabase.Dark1;
            var antitich = SkillDatabase.Antitichton;
            var fire1 = SkillDatabase.Fredmuelald;
            var fire2 = SkillDatabase.Fire1;
            var light1 = SkillDatabase.Light1;
            var almighty = SkillDatabase.Almighty1;

            Resistances.SetResistance(ResistanceType.Nu, Elements.Dark);
            Resistances.SetResistance(ResistanceType.Dr, Elements.Elec);
            Resistances.SetResistance(ResistanceType.Wk, Elements.Wind);

            Skills.AddRange([elec1, dark1, antitich, elec2, dark2, fire1, light1, almighty, fire2]);
            _move = 0;

            Description = $"{Name}: Starts at 9 turns. Loses 1 turn for each time you hit its weakness. Turns reset to 9 the following turn after {Name} attacks";
        }

        public override BattleResult ApplyElementSkill(BattleEntity user, ElementSkill skill)
        {
            BattleResult result = base.ApplyElementSkill(user, skill);

            if (result.ResultType == BattleResultType.Wk || result.ResultType == BattleResultType.TechWk)
                Turns--;

            return result;
        }


        public override EnemyAction GetNextAction(BattleSceneObject battleSceneObject)
        {
            EnemyAction action = new EnemyAction();

            action.Skill = Skills[_move];
            action.Target = FindElementSkillTarget((ElementSkill)action.Skill, battleSceneObject);

            _move++;
            if (_move >= Skills.Count)
                _move = 0;

            return action;
        }

        public override void ResetEnemyState()
        {
            Turns = 9;
            _move = 0;
        }
    }
}
