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
    public class ResistanceChangerEnemyFactory : EnemyFactory
    {
        public ResistanceChangerEnemyFactory()
        {
            _functionDictionary[EnemyNames.Thony] = MakeThony;
            _functionDictionary[EnemyNames.Conson] = MakeConson;
            _functionDictionary[EnemyNames.Hugline] = MakeHugline;
            _functionDictionary[EnemyNames.Danyll] = MakeDanyll;
            _functionDictionary[EnemyNames.Jiochroudice] = MakeJiochroudice;
            _functionDictionary[EnemyNames.Bazzaelth] = MakeBazzaelth;
            _functionDictionary[EnemyNames.Culdra] = MakeCuldra;
            _functionDictionary[EnemyNames.Lord] = MakeLord;
        }

        public Enemy MakeThony()
        {
            string name = EnemyNames.Thony;
            int hp = 12;
            Elements resist1 = Elements.Ice;
            Elements resist2 = Elements.Fire;

            var thony = MakeResistanceChangerEnemy(name, hp, resist1, resist2);

            thony.Skills.Add(SkillDatabase.Ice1.Clone());
            thony.Skills.Add(SkillDatabase.Fire1.Clone());

            return thony;
        }

        public Enemy MakeConson()
        {
            string name = EnemyNames.Conson;
            int hp = 12;
            Elements resist1 = Elements.Light;
            Elements resist2 = Elements.Dark;

            var conson = MakeResistanceChangerEnemy(name, hp, resist1, resist2);
            
            conson.Skills.Add(SkillDatabase.Light1.Clone());
            conson.Skills.Add(SkillDatabase.Dark1.Clone());

            return conson;
        }

        public Enemy MakeHugline()
        {
            string name = EnemyNames.Hugline;
            int hp = 23;
            Elements resist1 = Elements.Wind;
            Elements resist2 = Elements.Elec;

            var hugline = MakeResistanceChangerEnemyAdvanced(name, hp, resist1, resist2);

            hugline.Skills.Add(SkillDatabase.Wind1.Clone());
            hugline.Skills.Add(SkillDatabase.Elec1.Clone());

            return hugline;
        }

        public Enemy MakeDanyll()
        {
            string name = EnemyNames.Danyll;
            int hp = 23;
            Elements resist1 = Elements.Light;
            Elements resist2 = Elements.Dark;

            var danyll = MakeResistanceChangerEnemyAdvanced(name, hp, resist1, resist2);

            danyll.Skills.Add(SkillDatabase.Dark1.Clone());
            danyll.Skills.Add(SkillDatabase.Light1.Clone());

            return danyll;
        }

        public Enemy MakeJiochroudice()
        {
            string name = EnemyNames.Jiochroudice;
            int hp = 23;
            Elements resist1 = Elements.Ice;
            Elements resist2 = Elements.Fire;

            var jiochroudice = MakeResistanceChangerEnemyAdvanced(name, hp, resist1, resist2);

            jiochroudice.Skills.Add(SkillDatabase.Ice1.Clone());
            jiochroudice.Skills.Add(SkillDatabase.Fire1.Clone());

            return jiochroudice;
        }

        public Enemy MakeBazzaelth()
        {
            string name = EnemyNames.Bazzaelth;
            int hp = 30;
            Elements resist1 = Elements.Wind;
            Elements resist2 = Elements.Elec;

            var resistanceChangerEnemyAdv = MakeResistanceChangerEnemyAdvanced(name, hp, resist1, resist2);

            resistanceChangerEnemyAdv.Skills.Add(SkillDatabase.WindMadGodAll.Clone());
            resistanceChangerEnemyAdv.Skills.Add(SkillDatabase.ElecMadGodAll.Clone());

            return resistanceChangerEnemyAdv;
        }

        public Enemy MakeCuldra()
        {
            string name = EnemyNames.Culdra;
            int hp = 30;
            Elements resist1 = Elements.Fire;
            Elements resist2 = Elements.Ice;

            var resistanceChangerEnemyAdv = MakeResistanceChangerEnemyAdvanced(name, hp, resist1, resist2);

            resistanceChangerEnemyAdv.Skills.Add(SkillDatabase.FireMadGodAll.Clone());
            resistanceChangerEnemyAdv.Skills.Add(SkillDatabase.IceMadGodAll.Clone());

            return resistanceChangerEnemyAdv;
        }

        public Enemy MakeLord()
        {
            string name = EnemyNames.Lord;
            int hp = 30;
            Elements resist1 = Elements.Dark;
            Elements resist2 = Elements.Light;

            var resistanceChangerEnemyAdv = MakeResistanceChangerEnemyAdvanced(name, hp, resist1, resist2);

            resistanceChangerEnemyAdv.Skills.Add(SkillDatabase.DarkMadGodAll.Clone());
            resistanceChangerEnemyAdv.Skills.Add(SkillDatabase.LightMadGodAll.Clone());

            return resistanceChangerEnemyAdv;
        }

        protected Enemy MakeResistanceChangerEnemy(string name, int hp, Elements resist1, Elements resist2)
        {
            var resistChangerEnemy = new ResistanceChangerEnemy
            {
                Name = $"[RCE] {name}",
                MaxHP = hp + _tierBoost,
                Image = CharacterImageAssets.GetImagePath(name),
                Resistances = new ResistanceArray(),
                Resist1 = resist1,
                Resist2 = resist2
            };

            resistChangerEnemy.Resistances.SetResistance(ResistanceType.Rs, resist1);
            resistChangerEnemy.Resistances.SetResistance(ResistanceType.Wk, resist2);

            return resistChangerEnemy;
        }

        protected Enemy MakeResistanceChangerEnemyAdvanced(string name, int hp, Elements resist1, Elements resist2)
        {
            var resistChangerEnemy = new ResistanceChangerEnemy
            {
                Name = $"[RCE] {name}",
                MaxHP = hp + _tierBoost,
                Image = CharacterImageAssets.GetImagePath(name),
                Resistances = new ResistanceArray(),
                Resist1 = resist1,
                Resist2 = resist2
            };

            resistChangerEnemy.Resistances.SetResistance(ResistanceType.Dr, resist1);
            resistChangerEnemy.Resistances.SetResistance(ResistanceType.Wk, resist2);

            return resistChangerEnemy;
        }
    }
}
