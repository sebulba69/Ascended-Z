using AscendedZ.entities;
using AscendedZ.skills;
using AscendedZ.statuses;
using AscendedZ.statuses.buff_elements;
using AscendedZ.statuses.void_elements;
using AscendedZ.statuses.weak_element;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AscendedZ
{
    public static class SkillDatabase
    {
        public static readonly Dictionary<Elements, Elements> ElementalOpposites = new Dictionary<Elements, Elements>
        {
            { Elements.Fire, Elements.Ice },
            { Elements.Ice, Elements.Fire },
            { Elements.Wind, Elements.Elec },
            { Elements.Elec, Elements.Wind },
            { Elements.Light, Elements.Dark },
            { Elements.Dark, Elements.Light }
        };

        #region Tiered Skills
        public static ElementSkill Elec1 { get => CreateSingleHitElementSkill("Spark", Elements.Elec, false); }
        public static ElementSkill Fire1 { get => CreateSingleHitElementSkill("Singe", Elements.Fire, false); }
        public static ElementSkill Ice1 { get => CreateSingleHitElementSkill("Shiver", Elements.Ice, false); }
        public static ElementSkill Light1 { get => CreateSingleHitElementSkill("Gleam", Elements.Light, false); }
        public static ElementSkill Wind1 { get => CreateSingleHitElementSkill("Breeze", Elements.Wind, false); }
        public static ElementSkill Dark1 { get => CreateSingleHitElementSkill("Shadow", Elements.Dark, false); }
        public static ElementSkill Almighty1 { get => CreateSingleHitElementSkill("Nebula", Elements.Almighty, false); }

        public static ElementSkill PierceElec1 { get => CreateSingleHitElementSkill("Pierce Spark", Elements.Elec, true); }
        public static ElementSkill PierceFire1 { get => CreateSingleHitElementSkill("Pierce Flame", Elements.Fire, true); }
        public static ElementSkill PierceIce1 { get => CreateSingleHitElementSkill("Pierce Ice", Elements.Ice, true); }
        public static ElementSkill PierceLight1 { get => CreateSingleHitElementSkill("Pierce Ray", Elements.Light, true); }
        public static ElementSkill PierceWind1 { get => CreateSingleHitElementSkill("Pierce Wind", Elements.Wind, true); }
        public static ElementSkill PierceDark1 { get => CreateSingleHitElementSkill("Pierce Dark", Elements.Dark, true); }

        public static ElementSkill ElecAll { get => CreateMultiHitElementSkill("Zap", Elements.Elec); }
        public static ElementSkill FireAll { get => CreateMultiHitElementSkill("Flame", Elements.Fire); }
        public static ElementSkill IceAll { get => CreateMultiHitElementSkill("Ice", Elements.Ice); }
        public static ElementSkill LightAll { get => CreateMultiHitElementSkill("Beam", Elements.Light); }
        public static ElementSkill WindAll { get => CreateMultiHitElementSkill("Gust", Elements.Wind); }
        public static ElementSkill DarkAll { get => CreateMultiHitElementSkill("Darkness", Elements.Dark); }
        public static ElementSkill Almighty { get => CreateMultiHitElementSkill("Galaxy", Elements.Almighty); }
        public static ElementSkill AlmightyWipe 
        {
            get 
            { 
                var p = CreateMultiHitElementSkill("Black Hole", Elements.Almighty, 10000);

                p.ForceWex = true;

                return p;
            }
        }
        public static ElementSkill ElecAllP 
        {
            get 
            { 
                var skill = CreateMultiHitElementSkill("Savage Thunder", Elements.Elec);
                skill.Piercing = true;
                return skill;
            } 
        }

        public static ElementSkill FireAllP
        {
            get
            {
                var skill = CreateMultiHitElementSkill("Savage Flames", Elements.Fire);
                skill.Piercing = true;
                return skill;
            }
        }

        public static ElementSkill IceAllP
        {
            get
            {
                var skill = CreateMultiHitElementSkill("Savage Chill", Elements.Ice);
                skill.Piercing = true;
                return skill;
            }
        }

        public static ElementSkill WindAllP
        {
            get
            {
                var skill = CreateMultiHitElementSkill("Savage Winds", Elements.Wind);
                skill.Piercing = true;
                return skill;
            }
        }


        public static ElementSkill DarkAllP
        {
            get
            {
                var skill = CreateMultiHitElementSkill("Savage Darkness", Elements.Dark);
                skill.Piercing = true;
                return skill;
            }
        }


        public static ElementSkill LightAllP
        {
            get
            {
                var skill = CreateMultiHitElementSkill("Savage Radiance", Elements.Light);
                skill.Piercing = true;
                return skill;
            }
        }


        public static ElementSkill SkillCopy { get => CreateMultiHitElementSkill("Skill Copy", Elements.Almighty); }
        public static ElementSkill DracoTherium { get => CreateMultiHitElementSkill("Draco Therium", Elements.Dark); } // unique skill for Kellam
        public static StatusSkill Jyndesdarth
        {
            get
            {
                var jyndesdarth = MakeStatusSkill("Jyndesdarth", new Jyndesdarth());
                jyndesdarth.Icon = SkillAssets.ALMIGHT_ICON;
                jyndesdarth.EndupAnimation = SkillAssets.ALMIGHTY;
                jyndesdarth.TargetType = TargetTypes.SELF;
                return jyndesdarth;
            }
        }


        public static ElementSkill HolyLight
        {
            get
            {
                var skill = CreateMultiHitElementSkill("Holy Light", Elements.Light);
                skill.Statuses.Add(Torpefy);
                return skill;
            }
        }
        #endregion
        #region Evil Combo skills
        public static ElementSkill Fredmuelald
        {
            get
            {
                var fredmuelald = CreateSingleHitElementSkill("Fredmuelald", Elements.Fire, false);
                fredmuelald.Statuses.Add(Poison);

                return fredmuelald;
            }
        }

        public static ElementSkill BindFire
        {
            get
            {
                var fredmuelald = CreateSingleHitElementSkill("Bind Fire", Elements.Fire, false);
                fredmuelald.Statuses.Add(Bind);

                return fredmuelald;
            }
        }

        public static ElementSkill Sarawaldbet
        {
            get
            {
                var sarawaldbet = CreateSingleHitElementSkill("Sarawaldbet", Elements.Ice, false);
                sarawaldbet.Statuses.Add(Bind);

                return sarawaldbet;
            }
        }

        public static ElementSkill OblivionsEmbrace
        {
            get
            {
                var oblivionsEmbrace = CreateSingleHitElementSkill("Oblivion's Embrace", Elements.Dark, false);
                oblivionsEmbrace.Statuses.Add(DefDebuff);

                return oblivionsEmbrace;
            }
        }

        public static ElementSkill ArcFlash
        {
            get
            {
                var arcFlash = CreateSingleHitElementSkill("Arc Flash", Elements.Light, false);
                arcFlash.Statuses.Add(Bind);

                return arcFlash;
            }
        }
        

        public static ElementSkill Hellfire 
        { 
            get 
            { 
                var hellfire = CreateMultiHitElementSkill("Hellfire", Elements.Fire);
                hellfire.Statuses.Add(Poison);

                return hellfire;
            } 
        }

        public static ElementSkill Debilifire
        {
            get
            {
                var debiliFire = CreateMultiHitElementSkill("Debilifire", Elements.Fire);
                debiliFire.Statuses.Add(Debilitate);

                return debiliFire;
            }
        }

        public static ElementSkill Curse
        {
            get
            {
                var curse = CreateMultiHitElementSkill("Curse", Elements.Dark);
                curse.Statuses.Add(DefDebuff);

                return curse;
            }
        }

        public static ElementSkill ZephyrShield
        {
            get
            {
                var zephyrShield = CreateSingleHitElementSkill("Zephyr Shield", Elements.Wind, false);
                zephyrShield.Statuses.Add(Seal);

                return zephyrShield;
            }
        }


        public static ElementSkill Eliricpaul
        {
            get
            {
                var zephyrShield = CreateSingleHitElementSkill("Eliricpaul", Elements.Elec, false);
                zephyrShield.Statuses.Add(AtkDebuff);

                return zephyrShield;
            }
        }

        public static ElementSkill Antitichton
        {
            get
            {
                var zephyrShield = CreateMultiHitElementSkill("Antitichton", Elements.Almighty);
                zephyrShield.Statuses.Add(Debilitate);

                return zephyrShield;
            }
        }

        public static ElementSkill FractalBeam
        {
            get
            {
                var beam = CreateSingleHitElementSkill("Fractal Beam", Elements.Almighty, false);
                beam.ForceWex = true;
                return beam;
            }
        }

        public static ElementSkill DeadlyWind
        {
            get
            {
                var deadlyWind = CreateSingleHitElementSkill("Deadly Wind", Elements.Wind, false);
                deadlyWind.Statuses.Add(Poison);

                return deadlyWind;
            }
        }

        public static ElementSkill AlmightyMadGod
        {

            get
            {
                var almMG = CreateSingleHitElementSkill("Cosmos of the Mad God", Elements.Almighty, false);
                almMG.Statuses.AddRange([Poison, Seal, Bind]);

                return almMG;
            }
        }

        public static ElementSkill FireMadGod
        {

            get
            {
                var fireMG = CreateSingleHitElementSkill("Flame of the Mad God", Elements.Fire, false);
                fireMG.Statuses.AddRange([Poison, Seal, Bind]);

                return fireMG;
            }
        }

        public static ElementSkill FireMadGodAll
        {

            get
            {
                var fireMG = CreateMultiHitElementSkill("Inferno of the Mad God", Elements.Fire);
                fireMG.Statuses.AddRange([Poison, Seal, Bind]);

                return fireMG;
            }
        }

        public static ElementSkill FireMadGodAllP
        {

            get
            {
                var fireMG = CreateMultiHitElementSkill("Prometheus' Flame", Elements.Fire);
                fireMG.Statuses.AddRange([Poison, Seal, Bind]);
                fireMG.Piercing = true;
                return fireMG;
            }
        }

        public static ElementSkill IceMadGod
        {

            get
            {
                var iceMG = CreateSingleHitElementSkill("Glacier of the Mad God", Elements.Ice, false);
                iceMG.Statuses.AddRange([Poison, Seal, Bind]);

                return iceMG;
            }
        }

        public static ElementSkill IceMadGodAll
        {

            get
            {
                var iceMG = CreateMultiHitElementSkill("Hail of the Mad God", Elements.Ice);
                iceMG.Statuses.AddRange([Poison, Seal, Bind]);

                return iceMG;
            }
        }

        public static ElementSkill IceMadGodAllP
        {

            get
            {
                var iceMG = CreateMultiHitElementSkill("Skadi's Mountain", Elements.Ice);
                iceMG.Statuses.AddRange([Poison, Seal, Bind]);
                iceMG.Piercing = true;
                return iceMG;
            }
        }

        public static ElementSkill WindMadGod
        {

            get
            {
                var windMG = CreateSingleHitElementSkill("Tornado of the Mad God", Elements.Wind, false);
                windMG.Statuses.AddRange([Poison, Seal, Bind]);

                return windMG;
            }
        }

        public static ElementSkill WindMadGodAll
        {

            get
            {
                var windMG = CreateMultiHitElementSkill("Tempest of the Mad God", Elements.Wind);
                windMG.Statuses.AddRange([Poison, Seal, Bind]);

                return windMG;
            }
        }

        public static ElementSkill WindMadGodAllP
        {

            get
            {
                var windMG = CreateMultiHitElementSkill("Shu's Wrath", Elements.Wind);
                windMG.Statuses.AddRange([Poison, Seal, Bind]);
                windMG.Piercing = true;
                return windMG;
            }
        }

        public static ElementSkill ElecMadGod
        {

            get
            {
                var elecMG = CreateSingleHitElementSkill("Storm of the Mad God", Elements.Elec, false);
                elecMG.Statuses.AddRange([Poison, Seal, Bind]);

                return elecMG;
            }
        }

        public static ElementSkill ElecMadGodAll
        {

            get
            {
                var elecMG = CreateMultiHitElementSkill("Flash of the Mad God", Elements.Elec);
                elecMG.Statuses.AddRange([Poison, Seal, Bind]);

                return elecMG;
            }
        }

        public static ElementSkill ElecMadGodAllP
        {

            get
            {
                var elecMG = CreateMultiHitElementSkill("Zeus' Bolt", Elements.Elec);
                elecMG.Statuses.AddRange([Poison, Seal, Bind]);
                elecMG.Piercing = true;

                return elecMG;
            }
        }

        public static ElementSkill LightMadGod
        {

            get
            {
                var lightMG = CreateSingleHitElementSkill("Ray of the Mad God", Elements.Light, false);
                lightMG.Statuses.AddRange([Poison, Seal, Bind]);

                return lightMG;
            }
        }


        public static ElementSkill LightMadGodAll
        {

            get
            {
                var mgSkill = CreateMultiHitElementSkill("Beam of the Mad God", Elements.Light);
                mgSkill.Statuses.AddRange([Poison, Seal, Bind]);
                return mgSkill;
            }
        }

        public static ElementSkill LightMadGodAllP
        {

            get
            {
                var mgSkill = CreateMultiHitElementSkill("Ray of Salvation", Elements.Light);
                mgSkill.Statuses.AddRange([Poison, Seal, Bind]);
                mgSkill.Piercing = true;
                return mgSkill;
            }
        }

        public static ElementSkill DarkMadGod
        {

            get
            {
                var darkMG = CreateSingleHitElementSkill("Curse of the Mad God", Elements.Dark, false);
                darkMG.Statuses.AddRange([Poison, Seal, Bind]);

                return darkMG;
            }
        }

        public static ElementSkill DarkMadGodAll
        {

            get
            {
                var mgSkill = CreateMultiHitElementSkill("Vortex of the Mad God", Elements.Dark);
                mgSkill.Statuses.AddRange([Poison, Seal, Bind]);
                return mgSkill;
            }
        }

        public static ElementSkill DarkMadGodAllP
        {

            get
            {
                var mgSkill = CreateMultiHitElementSkill("Hades Blast", Elements.Dark);
                mgSkill.Statuses.AddRange([Poison, Seal, Bind]);
                mgSkill.Piercing = true;
                return mgSkill;
            }
        }
        #endregion

        private static readonly Dictionary<Elements, ElementSkill> SingleHitElementSkills = new Dictionary<Elements, ElementSkill>()
        {
            { Elements.Elec, Elec1 },
            { Elements.Fire, Fire1 },
            { Elements.Ice, Ice1 },
            { Elements.Light, Light1 },
            { Elements.Wind, Wind1 },
            { Elements.Dark, Dark1 }
        };

        private static readonly Dictionary<Elements, ElementSkill> AllHitElementSkills = new Dictionary<Elements, ElementSkill>() 
        {
            { Elements.Elec, ElecAll },
            { Elements.Fire, FireAll },
            { Elements.Ice, IceAll },
            { Elements.Light, LightAll },
            { Elements.Wind, WindAll },
            { Elements.Dark, DarkAll },
            { Elements.Almighty, Almighty }
        };

        private static ElementSkill CreateSingleHitElementSkill(string name, Elements element, bool isPiercing)
        {
            var skill = MakeNewSingleHitElement(name, element, 2, 1);
            skill.Piercing = isPiercing;
            return skill;
        }

        private static ElementSkill CreateMultiHitElementSkill(string name, Elements element, int dmgOverride = 1)
        {
            var s = MakeNewSingleHitElement(name, element, dmgOverride, 2);
            s.TargetType = TargetTypes.OPP_ALL;
            return s;
        }

        public static ISkill GetSkillFromElement(Elements element)
        {
            var skills = new ElementSkill[]{ Fire1, Ice1, Elec1, Wind1, Dark1, Light1 };
            return skills[(int)element].Clone();
        }

        private static ElementSkill MakeNewSingleHitElement(string name, Elements element, int damage, int tier)
        {
            return new ElementSkill
            {
                BaseName = name,
                Damage = damage,
                TargetType = TargetTypes.SINGLE_OPP,
                Element = element,
                StartupAnimation = SkillAssets.STARTUP1_MG,
                EndupAnimation = SkillAssets.GetAnimationByElementAndTier(tier, element),
                Icon = SkillAssets.GetElementIconByElementEnum(element),
                Tier = tier
            };
        }

        public static StatusSkill ElecBuff1 { get => MakeBuffSkill("Elec+", new BuffElecStatus { Amount = 0.25 }); }
        public static StatusSkill FireBuff1 { get => MakeBuffSkill("Fire+", new BuffFireStatus { Amount = 0.25 }); }
        public static StatusSkill WindBuff1 { get => MakeBuffSkill("Wind+", new BuffWindStatus { Amount = 0.25 }); }
        public static StatusSkill IceBuff1 { get => MakeBuffSkill("Ice+", new BuffIceStatus { Amount = 0.25 }); }
        public static StatusSkill LightBuff1 { get => MakeBuffSkill("Light+", new BuffLightStatus { Amount = 0.25 }); }
        public static StatusSkill DarkBuff1 { get => MakeBuffSkill("Dark+", new BuffDarkStatus { Amount = 0.25 }); }

        public static StatusSkill LusterCandy
        {
            get
            {
                var lusterCandy = MakeMultiFlatBuff("Luster Candy", SkillAssets.LUSTER_CANDY, [new AtkChangeStatus(), new DefChangeStatus()]);
                lusterCandy.EndupAnimation = SkillAssets.ATK_BUFF;
                lusterCandy.SetDescription("Boost team attack and defense for all elements by 15%. Caps at 2 stacks.");
                return lusterCandy;
            }
        }

        public static StatusSkill Debilitate
        {
            get
            {
                var debilitate = MakeMultiFlatDebuff("Debilitate", SkillAssets.DEBILITATE, [new AtkChangeStatus(), new DefChangeStatus()]);
                debilitate.SetDescription("Decrease enemy team attack and defense for all elements by 15%. Caps at 2 stacks.");
                return debilitate;
            }
        }

        public static StatusSkill HolyGrail
        {
            get
            {
                var lusterCandy = MakeMultiFlatBuff("Holy Grail", SkillAssets.LUSTER_CANDY, 
                    [new AtkChangeStatus(), 
                    new AtkChangeStatus(), 
                    new DefChangeStatus(),
                    new DefChangeStatus()
                    ]);
                lusterCandy.EndupAnimation = SkillAssets.ATK_BUFF;
                lusterCandy.SetDescription("Boost team attack and defense (x2) for all elements by 15%. Caps at 2 stacks.");
                return lusterCandy;
            }
        }

        public static StatusSkill Torpefy
        {
            get
            {
                var debilitate = MakeMultiFlatDebuff("Torpefy", SkillAssets.DEBILITATE, 
                    [
                    new AtkChangeStatus(), 
                    new AtkChangeStatus(), 
                    new DefChangeStatus(),
                    new DefChangeStatus()
                    ]);
                debilitate.SetDescription("Decrease enemy team attack and defense (x2) for all elements by 15%. Caps at 2 stacks.");
                return debilitate;
            }
        }

        public static StatusSkill AtkBuff { get
            {
                var atk = MakeFlatBuffAll("Atk+", SkillAssets.ATK_PLUS_ICON, new AtkChangeStatus());
                atk.EndupAnimation = SkillAssets.ATK_BUFF;
                atk.SetDescription("Boost team attack for all elements by 15%. Caps at 2 stacks.");
                return atk;
            }
        }

        public static StatusSkill DefBuff 
        { 
            get 
            {
                var def = MakeFlatBuffAll("Def+", SkillAssets.DEF_PLUS_ICON, new DefChangeStatus());
                def.SetDescription("Boost team defense against all elements by 15%. Caps at 2 stacks.");
                return def;
            } 
        }
        public static StatusSkill AtkDebuff 
        { 
            get
            {
                var atkM = MakeFlatDebuffAll("Atk-", SkillAssets.ATK_MINUS_ICON, new AtkChangeStatus());
                atkM.SetDescription("Decrease enemy team attack for all elements by 15%. Caps at 2 stacks.");
                return atkM;
            }
        }

        public static StatusSkill DefDebuff 
        {
            get 
            {
                var defM = MakeFlatDebuffAll("Def-", SkillAssets.DEF_MINUS_ICON, new DefChangeStatus());
                defM.SetDescription("Decrease enemy team defense against all elements by 15%. Caps at 2 stacks.");
                return defM;
            } 
        }

        public static StatusSkill TechBuff 
        {
            get 
            { 
                var tech = MakeFlatBuff("Tech+", SkillAssets.TECH_PLUS_ICON, new TechnicalStatus());
                tech.EndupAnimation = SkillAssets.TECH_BUFF;
                tech.SetDescription("Applies the Technical status to a player.\nTechnicals treat all non-Null/Drained attacks as weaknesses.");
                return tech;
            } 
        }

        public static StatusSkill TechBuffAll
        {
            get
            {
                var techAll = TechBuff;
                techAll.BaseName = "Tech+ All";
                techAll.TargetType = TargetTypes.TEAM_ALL;
                return techAll;
            }
        }

        public static StatusSkill TechDebuff 
        {
            get 
            { 
                var techMinus = MakeFlatDebuff("Tech-", SkillAssets.TECH_MINUS_ICON, new TechnicalStatus());
                techMinus.SetDescription("Removes the Technical status if the enemy has it.");
                return techMinus;
            }
        }

        public static StatusSkill TechDebuffAll
        {
            get
            {
                var techMinus = MakeFlatDebuffAll("Tech- All", SkillAssets.TECH_MINUS_ICON, new TechnicalStatus());
                techMinus.SetDescription("Removes the Technical status if the enemy has it.");
                return techMinus;
            }
        }

        public static StatusSkill SilentPrayer
        {
            get
            {
                var silentPrayer = MakeMultiStatusSkill("Silent Prayer", [new AtkChangeStatus(), new DefChangeStatus()]);
                silentPrayer.Icon = SkillAssets.SILENT_PRAYER;
                silentPrayer.IsRemoveStatusSkill = true;
                silentPrayer.EndupAnimation = SkillAssets.VOID_SHIELD;
                silentPrayer.TargetType = TargetTypes.BOTH;

                return silentPrayer;
            }
        }

        public static StatusSkill AncientChoir
        {
            get
            {
                var silentPrayer = MakeMultiStatusSkill("Ancient Choir", [new AtkChangeStatus(), new DefChangeStatus()]);
                silentPrayer.Icon = SkillAssets.SILENT_PRAYER;
                silentPrayer.IsRemoveStatusSkill = true;
                silentPrayer.EndupAnimation = SkillAssets.TECH_BUFF;
                silentPrayer.TargetType = TargetTypes.BOTH;

                return silentPrayer;
            }
        }

        public static StatusSkill Confusion 
        { 
            get 
            {
                var confuse = MakeStatusSkill("Confusion Ray", new ConfuseStatus());
                confuse.Icon = SkillAssets.CONFUSION;
                confuse.TargetType = TargetTypes.SINGLE_OPP;

                confuse.EndupAnimation = SkillAssets.GetBuffAnimationByElement(Elements.Wind);
                return confuse;
            } 
        }

        public static StatusSkill Focus
        {
            get
            {
                var focus = MakeStatusSkill("Focus", new FocusStatus());
                focus.Icon = SkillAssets.FOCUS_ICON;
                focus.TargetType = TargetTypes.SELF;
                focus.EndupAnimation = SkillAssets.GetBuffAnimationByElement(Elements.Light);
                return focus;
            }
        }

        public static StatusSkill MarkOfDeathSingle
        {
            get
            {
                var markOfDeath = MakeStatusSkill("Mark of Death", new MarkOfDeathStatus());
                markOfDeath.Icon = SkillAssets.MARK_OF_DEATH_STATUS;
                markOfDeath.TargetType = TargetTypes.SINGLE_OPP;
                markOfDeath.EndupAnimation = SkillAssets.GetBuffAnimationByElement(Elements.Dark);
                return markOfDeath;
            }
        }
        public static StatusSkill MarkOfDeathAll
        {
            get
            {
                var markOfDeath = MarkOfDeathSingle;
                markOfDeath.Icon = SkillAssets.MARK_OF_DEATH_STATUS;
                markOfDeath.TargetType = TargetTypes.OPP_ALL;
                return markOfDeath;
            }
        }

        private static StatusSkill MakeBuffSkill(string name, ElementBuffStatus status)
        {
            StatusSkill statusSkill = MakeStatusSkill(name, status);
            statusSkill.EndupAnimation = SkillAssets.GetBuffAnimationByElement(status.BuffElement);
            statusSkill.Icon = SkillAssets.GetElementIconByElementEnum(status.BuffElement);

            return statusSkill;
        }

        private static StatusSkill MakeFlatBuff(string name, string icon, Status buffStatus)
        {
            StatusSkill buff = MakeStatusSkill(name, buffStatus);
            buff.EndupAnimation = SkillAssets.FLAT_BUFF;
            buff.Icon = icon;
            buff.TargetType = TargetTypes.SINGLE_TEAM;

            return buff;
        }

        private static StatusSkill MakeMultiFlatBuff(string name, string icon, List<Status> buffStatuses)
        {
            StatusSkill buff = MakeMultiStatusSkill(name, buffStatuses);
            buff.EndupAnimation = SkillAssets.FLAT_BUFF;
            buff.Icon = icon;
            buff.TargetType = TargetTypes.TEAM_ALL;

            return buff;
        }

        private static StatusSkill MakeFlatDebuff(string name, string icon, Status debuffStatus)
        {
            StatusSkill debuff = MakeStatusSkill(name, debuffStatus);
            debuff.EndupAnimation = SkillAssets.FLAT_DEBUFF;
            debuff.Icon = icon;
            debuff.TargetType = TargetTypes.SINGLE_OPP;
            debuff.IsCounterDecreaseStatus = true;

            return debuff;
        }

        private static StatusSkill MakeMultiFlatDebuff(string name, string icon, List<Status> debuffStatuses)
        {
            StatusSkill debuff = MakeMultiStatusSkill(name, debuffStatuses);
            debuff.EndupAnimation = SkillAssets.FLAT_DEBUFF;
            debuff.Icon = icon;
            debuff.TargetType = TargetTypes.OPP_ALL;
            debuff.IsCounterDecreaseStatus = true;

            return debuff;
        }

        private static StatusSkill MakeFlatBuffAll(string name, string icon, Status buffStatus)
        {
            StatusSkill buffAll = MakeFlatBuff(name, icon, buffStatus);
            buffAll.TargetType = TargetTypes.TEAM_ALL;
            return buffAll;
        }

        private static StatusSkill MakeFlatDebuffAll(string name, string icon, Status debuffStatus)
        {
            StatusSkill debuff = MakeFlatDebuff(name, icon, debuffStatus);
            debuff.TargetType = TargetTypes.OPP_ALL;
            return debuff;
        }


        public static EyeSkill BeastEye
        {
            get
            {
                EyeSkill eyeSkill = new EyeSkill() 
                {
                    BaseName = "Beast's Eye",
                    TargetType = TargetTypes.SELF,
                    StartupAnimation = SkillAssets.STARTUP1_MG,
                    EndupAnimation = SkillAssets.EYESKILLANIM,
                    EyeType = battle.BattleResultType.BeastEye,
                    Icon = SkillAssets.BEAST_EYE
                };

                return eyeSkill;
            }
        }

        public static EyeSkill DragonEye
        {
            get
            {
                EyeSkill eyeSkill = new EyeSkill()
                {
                    BaseName = "Dragon Eye",
                    TargetType = TargetTypes.SELF,
                    StartupAnimation = SkillAssets.STARTUP1_MG,
                    EndupAnimation = SkillAssets.EYESKILLANIM,
                    EyeType = battle.BattleResultType.DragonEye,
                    Icon = SkillAssets.BEAST_EYE
                };

                return eyeSkill;
            }
        }

        public static StatusSkill Stun
        {
            get
            {
                StatusSkill statusSkill = MakeStatusSkill("Stun", new StunStatus());

                statusSkill.Icon = SkillAssets.STUN_ICON;
                statusSkill.EndupAnimation = SkillAssets.STUN_T1;

                return statusSkill;
            }
        }

        public static StatusSkill AgroEnemy
        {
            get
            {
                StatusSkill statusSkill = MakeStatusSkill("Agro", new AgroStatus());

                statusSkill.EndupAnimation = SkillAssets.AGRO;
                statusSkill.Icon = SkillAssets.AGRO_ICON;

                return statusSkill;
            }
        }

        public static StatusSkill AgroPlayer 
        { 
            get 
            {
                StatusSkill statusSkill = AgroEnemy;
                statusSkill.TargetType = TargetTypes.SINGLE_TEAM;

                return statusSkill;
            } 
        }

        public static StatusSkill VoidFire
        {
            get
            {
                var voidFire = MakeChangeElementSkill("Void Fire", new VoidFireStatus());
                voidFire.SetDescription("Sets player resistance to Fire to Null.");

                return voidFire;
            }
        }
        public static StatusSkill VoidIce
        {
            get
            {
                var voidIce = MakeChangeElementSkill("Void Ice", new VoidIceStatus());
                voidIce.SetDescription("Sets player resistance to Ice to Null.");

                return voidIce;
            }
        }
        public static StatusSkill VoidWind
        {
            get
            {
                var voidWind = MakeChangeElementSkill("Void Wind", new VoidWindStatus());
                voidWind.SetDescription("Sets player resistance to Wind to Null.");

                return voidWind;
            }
        }

        public static StatusSkill VoidDark
        {
            get
            {
                return MakeChangeElementSkill("Void Dark", new VoidDarkStatus());
            }
        }

        public static StatusSkill VoidLight
        {
            get
            {
                return MakeChangeElementSkill("Void Light", new VoidLightStatus());
            }
        }


        public static StatusSkill VoidElec
        {
            get
            {
                return MakeChangeElementSkill("Void Elec", new VoidElecStatus());
            }
        }

        public static StatusSkill WeakFire
        {
            get
            {
                var s = MakeChangeElementSkill("Fire-", new WeakFireStatus());
                s.TargetType = TargetTypes.SINGLE_OPP;
                return s;
            }
        }

        public static StatusSkill WeakIce
        {
            get
            {
                var s = MakeChangeElementSkill("Ice-", new WeakIceStatus());
                s.TargetType = TargetTypes.SINGLE_OPP;
                return s;
            }
        }

        public static StatusSkill WeakElec
        {
            get
            {
                var s = MakeChangeElementSkill("Elec-", new WeakElecStatus());
                s.TargetType = TargetTypes.SINGLE_OPP;
                return s;
            }
        }

        public static StatusSkill WeakAllElec
        {
            get
            {
                var s = MakeChangeElementSkill("All Elec-", new WeakElecStatus());
                s.TargetType = TargetTypes.OPP_ALL;
                return s;
            }
        }

        
        public static StatusSkill WeakAllDark
        {
            get
            {
                var s = MakeChangeElementSkill("All Dark-", new WeakDarkStatus());
                s.TargetType = TargetTypes.OPP_ALL;
                return s;
            }
        }

        public static StatusSkill RemoveVoidWind
        {
            get
            {
                var s = MakeChangeElementSkill("Remove Void Wind", new VoidWindStatus());
                s.IsRemoveStatusSkill = true;
                s.TargetType = TargetTypes.SINGLE_OPP;
                s.SetDescription("Remove the status Void Wind if an enemy has it.");
                return s;
            }
        }

        public static StatusSkill RemoveVoidFire
        {
            get
            {
                var s = MakeChangeElementSkill("Remove Void Fire", new VoidFireStatus());
                s.IsRemoveStatusSkill = true;
                s.TargetType = TargetTypes.SINGLE_OPP;
                s.SetDescription("Remove the status Void Fire if an enemy has it.");
                return s;
            }
        }

        public static StatusSkill RemoveVoidIce
        {
            get
            {
                var s = MakeChangeElementSkill("Remove Void Ice", new VoidIceStatus());
                s.IsRemoveStatusSkill = true;
                s.TargetType = TargetTypes.SINGLE_OPP;
                s.SetDescription("Remove the status Void Ice if an enemy has it.");
                return s;
            }
        }

        public static StatusSkill RemovePoisonStun
        {
            get
            {
                List<Status> remove = new List<Status>() { new PoisonStatus(), new StunStatus() };
                var s = MakeMultiStatusSkill("Remove PoisStun", remove);
                s.IsRemoveStatusSkill = true;
                s.TargetType = TargetTypes.SINGLE_TEAM;
                s.EndupAnimation = SkillAssets.STATUS_RECOVER;
                s.Icon = SkillAssets.HEAL_ICON;
                s.SetDescription("Removes the Poison and Stun statuses from a single player.");
                return s;
            }
        }

        public static StatusSkill RemoveBindSeal
        {
            get
            {
                List<Status> remove = new List<Status>() { new BindStatus(), new SealStatus() };
                var s = MakeMultiStatusSkill("Remove BindSeal", remove);
                s.IsRemoveStatusSkill = true;
                s.TargetType = TargetTypes.SINGLE_TEAM;
                s.EndupAnimation = SkillAssets.STATUS_RECOVER;
                s.Icon = SkillAssets.HEAL_ICON;
                s.SetDescription("Removes the Bind and Seal statuses from a single player.");
                return s;
            }
        }

        public static StatusSkill RemovePoisonStunAll
        {
            get
            {
                List<Status> remove = new List<Status>() { new PoisonStatus(), new StunStatus() };
                var s = MakeMultiStatusSkill("Re-PoiStun All", remove);
                s.IsRemoveStatusSkill = true;
                s.TargetType = TargetTypes.TEAM_ALL;
                s.EndupAnimation = SkillAssets.STATUS_RECOVER;
                s.Icon = SkillAssets.HEAL_ICON;
                s.SetDescription("Removes the Poison and Stun statuses from all players.");
                return s;
            }
        }

        public static StatusSkill Bind
        {
            get
            {
                var bind = MakeStatusSkill("Bind", new BindStatus());
                bind.Icon = SkillAssets.BIND_ICON;
                bind.EndupAnimation = SkillAssets.BIND;
                return bind;
            }
        }

        public static StatusSkill Seal
        {
            get
            {
                var seal = MakeStatusSkill("Seal", new SealStatus());
                seal.Icon = SkillAssets.SEAL_ICON;
                seal.EndupAnimation = SkillAssets.SEAL;
                return seal;
            }
        }

        public static StatusSkill BindAll
        {
            get
            {
                var bind = MakeStatusSkill("Bind All", new BindStatus());
                bind.Icon = SkillAssets.BIND_ICON;
                bind.EndupAnimation = SkillAssets.BIND;
                bind.TargetType = TargetTypes.OPP_ALL;
                return bind;
            }
        }

        public static StatusSkill SealAll
        {
            get
            {
                var seal = MakeStatusSkill("Seal All", new SealStatus());
                seal.Icon = SkillAssets.SEAL_ICON;
                seal.EndupAnimation = SkillAssets.SEAL;
                seal.TargetType = TargetTypes.OPP_ALL;
                return seal;
            }
        }

        public static StatusSkill Poison
        {
            get
            {
                var poison = MakeStatusSkill("Poison", new PoisonStatus());
                poison.Icon = SkillAssets.POISON_ICON;
                poison.EndupAnimation = SkillAssets.POISON;
                return poison;
            }
        }

        public static StatusSkill PoisonAll
        {
            get
            {
                var poison = Poison;
                poison.TargetType = TargetTypes.OPP_ALL;
                poison.BaseName = "All Poison";
                return poison;
            }
        }

        public static StatusSkill BuffBoost
        {
            get
            {
                var status = MakeStatusSkill("Buff Boost", new BuffBoostStatus() { Stacks = 4 });
                status.Icon = SkillAssets.BUFF_BOOST_ICON;
                status.EndupAnimation = SkillAssets.ATK_BUFF;
                status.TargetType = TargetTypes.SELF;
                return status;
            }
        }

        public static StatusSkill VictoryCry
        {
            get
            {
                var vicCry = new BuffBoostStatus() { Stacks = 3 };
                vicCry.Name = "Victory Cry";
                vicCry.Icon = SkillAssets.VICTORY_CRY_ICON;

                var status = MakeStatusSkill("Victory Cry", vicCry);
                status.Icon = SkillAssets.VICTORY_CRY_ICON;
                status.EndupAnimation = SkillAssets.REVIVE;
                status.TargetType = TargetTypes.TEAM_ALL;
                status.SetDescription("Raises the maximum amount of buffs your team can receive from 2 to 3.");
                return status;
            }
        }

        public static StatusSkill DebuffBoost
        {
            get
            {
                var status = MakeStatusSkill("Debuff Boost", new DebuffBoostStatus());
                status.Icon = SkillAssets.DEBUFF_BOOST_ICON;
                status.EndupAnimation = SkillAssets.FLAT_DEBUFF;
                status.TargetType = TargetTypes.OPP_ALL;
                return status;
            }
        }

        private static StatusSkill MakeChangeElementSkill(string name, Status status)
        {
            StatusSkill statusSkill = MakeStatusSkill(name, status);

            statusSkill.Icon = status.Icon;
            statusSkill.EndupAnimation = SkillAssets.VOID_SHIELD;
            statusSkill.TargetType = TargetTypes.SINGLE_TEAM;

            return statusSkill;
        }

        private static StatusSkill MakeStatusSkill(string name, Status status)
        {
            return new StatusSkill
            {
                BaseName = name,
                TargetType = TargetTypes.SINGLE_OPP,
                StartupAnimation = SkillAssets.STARTUP1_MG,
                Status = status
            };
        }

        private static StatusSkill MakeMultiStatusSkill(string name, List<Status> statuses)
        {
            return new StatusSkill
            {
                BaseName = name,
                TargetType = TargetTypes.SINGLE_OPP,
                StartupAnimation = SkillAssets.STARTUP1_MG,
                Statuses = statuses
            };
        }

        public static HealSkill Spindlewarium { get => MakeHealSkill("Spindlewarium", 5, 1); } 

        public static HealSkill Heal1 { get => MakeHealSkill("Heal", 10, 1); } 
        public static HealSkill Revive1 
        { 
            get 
            { 
                var hs = MakeHealSkill("Revive", 5, 1, true);
                hs.EndupAnimation = SkillAssets.REVIVE;
                return hs;
            } 
        }

        public static HealSkill ReviveAll
        {
            get
            {
                var hs = MakeAllHeal("Revive All", 5, 1, true);
                hs.EndupAnimation = SkillAssets.REVIVE;
                hs.TargetType = TargetTypes.TEAM_ALL_DEAD;
                return hs;
            }
        }

        public static HealSkill Heal1All { get => MakeAllHeal("Allheal", 7, 1); }

        public static HealSkill Salvation 
        {
            get 
            {
                var allHeal = MakeAllHeal("Salvation", 7, 1);
                allHeal.RemoveStatusAilments = new List<Status>(
                    [
                        new StunStatus(),
                        new PoisonStatus(),
                        new BindStatus(),
                        new SealStatus(),
                        new WeakDarkStatus(),
                        new WeakElecStatus(),
                        new WeakFireStatus(),
                        new WeakIceStatus()
                    ]);

                return allHeal;
            }
        }

        private static HealSkill MakeAllHeal(string name, int amount, int tier, bool isRevive = false)
        {
            var hs = MakeHealSkill(name, amount, tier, isRevive);
            hs.TargetType = (!isRevive) ? TargetTypes.TEAM_ALL : TargetTypes.TEAM_ALL_DEAD;
            return hs;
        }

        private static HealSkill MakeHealSkill(string name, int amount, int tier, bool isRevive = false)
        {
            return new HealSkill()
            {
                BaseName = name,
                TargetType = (!isRevive) ? TargetTypes.SINGLE_TEAM : TargetTypes.SINGLE_TEAM_DEAD,
                StartupAnimation = SkillAssets.STARTUP1_MG,
                EndupAnimation = SkillAssets.HEAL_T1,
                Icon = SkillAssets.HEAL_ICON,
                HealAmount = amount,
                Tier = tier
            };
        }


        #region Temporary Battle Skills
        public static PassSkill Pass = new PassSkill()
        {
            BaseName = "Pass",
            TargetType = TargetTypes.SELF,
            StartupAnimation = string.Empty,
            EndupAnimation = string.Empty,
            Icon = SkillAssets.PASS_ICON
        };

        public static StatusSkill Guard
        {
            get
            {
                var guard = MakeStatusSkill("Guard", new GuardStatus());
                guard.Icon = SkillAssets.GUARD_ICON;
                guard.EndupAnimation = SkillAssets.VOID_SHIELD;
                guard.TargetType = TargetTypes.SELF;
                return guard;
            }
        }

        public static RetreatSkill Retreat = new RetreatSkill()
        {
            BaseName = "Retreat",
            TargetType = TargetTypes.SINGLE_OPP,
            StartupAnimation = string.Empty,
            EndupAnimation = string.Empty,
            Icon = SkillAssets.RETREAT_ICON
        };
        #endregion

        /// <summary>
        /// Get all skills that can be generated for a party member.
        /// </summary>
        /// <returns></returns>
        public static List<ISkill> GetAllGeneratableSkills(int tier)
        {
            List<ISkill> skills = new List<ISkill>();
            skills.AddRange([Fire1, Ice1, Wind1, Elec1, Light1, Dark1]);
            if (tier > TierRequirements.TIER15_STRONGER_ENEMIES)
            {
                skills.AddRange([PierceFire1, PierceIce1, PierceWind1, PierceElec1, PierceLight1, PierceDark1]);
            }

            if (tier > TierRequirements.TIER2_STRONGER_ENEMIES)
                skills.AddRange([VoidFire, VoidIce, VoidWind]);

            if (tier < TierRequirements.TIER5_STRONGER_ENEMIES)
            {
                skills.AddRange([RemoveVoidWind, RemoveVoidIce]);
                skills.Add(Heal1);
            }

            if (tier < TierRequirements.TIER15_STRONGER_ENEMIES)
                skills.Add(Revive1);

            if (tier > TierRequirements.TIER10_STRONGER_ENEMIES)
                skills.Add(VoidElec);

            if (tier < TierRequirements.TIER15_STRONGER_ENEMIES && tier > TierRequirements.TIER5_STRONGER_ENEMIES)
                skills.AddRange([Heal1All]);

            if (tier > TierRequirements.TIER8_STRONGER_ENEMIES && tier < TierRequirements.TIER10_STRONGER_ENEMIES)
                skills.AddRange([TechBuff, TechDebuff]);
            else if (tier >= TierRequirements.TIER10_STRONGER_ENEMIES)
                skills.AddRange([TechBuffAll, TechDebuff]);

            if (tier > TierRequirements.TIER6_STRONGER_ENEMIES && tier < TierRequirements.TIER9_STRONGER_ENEMIES)
                skills.AddRange([RemovePoisonStun]);
            else if (tier >= TierRequirements.TIER9_STRONGER_ENEMIES && tier < TierRequirements.TIER15_STRONGER_ENEMIES)
                skills.AddRange([RemovePoisonStunAll, RemoveBindSeal]);

            if (tier > TierRequirements.TIER10_STRONGER_ENEMIES)
            {
                if(tier > TierRequirements.TIER15_STRONGER_ENEMIES)
                {
                    skills.AddRange([HolyGrail, Torpefy, ReviveAll, Salvation]);
                }
                else
                {
                    skills.AddRange([LusterCandy, Debilitate, ReviveAll]);
                }
            }
            else
            {
                if (tier > TierRequirements.TIER6_STRONGER_ENEMIES)
                    skills.AddRange([AtkBuff, DefBuff]);

                if (tier > TierRequirements.TIER7_STRONGER_ENEMIES)
                    skills.AddRange([AtkDebuff, DefDebuff]);
            }

            if (tier > 260)
                skills.Add(VictoryCry);

            return skills;
        }
    }
}
