using AscendedZ.entities.enemy_objects.bosses;
using AscendedZ.entities.enemy_objects.enemy_ais;
using AscendedZ.entities.enemy_objects.special_bosses;
using AscendedZ.resistances;
using AscendedZ.skills;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AscendedZ.entities.enemy_objects.enemy_makers
{
    /// <summary>
    /// A factory for unique enemies with 1-off appearances.
    /// </summary>
    public class UniqueEnemyFactory : EnemyFactory
    {
        public UniqueEnemyFactory()
        {
            _functionDictionary[EnemyNames.Harbinger] = () => new Harbinger();
            _functionDictionary[EnemyNames.Elliot_Onyx] = () => new ElliotOnyx();
            _functionDictionary[EnemyNames.Sable_Vonner] = () => new SableVonner();
            _functionDictionary[EnemyNames.Cloven_Umbra] = () => new ClovenUmbra();
            _functionDictionary[EnemyNames.Ashen_Ash] = () => new AshenAsh();
            _functionDictionary[EnemyNames.Ethel_Aura] = MakeEthelAura;
            _functionDictionary[EnemyNames.Kellam_Von_Stein] = () => new KellamVonStein();
            _functionDictionary[EnemyNames.Drace_Skinner] = MakeDraceSkinner;
            _functionDictionary[EnemyNames.Jude_Stone] = MakeJudeStone;
            _functionDictionary[EnemyNames.Drace_Razor] = MakeDraceRazor;
            _functionDictionary[EnemyNames.Everit_Pickerin] = () => new EveritPickerin();
            _functionDictionary[EnemyNames.Alex_Church] = () => new AlexChurch();
            _functionDictionary[EnemyNames.Griffen_Hart] = () => new GriffenHart();
            _functionDictionary[EnemyNames.Bohumir_Cibulka] = () => new BohumirCibulka();
            _functionDictionary[EnemyNames.Zell_Grimsbane] = () => new ZellGrimsbane();
            _functionDictionary[EnemyNames.Soren_Winter] = () => new SorenWinter();
            _functionDictionary[EnemyNames.Requiem_Heliot] = () => new RequiemHeliot();
            _functionDictionary[EnemyNames.Sable_Craft] = () => new SableCraft();
            _functionDictionary[EnemyNames.Cinder_Morgan] = MakeTier190A;
            _functionDictionary[EnemyNames.Granger_Barlow] = MakeTier190B;
            _functionDictionary[EnemyNames.Not] = () => new Not();
            _functionDictionary[EnemyNames.Morden_Brack] = () => new MordenBrack();
            _functionDictionary[EnemyNames.Thorne_Lovelace] = () => new ThorneLovelace();
            _functionDictionary[EnemyNames.Law_Vossen] = () => new LawVossen();
            _functionDictionary[EnemyNames.Ocura] = MakeOcura;
            _functionDictionary[EnemyNames.Emush] = MakeEmush;
            _functionDictionary[EnemyNames.Hrothstyr_Zarmor] = MakeLogvat1;
            _functionDictionary[EnemyNames.Logvat] = MakeEmush;
            _functionDictionary[EnemyNames.Hrothstyr_Zarmor] = MakeLogvat1;
            _functionDictionary[EnemyNames.Logvat] = MakeLogvat2;
            _functionDictionary[EnemyNames.Theodstin_Glove] = MakeLogvat3;
            _functionDictionary[EnemyNames.Pleromyr] = MakePleromyr;
            _functionDictionary[EnemyNames.Arc_Hunt] = MakeArcHunt;
            _functionDictionary[EnemyNames.Buceala] = () => new Buceala();
            _functionDictionary[EnemyNames.Storm_Vossen] = MakeStormVossen;
            _functionDictionary[EnemyNames.Kodek] = MakeKodek;

            // special bosses
            _functionDictionary[EnemyNames.Nettala] = () => new Nettala();
            _functionDictionary[EnemyNames.Draco] = () => new Draco();
            _functionDictionary[EnemyNames.Drakalla] = () => new Drakalla();
            _functionDictionary[EnemyNames.Yacnacnalb] = () => new Yacnacnalb();
            _functionDictionary[EnemyNames.Mhaarvosh] = () => new Mhaarvosh();
            _functionDictionary[EnemyNames.Bhotldren] = () => new Bhotldren();
            _functionDictionary[EnemyNames.Aiucxaiobhlo] = () => new Aiucxaiobhlo();
            _functionDictionary[EnemyNames.Ghryztitralbh] = () => new Ghryztitralbh();

            // bounty bosses
            _functionDictionary[EnemyNames.Ancient_Nodys] = () => new AncientNodys();
            _functionDictionary[EnemyNames.Pakorag] = () => new Pakorag();
            _functionDictionary[EnemyNames.Iminth] = MakeIminth;
            _functionDictionary[EnemyNames.Sentinal] = MakeSentinal;
            _functionDictionary[EnemyNames.Floor_Architect] = MakeFloorArchitect;
        }

        public Enemy MakeEthelAura()
        {
            var ethel = MakeBossHellAI(EnemyNames.Ethel_Aura, 2);

            ethel.Resistances.SetResistance(ResistanceType.Wk, skills.Elements.Wind);
            ethel.Resistances.SetResistance(ResistanceType.Nu, skills.Elements.Elec);
            ethel.Resistances.SetResistance(ResistanceType.Nu, skills.Elements.Dark);

            ethel.Skills.Add(SkillDatabase.Elec1.Clone());
            ethel.Skills.Add(SkillDatabase.ElecAll.Clone());
            ethel.Skills.Add(SkillDatabase.Dark1.Clone());
            ethel.Skills.Add(SkillDatabase.VoidWind.Clone());

            return ethel;
        }

        public Enemy MakeDraceSkinner()
        {
            var draceSkinner = MakeBossHellAI(EnemyNames.Drace_Skinner, 3);

            draceSkinner.Resistances.SetResistance(ResistanceType.Wk, skills.Elements.Dark);
            draceSkinner.Resistances.SetResistance(ResistanceType.Nu, skills.Elements.Light);
            draceSkinner.Resistances.SetResistance(ResistanceType.Nu, skills.Elements.Fire);
            draceSkinner.Resistances.SetResistance(ResistanceType.Nu, skills.Elements.Wind);

            draceSkinner.Skills.Add(SkillDatabase.Light1.Clone());
            draceSkinner.Skills.Add(SkillDatabase.Elec1.Clone());
            draceSkinner.Skills.Add(SkillDatabase.Fire1.Clone());
            draceSkinner.Skills.Add(SkillDatabase.WindAll.Clone());
            draceSkinner.Skills.Add(SkillDatabase.Ice1.Clone());

            return draceSkinner;
        }

        public Enemy MakeJudeStone()
        {
            var judeStone = MakeBossHellAI(EnemyNames.Jude_Stone, 3);

            judeStone.Resistances.SetResistance(ResistanceType.Wk, skills.Elements.Light);
            judeStone.Resistances.SetResistance(ResistanceType.Wk, skills.Elements.Fire);
            judeStone.Resistances.SetResistance(ResistanceType.Nu, skills.Elements.Dark);
            judeStone.Resistances.SetResistance(ResistanceType.Nu, skills.Elements.Ice);

            judeStone.Skills.Add(SkillDatabase.IceAll.Clone());
            judeStone.Skills.Add(SkillDatabase.DarkAll.Clone());
            judeStone.Skills.Add(SkillDatabase.Dark1.Clone());
            judeStone.Skills.Add(SkillDatabase.VoidFire.Clone());
            judeStone.Skills.Add(SkillDatabase.VoidLight.Clone());
            judeStone.Skills.Add(SkillDatabase.WeakIce.Clone());
            judeStone.Skills.Add(SkillDatabase.Ice1.Clone());

            return judeStone;
        }

        public Enemy MakeArcHunt()
        {
            var boss = MakeBossHellAI(EnemyNames.Arc_Hunt, 3);

            boss.Resistances.SetResistance(ResistanceType.Wk, Elements.Dark);
            boss.Resistances.SetResistance(ResistanceType.Wk, Elements.Light);
            boss.Resistances.SetResistance(ResistanceType.Nu, Elements.Fire);
            boss.Resistances.SetResistance(ResistanceType.Nu, Elements.Ice);
            boss.Resistances.SetResistance(ResistanceType.Nu, Elements.Wind);
            boss.Resistances.SetResistance(ResistanceType.Nu, Elements.Elec);

            boss.Skills.Add(SkillDatabase.FireMadGod.Clone());
            boss.Skills.Add(SkillDatabase.IceMadGod.Clone());
            boss.Skills.Add(SkillDatabase.WindMadGod.Clone());
            boss.Skills.Add(SkillDatabase.ElecMadGod.Clone());

            return boss;
        }

        public Enemy MakeStormVossen()
        {
            var boss = MakeBossHellAI(EnemyNames.Storm_Vossen, 4);

            boss.Resistances.SetResistance(ResistanceType.Wk, Elements.Ice);
            boss.Resistances.SetResistance(ResistanceType.Dr, Elements.Fire);

            boss.Skills.Add(SkillDatabase.HolyGrail.Clone());
            boss.Skills.Add(SkillDatabase.FireMadGod.Clone());
            boss.Skills.Add(SkillDatabase.Almighty.Clone());
            boss.Skills.Add(SkillDatabase.Torpefy.Clone());
            boss.Skills.Add(SkillDatabase.PierceFire1.Clone());
            boss.Skills.Add(SkillDatabase.FireAllP.Clone());

            return boss;
        }

        public Enemy MakeOcura()
        {
            var ocura = MakeBossHellAI(EnemyNames.Ocura, 3, true);

            ocura.Resistances.SetResistance(ResistanceType.Nu, skills.Elements.Dark);
            ocura.Resistances.SetResistance(ResistanceType.Nu, skills.Elements.Light);

            ocura.Skills.Add(SkillDatabase.Elec1.Clone());
            ocura.Skills.Add(SkillDatabase.Fire1.Clone());
            ocura.Skills.Add(SkillDatabase.Ice1.Clone());
            ocura.Skills.Add(SkillDatabase.Wind1.Clone());
            ocura.Skills.Add(SkillDatabase.Dark1.Clone());
            ocura.Skills.Add(SkillDatabase.Light1.Clone());

            return ocura;
        }


        public Enemy MakeEmush()
        {
            var emush = MakeBossHellAI(EnemyNames.Emush, 4, true);

            emush.Resistances.SetResistance(ResistanceType.Wk, skills.Elements.Fire);
            emush.Resistances.SetResistance(ResistanceType.Dr, skills.Elements.Ice);
            emush.Resistances.SetResistance(ResistanceType.Dr, skills.Elements.Wind);
            emush.Skills.Add(SkillDatabase.Wind1.Clone());
            emush.Skills.Add(SkillDatabase.Ice1.Clone());
            emush.Skills.Add(SkillDatabase.Poison.Clone());
            emush.Skills.Add(SkillDatabase.IceAll.Clone());
            emush.Skills.Add(SkillDatabase.WindAll.Clone());
            
            return emush;
        }

        public Enemy MakeLogvat1()
        {
            var boss = MakeBossHellAI(EnemyNames.Hrothstyr_Zarmor, 2, true);

            boss.Resistances.SetResistance(ResistanceType.Wk, Elements.Light);
            boss.Resistances.SetResistance(ResistanceType.Dr, Elements.Ice);
            boss.Resistances.SetResistance(ResistanceType.Dr, Elements.Dark);

            boss.Skills.Add(SkillDatabase.OblivionsEmbrace.Clone());
            boss.Skills.Add(SkillDatabase.Ice1.Clone());
            boss.Skills.Add(SkillDatabase.Almighty1.Clone());

            return boss;
        }

        public Enemy MakeLogvat2()
        {
            var boss = MakeBossHellAI(EnemyNames.Logvat, 2, true);

            boss.Resistances.SetResistance(ResistanceType.Wk, Elements.Fire);
            boss.Resistances.SetResistance(ResistanceType.Dr, Elements.Ice);
            boss.Resistances.SetResistance(ResistanceType.Dr, Elements.Dark);

            boss.Skills.Add(SkillDatabase.Eliricpaul.Clone());
            boss.Skills.Add(SkillDatabase.Ice1.Clone());
            boss.Skills.Add(SkillDatabase.Almighty1.Clone());

            return boss;
        }

        public Enemy MakeLogvat3()
        {
            var boss = MakeBossHellAI(EnemyNames.Theodstin_Glove, 2, true);

            boss.Resistances.SetResistance(ResistanceType.Wk, Elements.Wind);
            boss.Resistances.SetResistance(ResistanceType.Dr, Elements.Ice);
            boss.Resistances.SetResistance(ResistanceType.Dr, Elements.Elec);

            boss.Skills.Add(SkillDatabase.Curse.Clone());
            boss.Skills.Add(SkillDatabase.Sarawaldbet.Clone());
            boss.Skills.Add(SkillDatabase.Antitichton.Clone());

            return boss;
        }

        public Enemy MakePleromyr()
        {
            var boss = MakeBossHellAI(EnemyNames.Pleromyr, 6, true);

            boss.Resistances.SetResistance(ResistanceType.Wk, Elements.Elec);
            boss.Resistances.SetResistance(ResistanceType.Nu, Elements.Fire);
            boss.Resistances.SetResistance(ResistanceType.Dr, Elements.Wind);

            boss.Skills.Add(SkillDatabase.ZephyrShield.Clone());
            boss.Skills.Add(SkillDatabase.Fredmuelald.Clone());
            boss.Skills.Add(SkillDatabase.LusterCandy.Clone());
            boss.Skills.Add(SkillDatabase.Almighty1.Clone());
            boss.Skills.Add(SkillDatabase.Fire1.Clone());
            boss.Skills.Add(SkillDatabase.Wind1.Clone());
            boss.Skills.Add(SkillDatabase.Debilitate.Clone());

            return boss;
        }

        public Enemy MakeKodek()
        {
            var boss = MakeBossHellAI(EnemyNames.Kodek, 3, true);

            boss.Resistances.SetResistance(ResistanceType.Nu, Elements.Fire);
            boss.Resistances.SetResistance(ResistanceType.Nu, Elements.Elec);
            boss.Resistances.SetResistance(ResistanceType.Dr, Elements.Wind);

            boss.Skills.Add(SkillDatabase.FireMadGod.Clone());
            boss.Skills.Add(SkillDatabase.PierceFire1.Clone());
            boss.Skills.Add(SkillDatabase.Wind1.Clone());
            boss.Skills.Add(SkillDatabase.Torpefy.Clone());
            boss.Skills.Add(SkillDatabase.PierceElec1.Clone());
            boss.Skills.Add(SkillDatabase.PierceWind1.Clone());
            boss.Skills.Add(SkillDatabase.HolyGrail.Clone());
            boss.Skills.Add(SkillDatabase.WindMadGod.Clone());

            return boss;
        }


        public Enemy MakeIminth()
        {
            var boss = MakeBossHellAI(EnemyNames.Iminth, 3, true);

            boss.Resistances.SetResistance(ResistanceType.Wk, Elements.Wind);
            boss.Resistances.SetResistance(ResistanceType.Nu, Elements.Elec);
            boss.Resistances.SetResistance(ResistanceType.Dr, Elements.Light);

            boss.Skills.Add(SkillDatabase.PierceLight1.Clone());
            boss.Skills.Add(SkillDatabase.LightAllP.Clone());
            boss.Skills.Add(SkillDatabase.Confusion.Clone());
            boss.Skills.Add(SkillDatabase.Antitichton.Clone());
            boss.Skills.Add(SkillDatabase.ElecMadGodAllP.Clone());
            boss.Skills.Add(SkillDatabase.HolyGrail.Clone());

            return boss;
        }

        public Enemy MakeSentinal()
        {
            var boss = MakeBossHellAI(EnemyNames.Sentinal, 3, true);

            boss.Resistances.SetResistance(ResistanceType.Wk, Elements.Light);
            boss.Resistances.SetResistance(ResistanceType.Dr, Elements.Fire);
            boss.Resistances.SetResistance(ResistanceType.Dr, Elements.Dark);

            boss.Skills.Add(SkillDatabase.AncientChoir.Clone());
            boss.Skills.Add(SkillDatabase.HolyGrail.Clone());
            boss.Skills.Add(SkillDatabase.Focus.Clone());
            boss.Skills.Add(SkillDatabase.Almighty1.Clone());
            boss.Skills.Add(SkillDatabase.DarkAllP.Clone());
            boss.Skills.Add(SkillDatabase.TechBuff.Clone());
            boss.Skills.Add(SkillDatabase.FireMadGod.Clone());
            boss.Skills.Add(SkillDatabase.DarkMadGod.Clone());
            boss.Skills.Add(SkillDatabase.Antitichton.Clone());

            return boss;
        }

        public Enemy MakeFloorArchitect()
        {
            var boss = MakeBossHellAI(EnemyNames.Floor_Architect, 3, true);

            boss.Resistances.SetResistance(ResistanceType.Dr, Elements.Wind);
            boss.Resistances.SetResistance(ResistanceType.Dr, Elements.Light);
            boss.Resistances.SetResistance(ResistanceType.Dr, Elements.Dark);

            boss.Skills.Add(SkillDatabase.MarkOfDeathSingle.Clone());
            boss.Skills.Add(SkillDatabase.AncientChoir.Clone());
            boss.Skills.Add(SkillDatabase.PierceWind1.Clone());

            boss.Skills.Add(SkillDatabase.Wind1.Clone());
            boss.Skills.Add(SkillDatabase.PierceLight1.Clone());
            boss.Skills.Add(SkillDatabase.LightAll.Clone());

            boss.Skills.Add(SkillDatabase.LightMadGod.Clone());
            boss.Skills.Add(SkillDatabase.DarkMadGod.Clone());
            boss.Skills.Add(SkillDatabase.WindMadGodAllP.Clone());

            return boss;
        }

        public Enemy MakeDraceRazor()
        {
            return new DraceRazor();
        }

        private BossHellAI MakeBossHellAI(string name, int turns, bool dCrawl = false)
        {
            var bhai = new BossHellAI()
            {
                Name = name,
                Image = CharacterImageAssets.GetImagePath(name),
                Resistances = new ResistanceArray(),
                Turns = turns
            };

            if (dCrawl)
                bhai.MaxHP = EntityDatabase.GetBossHPDC(name);
            else
                bhai.MaxHP = EntityDatabase.GetBossHP(name);

            bhai.Description = $"{bhai.Name}: Will iterate through each of its skills in the order they appear (from top to bottom) throughout the fight. " +
                $"Will always prioritize hitting player weaknesses if possible and avoiding their attacks getting nulled. " +
                $"If they have a Void Skill or an Eye Skill, they will only use it if their weakness is hit (once for the eye skill, 3 times for a void skill).";

            return bhai;
        }

        // EnemyNames.Cinder_Morgan, EnemyNames.Granger_Barlow
        public Enemy MakeTier190A()
        {
            string name = EnemyNames.Cinder_Morgan;
            var tier190Boss = new Tier190Boss() 
            {
                Name = name,
                Image = CharacterImageAssets.GetImagePath(name),
                MaxHP = EntityDatabase.GetBossHP(name),
                Turns = 2,
                TurnsForSkillSwap = 5
            };

            tier190Boss.Resistances.SetResistance(ResistanceType.Wk, Elements.Dark);
            tier190Boss.Resistances.SetResistance(ResistanceType.Dr, Elements.Light);
            tier190Boss.Resistances.SetResistance(ResistanceType.Dr, Elements.Wind);

            var arcFlash = SkillDatabase.ArcFlash;
            var almightySp = SkillDatabase.DeadlyWind;
            var heal = SkillDatabase.Heal1;

            var script = new List<ISkill>([almightySp, arcFlash]);

            tier190Boss.Script = script;
            tier190Boss.ReplacementSkill = heal;

            tier190Boss.Skills.AddRange(script);
            tier190Boss.Skills.Add(heal);

            tier190Boss.Description = $"{tier190Boss.Name}: Alternates between Light and Wind skills. Will cast a Heal spell every 5 turns";

            return tier190Boss;
        }

        public Enemy MakeTier190B()
        {
            string name = EnemyNames.Granger_Barlow;
            var tier190Boss = new Tier190Boss()
            {
                Name = name,
                Image = CharacterImageAssets.GetImagePath(name),
                MaxHP = EntityDatabase.GetBossHP(name),
                Turns = 2, 
                TurnsForSkillSwap = 2
            };

            tier190Boss.Resistances.SetResistance(ResistanceType.Wk, Elements.Light);
            tier190Boss.Resistances.SetResistance(ResistanceType.Dr, Elements.Dark);
            tier190Boss.Resistances.SetResistance(ResistanceType.Dr, Elements.Elec);

            var darkSp = SkillDatabase.OblivionsEmbrace;
            var almightySp = SkillDatabase.Eliricpaul;
            var buff = SkillDatabase.LusterCandy;

            var script = new List<ISkill>([almightySp, darkSp]);

            tier190Boss.Script = script;

            tier190Boss.Script = script;
            tier190Boss.ReplacementSkill = buff;

            tier190Boss.Skills.AddRange(script);
            tier190Boss.Description = $"{tier190Boss.Name}: Alternates between Dark and Elec skills. Will cast a buff spell every 2 turns";
            return tier190Boss;
        }
   
    }
}
