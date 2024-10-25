using AscendedZ.entities.enemy_objects.enemy_ais;
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
    public class AlternatingEnemyFactory: EnemyFactory
    {
        public AlternatingEnemyFactory()
        {
            _functionDictionary[EnemyNames.Conlen] = MakeConlen;
            _functionDictionary[EnemyNames.Orachar] = MakeOrachar;
            _functionDictionary[EnemyNames.Fastrobren] = MakeFastrobren;
            _functionDictionary[EnemyNames.Liamlas] = MakeLiamlas;
            _functionDictionary[EnemyNames.Fledan] = MakeFledan;
            _functionDictionary[EnemyNames.Walds] = MakeWalds;
            _functionDictionary[EnemyNames.CattuTDroni] = MakeCattuTDroni;
            _functionDictionary[EnemyNames.Aldmas] = MakeAldmas;
            _functionDictionary[EnemyNames.Fridan] = MakeFridan;
            _functionDictionary[EnemyNames.Paca] = MakePaca;
            _functionDictionary[EnemyNames.Wigfred] = MakeWigfred;
            _functionDictionary[EnemyNames.Lyley] = MakeLyley;
            _functionDictionary[EnemyNames.Acardeb] = MakeAcardeb;
            _functionDictionary[EnemyNames.Darol] = MakeDarol;
            _functionDictionary[EnemyNames.Hesbet] = MakeHesbet;
            _functionDictionary[EnemyNames.Khasterat] = MakeKhasterat;
            _functionDictionary[EnemyNames.Palmonu] = MakePalmonu;
            _functionDictionary[EnemyNames.Leos] = MakeLeos;
            _functionDictionary[EnemyNames.Camnonos] = MakeCamnonos;
            _functionDictionary[EnemyNames.Ridravos] = MakeRidravos;
            _functionDictionary[EnemyNames.Raos] = MakeRaos;
            _functionDictionary[EnemyNames.Kuo_toa] = MakeKuoToa;
            _functionDictionary[EnemyNames.Nolat] = MakeNolat;
            _functionDictionary[EnemyNames.Aboleth] = MakeAboleth;
            _functionDictionary[EnemyNames.Hollyshimmer] = MakeHollyshimmer;
            _functionDictionary[EnemyNames.Albedo] = MakeAlbedo;
            _functionDictionary[EnemyNames.Grizzleboink_the_Noodle_Snatcher] = MakeGrizzleboinkTheNoodleSnatcher;
            _functionDictionary[EnemyNames.Bue] = () => MakeBu(EnemyNames.Bue, 15, Elements.Ice);
            _functionDictionary[EnemyNames.Bued] = () => MakeBu(EnemyNames.Bued, 15, Elements.Fire);
            _functionDictionary[EnemyNames.Bureen] = () => MakeBu(EnemyNames.Bureen, 15, Elements.Wind);
            _functionDictionary[EnemyNames.Buight] = () => MakeBu(EnemyNames.Buight, 20, Elements.Light);
            _functionDictionary[EnemyNames.Builectric] = () => MakeBu(EnemyNames.Builectric, 20, Elements.Elec);
            _functionDictionary[EnemyNames.Burk] = () => MakeBu(EnemyNames.Burk, 20, Elements.Dark);

            // SPRT
            _functionDictionary[EnemyNames.Piazeor] = MakePiazor;
            _functionDictionary[EnemyNames.Kodose] = MakeKodose;
            _functionDictionary[EnemyNames.Omre] = MakeOmre;

            // normies
            _functionDictionary[EnemyNames.Uri] = MakeUri;
            _functionDictionary[EnemyNames.Zaalki] = MakeZaalki;
        }

        public Enemy MakeUri()
        {
            var enemy = MakeAlternatingEnemy(EnemyNames.Uri, 50);

            enemy.Resistances.SetResistance(ResistanceType.Wk, Elements.Wind);
            enemy.Resistances.SetResistance(ResistanceType.Wk, Elements.Ice);

            enemy.Skills.Add(SkillDatabase.ElecAllP.Clone());
            enemy.Skills.Add(SkillDatabase.FireAllP.Clone());

            return enemy;
        }

        public Enemy MakeZaalki()
        {
            var enemy = MakeAlternatingEnemy(EnemyNames.Zaalki, 50);

            enemy.Resistances.SetResistance(ResistanceType.Wk, Elements.Elec);
            enemy.Resistances.SetResistance(ResistanceType.Wk, Elements.Fire);

            enemy.Skills.Add(SkillDatabase.WindAllP.Clone());
            enemy.Skills.Add(SkillDatabase.IceAllP.Clone());

            return enemy;
        }

        public Enemy MakePiazor()
        {
            var enemy = MakeSupportEnemy(EnemyNames.Piazeor, 30);

            enemy.Resistances.SetResistance(ResistanceType.Wk, Elements.Fire);
            enemy.Resistances.SetResistance(ResistanceType.Dr, Elements.Ice);

            enemy.Skills.Add(SkillDatabase.IceMadGodAllP.Clone());

            return enemy;
        }

        public Enemy MakeKodose()
        {
            var enemy = MakeSupportEnemy(EnemyNames.Kodose, 30);

            enemy.Resistances.SetResistance(ResistanceType.Wk, Elements.Ice);
            enemy.Resistances.SetResistance(ResistanceType.Dr, Elements.Fire);

            enemy.Skills.Add(SkillDatabase.FireMadGodAllP.Clone());

            return enemy;
        }

        public Enemy MakeOmre()
        {
            var enemy = MakeSupportEnemy(EnemyNames.Omre, 30);

            enemy.Resistances.SetResistance(ResistanceType.Wk, Elements.Light);
            enemy.Resistances.SetResistance(ResistanceType.Dr, Elements.Dark);

            enemy.Skills.Add(SkillDatabase.DarkMadGodAllP.Clone());

            return enemy;
        }

        public Enemy MakeConlen()
        {
            var conlen = MakeAlternatingEnemy(EnemyNames.Conlen, 6 + _tierBoost);

            conlen.Resistances.SetResistance(ResistanceType.Wk, Elements.Wind);
            conlen.Resistances.SetResistance(ResistanceType.Wk, Elements.Ice);

            conlen.Skills.Add(SkillDatabase.Elec1.Clone());
            conlen.Skills.Add(SkillDatabase.Fire1.Clone());

            return conlen;
        }

        public Enemy MakeCattuTDroni()
        {
            var droni = MakeAlternatingEnemy(EnemyNames.CattuTDroni, 8);

            droni.Resistances.SetResistance(ResistanceType.Wk, Elements.Wind);
            droni.Resistances.SetResistance(ResistanceType.Rs, Elements.Elec);

            droni.Skills.Add(SkillDatabase.Elec1.Clone());
            droni.Skills.Add(SkillDatabase.Ice1.Clone());

            return droni;
        }

        public Enemy MakeOrachar()
        {
            var orachar = MakeAlternatingEnemy(EnemyNames.Orachar, 6);

            orachar.Resistances.SetResistance(ResistanceType.Wk, Elements.Fire);

            orachar.Skills.Add(SkillDatabase.Ice1.Clone());

            return orachar;
        }

        public Enemy MakeFastrobren()
        {
            var fastrobren = MakeAlternatingEnemy(EnemyNames.Fastrobren, 4);

            fastrobren.Resistances.SetResistance(ResistanceType.Wk, Elements.Light);

            fastrobren.Skills.Add(SkillDatabase.Dark1.Clone());

            return fastrobren;
        }

        public Enemy MakeLiamlas()
        {
            var liamlas = MakeAlternatingEnemy(EnemyNames.Liamlas, 6);
            
            liamlas.Resistances.SetResistance(ResistanceType.Wk, Elements.Dark);

            liamlas.Skills.Add(SkillDatabase.Light1.Clone());

            return liamlas;
        }

        public Enemy MakeFledan()
        {
            var fledan = MakeAlternatingEnemy(EnemyNames.Fledan, 10);

            fledan.Resistances.SetResistance(ResistanceType.Rs, Elements.Light);
            fledan.Resistances.SetResistance(ResistanceType.Rs, Elements.Ice);
            fledan.Resistances.SetResistance(ResistanceType.Wk, Elements.Dark);

            fledan.Skills.Add(SkillDatabase.Light1.Clone());
            fledan.Skills.Add(SkillDatabase.Ice1.Clone());

            return fledan;
        }

        public Enemy MakeWalds()
        {
            var walds = MakeAlternatingEnemy(EnemyNames.Walds, 10);

            walds.Resistances.SetResistance(ResistanceType.Rs, Elements.Dark);
            walds.Resistances.SetResistance(ResistanceType.Rs, Elements.Wind);
            walds.Resistances.SetResistance(ResistanceType.Wk, Elements.Light);

            walds.Skills.Add(SkillDatabase.Dark1.Clone());
            walds.Skills.Add(SkillDatabase.Wind1.Clone());

            return walds;
        }

        public Enemy MakeAldmas()
        {
            var aldmas = MakeAlternatingEnemy(EnemyNames.Aldmas, 7);

            aldmas.Resistances.SetResistance(ResistanceType.Wk, Elements.Ice);

            aldmas.Skills.Add(SkillDatabase.FireAll.Clone());

            return aldmas;
        }

        public Enemy MakeFridan()
        {
            var fridan = MakeAlternatingEnemy(EnemyNames.Fridan, 8);

            fridan.Resistances.SetResistance(ResistanceType.Wk, Elements.Elec);

            fridan.Skills.Add(SkillDatabase.WindAll.Clone());

            return fridan;
        }

        public Enemy MakeLeos()
        {
            var leos = MakeAlternatingEnemy(EnemyNames.Leos, 20);

            leos.Resistances.SetResistance(ResistanceType.Wk, Elements.Ice);
            leos.Resistances.SetResistance(ResistanceType.Rs, Elements.Fire);
            leos.Resistances.SetResistance(ResistanceType.Dr, Elements.Wind);

            leos.Skills.AddRange([SkillDatabase.Fire1.Clone(), SkillDatabase.Wind1.Clone()]);

            return leos;
        }

        public Enemy MakeCamnonos()
        {
            var camnonos = MakeAlternatingEnemy(EnemyNames.Camnonos, 20);

            camnonos.Resistances.SetResistance(ResistanceType.Wk, Elements.Wind);
            camnonos.Resistances.SetResistance(ResistanceType.Rs, Elements.Ice);
            camnonos.Resistances.SetResistance(ResistanceType.Dr, Elements.Elec);

            camnonos.Skills.AddRange([SkillDatabase.Ice1.Clone(), SkillDatabase.ElecAll.Clone()]);

            return camnonos;
        }

        public Enemy MakeRidravos()
        {
            var ridravos = MakeAlternatingEnemy(EnemyNames.Ridravos, 20);

            ridravos.Resistances.SetResistance(ResistanceType.Wk, Elements.Elec);
            ridravos.Resistances.SetResistance(ResistanceType.Rs, Elements.Wind);
            ridravos.Resistances.SetResistance(ResistanceType.Dr, Elements.Fire);

            ridravos.Skills.AddRange([SkillDatabase.Wind1.Clone(), SkillDatabase.FireAll.Clone()]);

            return ridravos;
        }

        public Enemy MakeRaos()
        {
            var raos = MakeAlternatingEnemy(EnemyNames.Raos, 20);

            raos.Resistances.SetResistance(ResistanceType.Wk, Elements.Fire);
            raos.Resistances.SetResistance(ResistanceType.Rs, Elements.Elec);
            raos.Resistances.SetResistance(ResistanceType.Dr, Elements.Ice);

            raos.Skills.AddRange([SkillDatabase.Elec1.Clone(), SkillDatabase.IceAll.Clone()]);

            return raos;
        }

        public Enemy MakePaca() 
        { 
            var paca = MakeSupportEnemy(EnemyNames.Paca, 9);

            paca.Resistances.SetResistance(ResistanceType.Dr, Elements.Dark);

            paca.Skills.Add(SkillDatabase.Dark1.Clone());
            paca.Skills.Add(SkillDatabase.Light1.Clone());
            paca.Skills.Add(SkillDatabase.Wind1.Clone());

            return paca;
        }

        public Enemy MakeWigfred() 
        { 
            var wigfred = MakeSupportEnemy(EnemyNames.Wigfred, 9);

            wigfred.Resistances.SetResistance(ResistanceType.Dr, Elements.Elec);
            wigfred.Resistances.SetResistance(ResistanceType.Wk, Elements.Wind);

            wigfred.Skills.Add(SkillDatabase.Elec1.Clone());
            wigfred.Skills.Add(SkillDatabase.Ice1.Clone());
            wigfred.Skills.Add(SkillDatabase.ElecAll.Clone());

            return wigfred;
        }

        public Enemy MakeLyley() 
        { 
            var lyley = MakeSupportEnemy(EnemyNames.Lyley, 9);
            
            lyley.Resistances.SetResistance(ResistanceType.Dr, Elements.Fire);

            lyley.Skills.Add(SkillDatabase.Fire1.Clone());
            lyley.Skills.Add(SkillDatabase.Wind1.Clone());
            lyley.Skills.Add(SkillDatabase.FireAll.Clone());

            return lyley;
        }

        public Enemy MakeAcardeb()
        {
            var acardeb = MakeEyeEnemy(EnemyNames.Acardeb, 15, SkillDatabase.BeastEye);

            acardeb.Resistances.SetResistance(ResistanceType.Wk, Elements.Ice);

            acardeb.Skills.Add(SkillDatabase.Fire1.Clone());

            return acardeb;
        }

        public Enemy MakeDarol()
        {
            var darol = MakeEyeEnemy(EnemyNames.Darol, 15, SkillDatabase.BeastEye);

            darol.Resistances.SetResistance(ResistanceType.Wk, Elements.Light);

            darol.Skills.Add(SkillDatabase.Dark1.Clone());

            return darol;
        }

        public Enemy MakeKhasterat()
        {
            var khasterat = MakeEyeEnemy(EnemyNames.Khasterat, 15, SkillDatabase.DragonEye);

            khasterat.Resistances.SetResistance(ResistanceType.Wk, Elements.Ice);

            khasterat.Skills.Add(SkillDatabase.Fire1.Clone());

            return khasterat;
        }

        public Enemy MakePalmonu()
        {
            var palmonu = MakeEyeEnemy(EnemyNames.Palmonu, 15, SkillDatabase.DragonEye);

            palmonu.Resistances.SetResistance(ResistanceType.Wk, Elements.Dark);

            palmonu.Skills.Add(SkillDatabase.Light1.Clone());

            return palmonu;
        }

        public Enemy MakeHesbet()
        {
            var darol = MakeEyeEnemy(EnemyNames.Hesbet, 15, SkillDatabase.BeastEye);

            darol.Resistances.SetResistance(ResistanceType.Wk, Elements.Wind);

            darol.Skills.Add(SkillDatabase.Elec1.Clone());

            return darol;
        }

        public Enemy MakeHollyshimmer()
        {
            var hollyshimmer = MakeSupportEnemy(EnemyNames.Hollyshimmer, 25);

            hollyshimmer.Resistances.SetResistance(ResistanceType.Wk, Elements.Light);
            hollyshimmer.Resistances.SetResistance(ResistanceType.Nu, Elements.Dark);
            hollyshimmer.Resistances.SetResistance(ResistanceType.Nu, Elements.Elec);

            hollyshimmer.Skills.Add(SkillDatabase.OblivionsEmbrace.Clone());
            hollyshimmer.Skills.Add(SkillDatabase.Curse.Clone());

            return hollyshimmer;
        }

        public Enemy MakeAlbedo()
        {
            var albedo = MakeAlternatingEnemy(EnemyNames.Albedo, 25);

            albedo.Resistances.SetResistance(ResistanceType.Wk, Elements.Dark);
            albedo.Resistances.SetResistance(ResistanceType.Nu, Elements.Light);

            albedo.Skills.Add(SkillDatabase.Antitichton.Clone());
            albedo.Skills.Add(SkillDatabase.ArcFlash.Clone());

            return albedo;
        }

        public Enemy MakeGrizzleboinkTheNoodleSnatcher()
        {
            var morgathar = MakeAlternatingEnemy(EnemyNames.Grizzleboink_the_Noodle_Snatcher, 25);

            morgathar.Resistances.SetResistance(ResistanceType.Wk, Elements.Wind);
            morgathar.Resistances.SetResistance(ResistanceType.Nu, Elements.Elec);
            morgathar.Resistances.SetResistance(ResistanceType.Nu, Elements.Fire);

            morgathar.Skills.Add(SkillDatabase.Hellfire.Clone());
            morgathar.Skills.Add(SkillDatabase.Eliricpaul.Clone());

            return morgathar;
        }

        public Enemy MakeAboleth()
        {
            var aboleth = MakeSupportEnemy(EnemyNames.Aboleth, 25);

            aboleth.Resistances.SetResistance(ResistanceType.Wk, Elements.Fire);
            aboleth.Resistances.SetResistance(ResistanceType.Nu, Elements.Ice);
            aboleth.Resistances.SetResistance(ResistanceType.Nu, Elements.Wind);

            aboleth.Skills.Add(SkillDatabase.Sarawaldbet.Clone());

            return aboleth;
        }

        public Enemy MakeKuoToa()
        {
            var kuoToa = MakeEyeEnemy(EnemyNames.Kuo_toa, 25, SkillDatabase.DragonEye);

            kuoToa.Resistances.SetResistance(ResistanceType.Wk, Elements.Ice);

            kuoToa.Skills.Add(SkillDatabase.Fredmuelald.Clone());

            return kuoToa;
        }

        public Enemy MakeNolat()
        {
            var nolat = MakeEyeEnemy(EnemyNames.Nolat, 25, SkillDatabase.DragonEye);

            nolat.Resistances.SetResistance(ResistanceType.Wk, Elements.Elec);
            nolat.Resistances.SetResistance(ResistanceType.Dr, Elements.Wind);

            nolat.Skills.Add(SkillDatabase.ZephyrShield.Clone());

            return nolat;
        }


        public Enemy MakeBu(string name, int hp, Elements element)
        {
            var bu = MakeAlternatingEnemy(name, hp);
            bu.Resistances.SetResistance(ResistanceType.Rs, element);
            bu.Skills.Add(SkillDatabase.GetSkillFromElement(element));
            bu.Turns = 2;
            return bu;
        }

        protected Enemy MakeEyeEnemy(string name, int hp, EyeSkill eyeSkill)
        {
            return new EyeEnemy
            {
                Name = $"[EYE] {name}",
                MaxHP = hp + _tierBoost,
                Image = CharacterImageAssets.GetImagePath(name),
                Resistances = new ResistanceArray(),
                EyeSkill = eyeSkill
            };
        }

        protected Enemy MakeSupportEnemy(string name, int hp)
        {
            var support = new SupportEnemy
            {
                Name = $"[SPRT] {name}",
                MaxHP = hp + _tierBoost,
                Image = CharacterImageAssets.GetImagePath(name),
                Resistances = new ResistanceArray()
            };

            support.LevelUpCompensation(((_tierBoost/3) * 10)/2);

            return support;
        }

        protected Enemy MakeAlternatingEnemy(string name, int hp)
        {
            return new AlternatingEnemy
            {
                Name = $"[AE] {name}",
                MaxHP = hp + _tierBoost,
                Image = CharacterImageAssets.GetImagePath(name),
                Resistances = new ResistanceArray()
            };
        }
    }
}
