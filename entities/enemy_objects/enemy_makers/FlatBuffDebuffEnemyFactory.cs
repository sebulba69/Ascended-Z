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
    public class FlatBuffDebuffEnemyFactory : EnemyFactory
    {
        private const int BASE_HP = 20;

        public FlatBuffDebuffEnemyFactory()
        {
            _functionDictionary[EnemyNames.Baos] = MakeBaos;
            _functionDictionary[EnemyNames.Cendros] = MakeCendros;
            _functionDictionary[EnemyNames.Rigratos] = MakeRigratos;
            _functionDictionary[EnemyNames.Zorliros] = MakeZorliros;
            _functionDictionary[EnemyNames.Zervos] = MakeZervos;
        }

        private Enemy MakeBaos()
        {
            var baos = MakeFlatBuffDebuffEnemy(EnemyNames.Baos, SkillDatabase.AtkBuff);

            baos.Resistances.SetResistance(ResistanceType.Rs, Elements.Fire);
            baos.Resistances.SetResistance(ResistanceType.Rs, Elements.Wind);

            baos.Skills.AddRange([SkillDatabase.Fire1.Clone(), SkillDatabase.Wind1.Clone()]);

            return baos;
        }

        private Enemy MakeCendros() 
        {
            var cendros = MakeFlatBuffDebuffEnemy(EnemyNames.Cendros, SkillDatabase.DefBuff);

            cendros.Resistances.SetResistance(ResistanceType.Rs, Elements.Ice);
            cendros.Resistances.SetResistance(ResistanceType.Rs, Elements.Elec);

            cendros.Skills.AddRange([SkillDatabase.Ice1.Clone(), SkillDatabase.Elec1.Clone()]);

            return cendros;
        }

        private Enemy MakeRigratos() 
        {
            var rigatros = MakeFlatBuffDebuffEnemy(EnemyNames.Rigratos, SkillDatabase.DefDebuff);

            rigatros.Resistances.SetResistance(ResistanceType.Wk, Elements.Dark);
            rigatros.Resistances.SetResistance(ResistanceType.Dr, Elements.Light);
            rigatros.Resistances.SetResistance(ResistanceType.Dr, Elements.Fire);

            rigatros.Skills.AddRange([SkillDatabase.FireAll.Clone(), SkillDatabase.LightAll.Clone()]);

            return rigatros;
        }

        private Enemy MakeZorliros() 
        {
            var zorliros = MakeFlatBuffDebuffEnemy(EnemyNames.Zorliros, SkillDatabase.DefBuff);

            zorliros.Resistances.SetResistance(ResistanceType.Wk, Elements.Wind);
            zorliros.Resistances.SetResistance(ResistanceType.Dr, Elements.Elec);
            zorliros.Resistances.SetResistance(ResistanceType.Dr, Elements.Dark);

            zorliros.Skills.AddRange([SkillDatabase.ElecAll.Clone(), SkillDatabase.DarkAll.Clone()]);

            return zorliros;
        }

        private Enemy MakeZervos()
        {
            var zervos = MakeFlatBuffDebuffEnemy(EnemyNames.Zervos, SkillDatabase.TechBuffAll);

            zervos.Resistances.SetResistance(ResistanceType.Wk, Elements.Ice);
            zervos.Resistances.SetResistance(ResistanceType.Dr, Elements.Fire);
            zervos.Resistances.SetResistance(ResistanceType.Dr, Elements.Wind);

            zervos.Skills.AddRange([SkillDatabase.WindAll.Clone(), SkillDatabase.FireAll.Clone()]);

            return zervos;
        }

        private Enemy MakeFlatBuffDebuffEnemy(string name, ISkill buffDebuff) 
        {
            return new FlatBuffDebuffEnemy()
            {
                Name = $"[FBDF] {name}",
                MaxHP = BASE_HP + _tierBoost,
                Image = CharacterImageAssets.GetImagePath(name),
                Resistances = new ResistanceArray(),
                FlatBuffDebuff = buffDebuff
            };
        }
    }
}
