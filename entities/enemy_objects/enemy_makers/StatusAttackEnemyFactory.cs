using AscendedZ.entities.enemy_objects.enemy_ais;
using AscendedZ.resistances;
using AscendedZ.skills;
using AscendedZ.statuses;
using AscendedZ.statuses.weak_element;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.entities.enemy_objects.enemy_makers
{
    public class StatusAttackEnemyFactory : EnemyFactory
    {
        public StatusAttackEnemyFactory()
        {
            _functionDictionary[EnemyNames.Thylaf] = MakeThylaf;
            _functionDictionary[EnemyNames.Arwig] = MakeArwig;
            _functionDictionary[EnemyNames.Riccman] = MakeRiccman;
            _functionDictionary[EnemyNames.Gormacwen] = MakeGormacwen;
            _functionDictionary[EnemyNames.Vidwerd] = MakeVidwerd;
            _functionDictionary[EnemyNames.Rahfortin] = MakeRahfortin;
            _functionDictionary[EnemyNames.Leswith] = MakeLeswith;
            _functionDictionary[EnemyNames.Isumforth] = MakeIsumforth;
            _functionDictionary[EnemyNames.Ingesc] = MakeIngesc;
            _functionDictionary[EnemyNames.Zalth] = MakeZalth;
            _functionDictionary[EnemyNames.Bernasbeorth] = MakeBernasbeorth;
            _functionDictionary[EnemyNames.Iaviol] = MakeIaviol;
            _functionDictionary[EnemyNames.Olu] = MakeOlu;
            _functionDictionary[EnemyNames.Yodigrin] = MakeYodigirin;
            _functionDictionary[EnemyNames.Vustuma] = MakeVustuma;
            _functionDictionary[EnemyNames.Tilaza_Fado] = MakeTilazaFado;
            _functionDictionary[EnemyNames.Iji] = MakeIji;
            _functionDictionary[EnemyNames.Sezuzo] = MakeSezuzo;
            _functionDictionary[EnemyNames.Dahone_Zude] = MakeDahoneZude;
            _functionDictionary[EnemyNames.Enu] = MakeEnu;
            _functionDictionary[EnemyNames.Juye] = MakeJuye;
            _functionDictionary[EnemyNames.Tuidonak] = MakeTuidonak;
            _functionDictionary[EnemyNames.Soredr] = MakeSoredr;
        }

        public Enemy MakeThylaf()
        {
            var thylaf = MakeAgroStatusEnemy(EnemyNames.Thylaf, 6);

            thylaf.Resistances.SetResistance(ResistanceType.Wk, Elements.Wind);

            thylaf.Skills.Add(SkillDatabase.Elec1.Clone());

            return thylaf;
        }

        public Enemy MakeArwig()
        {
            var arwig = MakeAgroStatusEnemy(EnemyNames.Arwig, 6);

            arwig.Resistances.SetResistance(ResistanceType.Wk, Elements.Fire);

            arwig.Skills.Add(SkillDatabase.Ice1.Clone());

            return arwig;
        }

        public Enemy MakeRiccman()
        {
            var riccman = MakeAgroStatusEnemy(EnemyNames.Riccman, 6);

            riccman.Resistances.SetResistance(ResistanceType.Wk, Elements.Elec);

            riccman.Skills.Add(SkillDatabase.Wind1.Clone());

            return riccman;
        }

        public Enemy MakeGormacwen()
        {
            var gormacwen = MakeAgroStatusEnemy(EnemyNames.Gormacwen, 10);

            gormacwen.Resistances.SetResistance(ResistanceType.Rs, Elements.Fire);

            gormacwen.Skills.Add(SkillDatabase.FireAll.Clone());

            return gormacwen;
        }

        public Enemy MakeVidwerd()
        {
            var vidwerd = MakeAgroStatusEnemy(EnemyNames.Vidwerd, 10);

            vidwerd.Resistances.SetResistance(ResistanceType.Rs, Elements.Dark);

            vidwerd.Skills.Add(SkillDatabase.DarkAll.Clone());

            return vidwerd;
        }

        public Enemy MakeRahfortin()
        {
            var rahfortin = MakeStatusAttackEnemy(EnemyNames.Rahfortin, 10);

            rahfortin.Name = $"[WES] {rahfortin.Name}";
            rahfortin.Status = new WeakDarkStatus();
            rahfortin.Resistances.SetResistance(ResistanceType.Wk, Elements.Light);
            rahfortin.Description = $"[WES] - Weakness Status Enemy: {rahfortin.Description}";

            rahfortin.Skills.Add(SkillDatabase.WeakAllDark.Clone());
            rahfortin.Skills.Add(SkillDatabase.DarkAll.Clone());

            return rahfortin;
        }

        public Enemy MakeLeswith()
        {
            var leswith = MakeStatusAttackEnemy(EnemyNames.Leswith, 10);
            
            leswith.Name = $"[WES] {leswith.Name}";
            leswith.Status = new WeakElecStatus();
            leswith.Resistances.SetResistance(ResistanceType.Wk, Elements.Wind);
            leswith.Description = $"[WES] - Weakness Exploit Status Enemy: {leswith.Description}";

            leswith.Skills.Add(SkillDatabase.WeakAllElec.Clone());
            leswith.Skills.Add(SkillDatabase.ElecAll.Clone());

            return leswith;
        }

        public Enemy MakeIsumforth()
        {
            var isumforth = MakeStunStatusEnemy(EnemyNames.Isumforth, 10);

            isumforth.Resistances.SetResistance(ResistanceType.Nu, Elements.Dark);
            isumforth.Resistances.SetResistance(ResistanceType.Rs, Elements.Fire);
            isumforth.Resistances.SetResistance(ResistanceType.Wk, Elements.Elec);

            isumforth.Skills.Add(SkillDatabase.Dark1.Clone());
            isumforth.Skills.Add(SkillDatabase.Fire1.Clone());

            return isumforth;
        }

        public Enemy MakeIngesc()
        {
            var ingesc = MakeStunStatusEnemy(EnemyNames.Ingesc, 12);

            ingesc.Resistances.SetResistance(ResistanceType.Nu, Elements.Elec);
            ingesc.Resistances.SetResistance(ResistanceType.Nu, Elements.Fire);

            ingesc.Skills.Add(SkillDatabase.FireAll.Clone());
            ingesc.Skills.Add(SkillDatabase.ElecAll.Clone());

            return ingesc;
        }

        public Enemy MakeZalth()
        {
            var zalth = MakeStunStatusEnemy(EnemyNames.Zalth, 12);

            zalth.Resistances.SetResistance(ResistanceType.Rs, Elements.Light);
            zalth.Resistances.SetResistance(ResistanceType.Dr, Elements.Ice);

            zalth.Skills.Add(SkillDatabase.Light1.Clone());
            zalth.Skills.Add(SkillDatabase.Ice1.Clone());

            return zalth;
        }

        public Enemy MakeBernasbeorth() 
        { 
            var bernasbeorth = MakePoisonEnemy(EnemyNames.Bernasbeorth, 14);

            bernasbeorth.Resistances.SetResistance(ResistanceType.Rs, Elements.Fire);
            bernasbeorth.Skills.Add(SkillDatabase.Light1.Clone());

            return bernasbeorth;
        }

        public Enemy MakeIaviol() 
        { 
            var iaviol = MakePoisonEnemy(EnemyNames.Iaviol, 14);

            iaviol.Resistances.SetResistance(ResistanceType.Nu, Elements.Ice);
            iaviol.Skills.Add(SkillDatabase.Ice1.Clone());

            return iaviol;
        }

        public Enemy MakeOlu() 
        { 
            var olu = MakePoisonEnemy(EnemyNames.Olu, 14);

            olu.Resistances.SetResistance(ResistanceType.Nu, Elements.Elec);
            olu.Skills.Add(SkillDatabase.Elec1.Clone());

            return olu;
        }

        public Enemy MakeYodigirin()
        {
            var yodigirin = MakePoisonAllEnemy(EnemyNames.Yodigrin, 20);

            yodigirin.Resistances.SetResistance(ResistanceType.Dr, Elements.Fire);

            yodigirin.Skills.Add(SkillDatabase.Fire1.Clone());

            return yodigirin;
        }

        public Enemy MakeVustuma()
        {
            var vustuma = MakePoisonAllEnemy(EnemyNames.Vustuma, 20);

            vustuma.Resistances.SetResistance(ResistanceType.Dr, Elements.Wind);

            vustuma.Skills.Add(SkillDatabase.Wind1.Clone());

            return vustuma;
        }

        #region Bind Enemies
        public Enemy MakeIji()
        {
            var iji = MakeBindEnemy(EnemyNames.Iji, 25, false);

            iji.Resistances.SetResistance(ResistanceType.Dr, Elements.Dark);
            iji.Resistances.SetResistance(ResistanceType.Wk, Elements.Light);

            iji.Skills.Add(SkillDatabase.Almighty.Clone());

            return iji;
        }

        public Enemy MakeSezuzo()
        {
            var sezuzo = MakeBindEnemy(EnemyNames.Sezuzo, 25, false);

            sezuzo.Resistances.SetResistance(ResistanceType.Dr, Elements.Fire);
            sezuzo.Resistances.SetResistance(ResistanceType.Wk, Elements.Ice);

            sezuzo.Skills.Add(SkillDatabase.FireAll.Clone());

            return sezuzo;
        }

        public Enemy MakeTilazaFado()
        {
            var tilazaFado = MakeBindEnemy(EnemyNames.Tilaza_Fado, 25, false);

            tilazaFado.Resistances.SetResistance(ResistanceType.Dr, Elements.Elec);
            tilazaFado.Resistances.SetResistance(ResistanceType.Wk, Elements.Wind);

            tilazaFado.Skills.Add(SkillDatabase.ElecAll.Clone());

            return tilazaFado;
        }

        public Enemy MakeTuidonak()
        {
            var tilazaFado = MakeBindEnemy(EnemyNames.Tuidonak, 25, true);

            tilazaFado.Resistances.SetResistance(ResistanceType.Dr, Elements.Ice);
            tilazaFado.Resistances.SetResistance(ResistanceType.Wk, Elements.Fire);

            tilazaFado.Skills.Add(SkillDatabase.Ice1.Clone());

            return tilazaFado;
        }
        #endregion

        #region Seal Enemies
        public Enemy MakeEnu()
        {
            var enu = MakeSealEnemy(EnemyNames.Enu, 24, false);

            enu.Resistances.SetResistance(ResistanceType.Dr, Elements.Fire);
            enu.Resistances.SetResistance(ResistanceType.Dr, Elements.Wind);
            enu.Resistances.SetResistance(ResistanceType.Wk, Elements.Ice);

            enu.Skills.Add(SkillDatabase.Fire1.Clone());

            return enu;
        }

        public Enemy MakeJuye()
        {
            var juye = MakeSealEnemy(EnemyNames.Juye, 24, false);

            juye.Resistances.SetResistance(ResistanceType.Dr, Elements.Ice);
            juye.Resistances.SetResistance(ResistanceType.Dr, Elements.Elec);
            juye.Resistances.SetResistance(ResistanceType.Wk, Elements.Fire);

            juye.Skills.Add(SkillDatabase.Ice1.Clone());

            return juye;
        }

        public Enemy MakeDahoneZude()
        {
            var dahoneZude = MakeSealEnemy(EnemyNames.Dahone_Zude, 24, false);

            dahoneZude.Resistances.SetResistance(ResistanceType.Dr, Elements.Elec);
            dahoneZude.Resistances.SetResistance(ResistanceType.Dr, Elements.Dark);
            dahoneZude.Resistances.SetResistance(ResistanceType.Wk, Elements.Wind);

            dahoneZude.Skills.Add(SkillDatabase.Elec1.Clone());

            return dahoneZude;
        }

        public Enemy MakeSoredr()
        {
            var dahoneZude = MakeSealEnemy(EnemyNames.Soredr, 24, true);

            dahoneZude.Resistances.SetResistance(ResistanceType.Dr, Elements.Light);
            dahoneZude.Resistances.SetResistance(ResistanceType.Wk, Elements.Dark);

            dahoneZude.Skills.Add(SkillDatabase.Dark1.Clone());

            return dahoneZude;
        }
        #endregion

        private Enemy MakeAgroStatusEnemy(string name, int hp)
        {
            var statusAttackEnemy = MakeStatusAttackEnemy(name, hp);

            statusAttackEnemy.Name = $"[AGRO] {statusAttackEnemy.Name}";
            statusAttackEnemy.Status = new AgroStatus();
            statusAttackEnemy.Skills.Add(SkillDatabase.AgroEnemy.Clone());
            statusAttackEnemy.Description = $"[AGRO] - Agro Enemy: {statusAttackEnemy.Description}";

            return statusAttackEnemy;
        }

        private Enemy MakeStunStatusEnemy(string name, int hp)
        {
            var statusAttackEnemy = MakeStatusAttackEnemy(name, hp);

            statusAttackEnemy.Name = $"[STN] {statusAttackEnemy.Name}";
            statusAttackEnemy.Status = new StunStatus();
            statusAttackEnemy.Skills.Add(SkillDatabase.Stun.Clone());
            statusAttackEnemy.Description = $"[STN] - Status Enemy: Alternates between stunning and attacking. Will stop applying stun if agro is active or only 1 player doesn't have stun applied.";

            return statusAttackEnemy;
        }

        private Enemy MakePoisonEnemy(string name, int hp)
        {
            var statusAttackEnemy = MakeStatusAttackEnemy(name, hp);

            statusAttackEnemy.Name = $"[PSN] {statusAttackEnemy.Name}";
            statusAttackEnemy.Status = new PoisonStatus();
            statusAttackEnemy.Skills.Add(SkillDatabase.Poison.Clone());
            statusAttackEnemy.Description = $"[PSN] - Poison Enemy: {statusAttackEnemy.Description}";

            return statusAttackEnemy;
        }

        private Enemy MakeBindEnemy(string name, int hp, bool all)
        {
            var statusAttackEnemy = MakeStatusAttackEnemy(name, hp);

            statusAttackEnemy.Name = $"[BIND] {statusAttackEnemy.Name}";
            statusAttackEnemy.Status = new BindStatus();
            if(all)
                statusAttackEnemy.Skills.Add(SkillDatabase.BindAll.Clone());
            else
                statusAttackEnemy.Skills.Add(SkillDatabase.Bind.Clone());
            statusAttackEnemy.Description = $"[BIND] - Bind Enemy: {statusAttackEnemy.Description}";

            return statusAttackEnemy;
        }

        private Enemy MakeSealEnemy(string name, int hp, bool all)
        {
            var statusAttackEnemy = MakeStatusAttackEnemy(name, hp);

            statusAttackEnemy.Name = $"[SEAL] {statusAttackEnemy.Name}";
            statusAttackEnemy.Status = new SealStatus();
            
            if (all)
                statusAttackEnemy.Skills.Add(SkillDatabase.SealAll.Clone());
            else
                statusAttackEnemy.Skills.Add(SkillDatabase.Seal.Clone());

            statusAttackEnemy.Description = $"[SEAL] - Seal Enemy: {statusAttackEnemy.Description}";

            return statusAttackEnemy;
        }

        private Enemy MakePoisonAllEnemy(string name, int hp)
        {
            var statusAttackEnemy = MakePoisonEnemy(name, hp);
            statusAttackEnemy.Skills.Clear();
            statusAttackEnemy.Skills.Add(SkillDatabase.PoisonAll.Clone());
            return statusAttackEnemy;
        }

        private StatusAttackEnemy MakeStatusAttackEnemy(string name, int hp)
        {
            return new StatusAttackEnemy 
            {
                Name = name,
                MaxHP = hp + _tierBoost,
                Image = CharacterImageAssets.GetImagePath(name),
                Resistances = new ResistanceArray()
            };
        }
    }
}
