using AscendedZ.entities.enemy_objects.enemy_ais;
using AscendedZ.resistances;
using AscendedZ.skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.entities.enemy_objects.enemy_makers
{
    public class BuffEnemyFactory : EnemyFactory
    {
        public BuffEnemyFactory()
        {
            _functionDictionary[EnemyNames.Anrol] = MakeAnrol;
            _functionDictionary[EnemyNames.David] = MakeDavid;
            _functionDictionary[EnemyNames.Nanfrea] = MakeNanfrea;
            _functionDictionary[EnemyNames.Ferza] = MakeFerza;
            _functionDictionary[EnemyNames.Wennald] = MakeWennald;
            _functionDictionary[EnemyNames.Garcar] = MakeGarcar;
        }

        public Enemy MakeAnrol()
        {
            var anrol = MakeBuffEnemy(EnemyNames.Anrol, 10);

            anrol.Resistances.SetResistance(ResistanceType.Rs, Elements.Ice);

            anrol.Skills.Add(SkillDatabase.IceBuff1.Clone());
            anrol.Skills.Add(SkillDatabase.Ice1.Clone());

            return anrol;
        }

        public Enemy MakeFerza()
        {
            var ferza = MakeBuffEnemy(EnemyNames.Ferza, 10);

            ferza.Resistances.SetResistance(ResistanceType.Rs, Elements.Elec);
            ferza.Resistances.SetResistance(ResistanceType.Wk, Elements.Wind);

            ferza.Skills.Add(SkillDatabase.ElecBuff1.Clone());
            ferza.Skills.Add(SkillDatabase.Elec1.Clone());

            return ferza;
        }

        public Enemy MakeDavid()
        {
            var david = MakeBuffEnemy(EnemyNames.David, 10);

            david.Resistances.SetResistance(ResistanceType.Rs, Elements.Wind);
            david.Resistances.SetResistance(ResistanceType.Wk, Elements.Elec);

            david.Skills.Add(SkillDatabase.WindBuff1.Clone());
            david.Skills.Add(SkillDatabase.Wind1.Clone());

            return david;
        }

        public Enemy MakeNanfrea()
        {
            var nanfrea = MakeBuffEnemy(EnemyNames.Nanfrea, 10);

            nanfrea.Resistances.SetResistance(ResistanceType.Rs, Elements.Fire);
            nanfrea.Resistances.SetResistance(ResistanceType.Wk, Elements.Ice);

            nanfrea.Skills.Add(SkillDatabase.FireBuff1.Clone());
            nanfrea.Skills.Add(SkillDatabase.Fire1.Clone());

            return nanfrea;
        }

        public Enemy MakeWennald()
        {
            var wennald = MakeBuffEnemy(EnemyNames.Wennald, 15);

            wennald.Resistances.SetResistance(ResistanceType.Dr, Elements.Light);

            wennald.Skills.Add(SkillDatabase.LightBuff1.Clone());
            wennald.Skills.Add(SkillDatabase.LightAll.Clone());

            return wennald;
        }

        public Enemy MakeGarcar()
        {
            var garcar = MakeBuffEnemy(EnemyNames.Garcar, 15);

            garcar.Resistances.SetResistance(ResistanceType.Dr, Elements.Dark);

            garcar.Skills.Add(SkillDatabase.DarkBuff1.Clone());
            garcar.Skills.Add(SkillDatabase.DarkAll.Clone());

            return garcar;
        }

        private Enemy MakeBuffEnemy(string name, int hp)
        {
            return new BuffEnemy
            {
                Name = $"[BUFF] {name}",
                MaxHP = hp + _tierBoost,
                Image = CharacterImageAssets.GetImagePath(name),
                Resistances = new ResistanceArray()
            };
        }
    }
}
