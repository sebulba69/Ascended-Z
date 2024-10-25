using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.entities
{
    public static class PartyNames
    {
        // Pre-made
        public static readonly string Locphiedon = "Locphiedon";
        public static readonly string Gagar = "Gagar";
        public static readonly string Yuudam = "Yuudam";
        public static readonly string Pecheal = "Pecheal";
        public static readonly string Toke = "Toke";
        public static readonly string Maxwald = "Maxwald";
        public static readonly string Halvia = "Halvia";
        public static readonly string Tyhere = "Tyhere";
        public static readonly string Paria = "Paria";
        public static readonly string Joan = "Joan";
        public static readonly string Andmond = "Andmond";

        // Minions
        public static readonly string BuceyRed = "Bucey Red";
        public static readonly string BuceyBlue = "Bucey Blue";
        public static readonly string BuceyGreen = "Bucey Green";

        // Fusion 1
        public static readonly string Ancrow = "Ancrow"; // Fire
        public static readonly string Marchris = "Marchris"; // Fire
        public static readonly string Candun = "Candun"; // Ice
        public static readonly string Thryth = "Thryth"; // Ice
        public static readonly string Samlin = "Samlin"; // Wind
        public static readonly string Everever = "Everever"; // Wind
        public static readonly string Ciavid = "Ciavid"; // Elec
        public static readonly string Eri = "Eri"; // Elec
        public static readonly string Conson = "Conson"; // Light
        public static readonly string Winegeful = "Winegiful"; // Light
        public static readonly string Cermas = "Cermas"; // Dark
        public static readonly string Fledron = "Fledron"; // Dark

        public static readonly string Ride = "Ride"; // Fire
        public static readonly string Muelwise = "Muel-wise"; // Fire
        public static readonly string Pher = "Pher"; // Fire

        public static readonly string Shacy = "Shacy"; // Ice
        public static readonly string Swithwil = "Swithwil"; // Ice
        public static readonly string Isenann = "Isenann"; // Ice

        public static readonly string Lesdan = "Lesdan"; // Wind
        public static readonly string Ronboard = "Ronbaord"; // Wind
        public static readonly string Dosam = "Dosam"; // Wind

        public static readonly string Tinedo = "Tinedo"; // Elec
        public static readonly string Xtrasu = "X'Trasu"; // Elec
        public static readonly string Laanard = "Laanard"; // Elec

        public static readonly string Earic = "Earic"; // Light
        public static readonly string LatauVHurquij = "Latau V'Hurquij"; // Light
        public static readonly string Hallou = "Hallou"; // Light

        public static readonly string Baring = "Baring"; // Dark
        public static readonly string Tami = "Tami"; // Dark
        public static readonly string Dinowaru = "Dinowaru"; // Dark

        public static readonly string Groskopf = "Groskopf"; // Dark
        public static readonly string Onay = "Onay"; // Fire
        public static readonly string Aydinc = "Aydinc"; // Wind
        public static readonly string Kory = "Kory"; // Elec
        public static readonly string Claude = "Claude"; // Light
        public static readonly string Burckhard = "Burckhard"; // Ice

        // Fusion 6 - Fire, Ice, Wind, Elec, Dark, Light
        public static readonly string Lozinul = "Lozinul";
        public static readonly string Uvale = "Uvale";
        public static readonly string Dasinevu = "Dasinevu";
        public static readonly string Dome = "Dome";
        public static readonly string Snans = "Snans";
        public static readonly string Famber = "Famber";

        // Fusion 6 - Fire, Ice, Wind, Elec,Dark, Light
        public static readonly string Sacha_Kegul = "Sacha Kegul";
        public static readonly string Qrok_Emut = "Qrok Emut";
        public static readonly string Towane = "Towane";
        public static readonly string Reeclacwu = "Reeclacwu";
        public static readonly string ShadowNinja = "ShadowNinja";
        public static readonly string Valuvari = "Valuvari";
    }

    public static class EnemyNames
    {
        // Enemies (Tutorial)
        public static readonly string Conlen = "Conlen";
        public static readonly string Liamlas = "Liamlas";
        public static readonly string Orachar = "Orahcar";
        public static readonly string Fastrobren = "Fastrobren";
        public static readonly string CattuTDroni = "Cattu T'Droni";

        // Enemies (Randomized for Floors)
        // alternate enemies (no weakness)
        public static readonly string Fledan = "Fledan";
        public static readonly string Walds = "Walds";
        public static readonly string Paca = "Paca";
        public static readonly string Wigfred = "Wigfred";
        public static readonly string Lyley = "Lyley";
        public static readonly string Aboleth = "Aboleth";
        public static readonly string Hollyshimmer = "Hollyshimmer";
        public static readonly string Albedo = "Albedo";
        public static readonly string Cinnamonstem = "Cinnamonstem";
        public static readonly string Grizzleboink_the_Noodle_Snatcher = "Grizzleboink the Noodle-Snatcher";

        // agro - status
        public static readonly string Thylaf = "Thylaf"; // elec
        public static readonly string Arwig = "Arwig"; // ice
        public static readonly string Riccman = "Riccman"; // wind
        public static readonly string Gormacwen = "Gormacwen"; // fire
        public static readonly string Vidwerd = "Vidwerd"; // dark

        // wex hunters
        public static readonly string Isenald = "Isenald"; // light
        public static readonly string Gardmuel = "Gardmuel"; // dark
        public static readonly string Sachael = "Sachael"; // fire

        public static readonly string Pebrand = "Pebrand"; // ice
        public static readonly string Leofuwil = "Leofuwil"; // elec

        // copy cats
        public static readonly string Naldbear = "Naldbear"; // elec
        public static readonly string Stroma_Hele = "Stroma Hele"; // fire
        public static readonly string Sylla = "Sylla"; // wind
        public static readonly string Venforth = "Venforth"; // ice

        // resistance changers
        public static readonly string Thony = "Thony"; // Ice --> Fire
        public static readonly string Conson = "Conson"; // Light --> Dark

        // protector enemies
        public static readonly string Ed = "Ed"; // protects fire
        public static readonly string Otem = "Otem"; // protects ice
        public static readonly string Hesret = "Hesret"; // protects wind
        public static readonly string LaChris = "La-chris"; // protects wind

        // more alt enemies w/ buffs
        public static readonly string Nanfrea = "Nanfrea"; // boost fire
        public static readonly string Anrol = "Anrol"; // boost ice
        public static readonly string David = "Da-vid"; // boost wind
        public static readonly string Ferza = "Ferza"; // boost elec
        public static readonly string Garcar = "Garcar"; // boost dark
        public static readonly string Wennald = "Wennald"; // boost light

        // enemies with all-hits
        public static readonly string Aldmas = "Aldmas"; // all-hit fire (AE)
        public static readonly string Fridan = "Fridan"; // all-hit force (AE)
        public static readonly string Rahfortin = "Rahfortin"; // all-hit WEX + dark (AE)
        public static readonly string Leswith = "Leswith"; // all-hit WEX + elec (AE)

        // enemies that just provide turns
        public static readonly string Bue = "Bue"; // (AE) turns = 2, ice
        public static readonly string Bued = "Bued"; // (AE) turns = 2, fire
        public static readonly string Bureen = "Bureen"; // (AE) turns = 2, wind
        public static readonly string Buight = "Buight"; // (AE) turns = 2, light
        public static readonly string Builectric = "Builectric"; // (AE) turns = 2, elec
        public static readonly string Burk = "Burk"; // (AE) turns = 2, dark

        // eye enemies
        public static readonly string Acardeb = "Acardeb";
        public static readonly string Darol = "Darol";
        public static readonly string Hesbet = "Hesbet";
        public static readonly string Nolat = "Nolat";
        public static readonly string Kuo_toa = "Kuo-toa";

        // random enemies
        public static readonly string Charcas = "Charcas";
        public static readonly string Samjaris = "Samjaris";
        public static readonly string Tily = "Tily";
        public static readonly string ReeshiDeeme = "Reeshi Deeme";
        public static readonly string Nanles = "Nanles";
        public static readonly string Lyelof = "Lyelof";
        public static readonly string Keri = "Keri";
        public static readonly string FoameShorti = "Foame Shorti";
        public static readonly string Ethel = "Ethel";
        public static readonly string DrigaBoli = "Driga Boli";
        public static readonly string ChAffar = "Ch'Affar";
        public static readonly string Ardeb = "Ardeb";
        public static readonly string Ansung = "Ansung";
        public static readonly string Hahere = "Hahere";
        public static readonly string Brast = "Brast";
        public static readonly string Locfridegel = "Locfridegel";
        public static readonly string Garo = "Garo";
        public static readonly string Casgifu = "Casgifu";
        public static readonly string Kryonii = "Kryonii";
        public static readonly string Paron = "Paron";
        public static readonly string Geortom = "Geortom";
        public static readonly string Ordtheod = "Ordtheod";

        // MAD
        public static readonly string Elmaulgikr = "Elmaulgikr";
        public static readonly string Vualdr = "Vualdr";
        public static readonly string Zadzek = "Zadzek";
        public static readonly string Skotzag = "Skotzag";
        public static readonly string Gostviltirst = "Gostviltirst";
        public static readonly string Phorna = "Phorna";

        // STUN
        public static readonly string Zalth = "Zalth";
        public static readonly string Ingesc = "Ingesc"; // dark
        public static readonly string Isumforth = "Isumforth"; // protects dark

        // POISON
        public static readonly string Bernasbeorth = "Bernasbeorth"; // Elec --> Wind
        public static readonly string Iaviol = "Iaviol";
        public static readonly string Olu = "Olu";

        // DINOSAURS RAAAWWW
        public static readonly string Yodigrin = "Yodigrin";
        public static readonly string Vustuma = "Vustuma";
        public static readonly string Gupmoth = "Gupmoth";
        public static readonly string Maltamos = "Maltamos";
        public static readonly string Rusnopi = "Rusnopi";
        public static readonly string Uptali = "Uptali";
        public static readonly string Sufnod = "Sufnod";
        public static readonly string Ket = "Ket";
        public static readonly string Khasterat = "Khasterat";
        public static readonly string Palmonu = "Palmonu";
        public static readonly string Baos = "Baos";
        public static readonly string Cendros = "Cendros";
        public static readonly string Rigratos = "Rigratos";
        public static readonly string Zorliros = "Zorliros";
        public static readonly string Zervos = "Zervos";
        public static readonly string Vaphos = "Vaphos";
        public static readonly string Leos = "Leos";
        public static readonly string Camnonos = "Camnonos";
        public static readonly string Ridravos = "Ridravos";
        public static readonly string Raos = "Raos";

        // Bosses (Normal)
        public static readonly string Harbinger = "Harbinger, Mangler of Legs";
        public static readonly string Elliot_Onyx = "Elliot Onyx";
        public static readonly string Sable_Vonner = "Sable Vonner";
        public static readonly string Cloven_Umbra = "Cloven Umbra";
        public static readonly string Ashen_Ash = "Ashen Ash";
        public static readonly string Ethel_Aura = "Ethel Aura";
        public static readonly string Kellam_Von_Stein = "Kellam Von Stein";
        public static readonly string Drace_Skinner = "Drace Skinner";
        public static readonly string Jude_Stone = "Jude Stone";
        public static readonly string Drace_Razor = "Drace Razor";
        public static readonly string Everit_Pickerin = "Everit Pickerin";
        public static readonly string Alex_Church = "Alex Church";
        public static readonly string Griffen_Hart = "Griffen Hart";
        public static readonly string Bohumir_Cibulka = "Bohumir Cibulka";
        public static readonly string Zell_Grimsbane = "Zell Grimsbane";
        public static readonly string Soren_Winter = "Soren Winter";
        public static readonly string Requiem_Heliot = "Requiem Heliot";
        public static readonly string Not = "Not";
        public static readonly string Sable_Craft = "Sable Craft";
        public static readonly string Cinder_Morgan = "Cinder Morgan";
        public static readonly string Granger_Barlow = "Granger Barlow";
        public static readonly string Thorne_Lovelace = "Thorne Lovelace";
        public static readonly string Morden_Brack = "Morden Brack";
        public static readonly string Law_Vossen = "Law Vossen";
        public static readonly string Arc_Hunt = "Arc Hunt";
        public static readonly string Buceala = "Buceala";
        public static readonly string Storm_Vossen = "Storm Vossen";

        // Bosses (Dungeon)
        public static readonly string Ocura = "Ocura";
        public static readonly string Emush = "Emush";
        // 150
        public static readonly string Hrothstyr_Zarmor = "Hrothstyr Zarmor";
        public static readonly string Logvat = "Logvat";
        public static readonly string Theodstin_Glove = "Theodstin Glove";
        // 200
        public static readonly string Pleromyr = "Pleromyr";
        // 250
        public static readonly string Kodek = "Kodek";

        // Bosses (Random Dungeon)
        public static readonly string Algrools = "Algrools";
        public static readonly string Gos = "Gos";
        public static readonly string Laltujass = "Laltujass";
        public static readonly string Pool = "Pool";
        public static readonly string Qibrel = "Qibrel";
        public static readonly string Sirgopes = "Sirgopes";
        public static readonly string Suamgu = "Suamgu";
        public static readonly string Vrasrohd = "Vrasrohd";
        public static readonly string Vrosh = "Vrosh";
        public static readonly string Zaaxtrul = "Zaaxtrul";
        public static readonly string Udeon = "Udeon";
        public static readonly string Dremphannyal = "Dremphannyal";
        public static readonly string Mugogon = "Mugogon";
        public static readonly string Nainlifael = "Nainlifael";

        // Bind enemies
        public static readonly string Iji = "Iji";
        public static readonly string Sezuzo = "Sezuzo";
        public static readonly string Tilaza_Fado = "Tilaza Fado";
        public static readonly string Tuidonak = "Tuidonak";

        // Seal Enemies
        public static readonly string Enu = "Enu";
        public static readonly string Juye = "Juye";
        public static readonly string Dahone_Zude = "Dahone Zude";
        public static readonly string Soredr = "Soredr";

        // Res Changer/Sprt
        public static readonly string Ouadia = "Ouadia";
        public static readonly string Jiochroudice = "Jiochroudice";
        public static readonly string Hugline = "Hugline";
        public static readonly string Danyll = "Danyll";

        // new enemies
        public static readonly string Bazzaelth = "Bazzaelth";
        public static readonly string Culdra = "Culdra";
        public static readonly string Lord = "Lord";
        public static readonly string Dimnain = "Dimnain";
        public static readonly string Green_Reaper = "Green Reaper";
        public static readonly string Black_Getter = "Black Getter";

        public static readonly string Piazeor = "Piazeor";
        public static readonly string Kodose = "Kodose";
        public static readonly string Omre = "Omre";
        public static readonly string Uri = "Uri";
        public static readonly string Zaalki = "Zaalki";
        

        // Final bosses
        public static readonly string Nettala = "Nettala";
        public static readonly string Draco = "Draco";
        public static readonly string Drakalla = "Drakalla";

        // Bounty bosses
        public static readonly string Sentinal = "Sentinal";
        public static readonly string Ancient_Nodys = "Ancient Nodys";
        public static readonly string Iminth = "Iminth";
        public static readonly string Pakorag = "Pakorag";
        public static readonly string Floor_Architect = "Floor Architect";

        // Elders
        public static readonly string Aiucxaiobhlo = "Aiucxaiobh'lo";
        public static readonly string Bhotldren = "Bhotl'dren";
        public static readonly string Ghryztitralbh = "Ghryzt'itralbh";
        public static readonly string Mhaarvosh = "Mh'aarvosh";
        public static readonly string Yacnacnalb = "Yacnacnalb";

        // Tikki
        public static readonly string FireTikki = "Buifirikki Tibukki";
        public static readonly string IceTikki = "Biecikki Bucita";
        public static readonly string ElecTikki = "Beletribulikki Shocklani";
        public static readonly string DarkTikki = "Budarikki Karknala";
        public static readonly string LightTikki = "Bulikki Ightka";
        public static readonly string WindTikki = "Buwikki Tind";
    }
}
