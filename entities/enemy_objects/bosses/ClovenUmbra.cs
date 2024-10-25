using AscendedZ.battle;
using AscendedZ.battle.battle_state_machine;
using AscendedZ.entities.battle_entities;
using AscendedZ.entities.enemy_objects.enemy_ais;
using AscendedZ.resistances;
using AscendedZ.skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.entities.enemy_objects.bosses
{
    public class ClovenUmbra : WeaknessHunterEnemy
    {
        private List<int> _script;
        private int _scriptPlacement;
        private bool _flip = true;

        public ClovenUmbra() : base()
        {
            Name = EnemyNames.Cloven_Umbra;
            Image = CharacterImageAssets.GetImagePath(Name);
            MaxHP = EntityDatabase.GetBossHP(Name);
            Turns = 1;
            _isBoss = true;

            Resistances.SetResistance(ResistanceType.Nu, Elements.Fire);
            Resistances.SetResistance(ResistanceType.Nu, Elements.Dark);
            Resistances.SetResistance(ResistanceType.Wk, Elements.Ice);
            Resistances.SetResistance(ResistanceType.Wk, Elements.Light);

            Skills.Add(SkillDatabase.Fire1.Clone());
            Skills.Add(SkillDatabase.Dark1.Clone());
            Skills.Add(SkillDatabase.Ice1.Clone());
            Skills.Add(SkillDatabase.Light1.Clone());
            Skills.Add(SkillDatabase.Spindlewarium.Clone());

            _scriptPlacement = 0;
            _script = new List<int>() { 0, 1 };

            Description = $"{Name}: Will check to see if you are weak to the opposite element of its weaknesses. If you are, it will use both opposite skills against you. If you aren't, it will heal itself.";
        }

        public override EnemyAction GetNextAction(BattleSceneObject battleSceneObject)
        {
            EnemyAction action = new EnemyAction();

            int index = _script[_scriptPlacement];
            var element = Skills[index] as ElementSkill;
            var wex = FindPlayersWithWeaknessToElement(battleSceneObject, element.Element);

            if(wex.Count == 0)
            {
                action.Skill = Skills[Skills.Count - 1];
                action.Target = this;
            }
            else
            {
                action.Skill = Skills[index];
                action.Target = wex[_rng.Next(wex.Count)];
            }

            _scriptPlacement++;

            if (_scriptPlacement == _script.Count)
                _scriptPlacement = 0;

            return action;
        }

        public override void ResetEnemyState()
        {
            _script.Clear();

            if (_flip)
            {
                Resistances.SetResistance(ResistanceType.Nu, Elements.Ice);
                Resistances.SetResistance(ResistanceType.Nu, Elements.Light);
                Resistances.SetResistance(ResistanceType.Wk, Elements.Fire);
                Resistances.SetResistance(ResistanceType.Wk, Elements.Dark);

                _script.Add(2);
                _script.Add(3);
            }
            else
            {
                Resistances.SetResistance(ResistanceType.Nu, Elements.Fire);
                Resistances.SetResistance(ResistanceType.Nu, Elements.Dark);
                Resistances.SetResistance(ResistanceType.Wk, Elements.Ice);
                Resistances.SetResistance(ResistanceType.Wk, Elements.Light);

                _script.Add(0);
                _script.Add(1);
            }

            _flip = !_flip;
            _scriptPlacement = 0;
            base.ResetEnemyState();
        }
    }
}
