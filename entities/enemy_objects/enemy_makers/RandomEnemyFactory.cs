using AscendedZ.entities.enemy_objects.enemy_ais;
using AscendedZ.resistances;
using AscendedZ.skills;
using AscendedZ.statuses.weak_element;
using AscendedZ.statuses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Godot;
using AscendedZ.game_object;

namespace AscendedZ.entities.enemy_objects.enemy_makers
{
    public class RandomEnemyFactory : EnemyFactory
    {
        private Random _rng;
        private List<ElementSkill> _elementalSkills;
        private List<StatusSkill> _miscStatuses;
        private List<ISkill> _voidSkills;
        private List<string> _names;
        private List<Elements> _elements;
        private List<ResistanceType> _resistances;
        private List<string> _bossNames;

        private bool _addedA, _addedB, _addedC, _addedD, _addedE;

        public RandomEnemyFactory() : base()
        {
            _elementalSkills = new List<ElementSkill>() 
            {
                SkillDatabase.Fire1, SkillDatabase.Ice1, SkillDatabase.Wind1, SkillDatabase.Elec1, SkillDatabase.Dark1, SkillDatabase.Light1,
                SkillDatabase.FireAll, SkillDatabase.IceAll, SkillDatabase.WindAll, SkillDatabase.ElecAll, SkillDatabase.DarkAll, SkillDatabase.LightAll
            };

            _miscStatuses = new List<StatusSkill>() 
            {
                SkillDatabase.Poison, SkillDatabase.Stun, SkillDatabase.AtkBuff, SkillDatabase.DefBuff, SkillDatabase.DefDebuff, SkillDatabase.AtkDebuff,
                SkillDatabase.PoisonAll, SkillDatabase.TechBuffAll
            };

            _voidSkills = new List<ISkill>() 
            {
                SkillDatabase.VoidDark, SkillDatabase.VoidLight, SkillDatabase.VoidFire,
                SkillDatabase.VoidIce, SkillDatabase.VoidWind, SkillDatabase.VoidElec
            };

            _names = new List<string>() 
            {
                EnemyNames.Ansung, EnemyNames.Ardeb, EnemyNames.ChAffar, EnemyNames.Charcas, EnemyNames.DrigaBoli,
                EnemyNames.Ethel, EnemyNames.FoameShorti, EnemyNames.Keri, EnemyNames.Lyelof, EnemyNames.Nanles,
                EnemyNames.ReeshiDeeme, EnemyNames.Samjaris, EnemyNames.Tily, EnemyNames.Hahere, EnemyNames.Brast,
                EnemyNames.Locfridegel, EnemyNames.Garo, EnemyNames.Casgifu, EnemyNames.Kryonii, EnemyNames.Paron,
                EnemyNames.Geortom, EnemyNames.Ordtheod
            };

            _bossNames = new List<string>() 
            {
                EnemyNames.Algrools, EnemyNames.Gos, EnemyNames.Laltujass, EnemyNames.Pool, EnemyNames.Qibrel,
                EnemyNames.Sirgopes, EnemyNames.Suamgu, EnemyNames.Vrasrohd, EnemyNames.Vrosh, EnemyNames.Zaaxtrul,EnemyNames.Udeon,
                EnemyNames.Dremphannyal, EnemyNames.Mugogon, EnemyNames.Nainlifael
            };

            _elements = new List<Elements>() 
            {
                Elements.Fire, Elements.Ice, Elements.Wind, Elements.Elec, Elements.Dark, Elements.Light
            };

            _resistances = new List<ResistanceType>() { ResistanceType.Rs, ResistanceType.Nu, ResistanceType.Dr };

            _rng = new Random();

            foreach(string name in _names)
            {
                _functionDictionary.Add(name, GenerateEnemy);
            }
        }

        public override void SetTier(int tier)
        {
            base.SetTier(tier);
            SetSkillsForTier(tier);
        }

        public Enemy GenerateEnemy()
        {
            int aiMax = 11;
            string name = _names[_rng.Next(_names.Count)];
            int hp = _rng.Next(10 + _tierBoost, 20 + _tierBoost);

            int ai = _rng.Next(aiMax);
            Enemy enemy;

            var skills = new List<ISkill>();

            if (ai == 0)
            {
                var swapElement = GetRandomElement(_rng);
                var opposite = SkillDatabase.ElementalOpposites[swapElement];

                enemy = MakeResistanceChangerEnemy(name, hp, swapElement, opposite);

                skills.AddRange(_elementalSkills.FindAll(e => e.Element == swapElement));
                skills.AddRange(_elementalSkills.FindAll(e => e.Element == opposite));
            }
            else if (ai == 1)
            {
                enemy = MakeWeaknessHunterEnemy(name, hp);
                PopulateEnemyResistanceRandom(_rng, enemy);
                PopulateEnemySkillsRandom(_rng, enemy);
            }
            else if (ai == 2) 
            {
                enemy = MakeSupportEnemy(name, hp);
               
                PopulateEnemyResistanceRandom(_rng, enemy);
                PopulateEnemySkillsRandom(_rng, enemy);
            }
            else if (ai == 3)
            {
                var wexElement = GetRandomElement(_rng);
                enemy = (_rng.Next(1) == 0) ? MakeEyeEnemy(name, hp, SkillDatabase.BeastEye) : MakeEyeEnemy(name, hp, SkillDatabase.DragonEye);
                PopulateEnemyResistanceRandom(_rng, enemy);

                enemy.Resistances.SetResistance(ResistanceType.Wk, wexElement);

                PopulateEnemySkillsRandom(_rng, enemy);
            }
            else if (ai == 4)
            {
                enemy = MakeCopyCatEnemy(name, hp);
                PopulateEnemyResistanceRandom(_rng, enemy);
                PopulateEnemySkillsRandom(_rng, enemy);
            }
            else if(ai == 5)
            {
                var voidElement = GetRandomElement(_rng);
                enemy = MakeProtectorEnemy(name, hp, voidElement);
                var voidSkill = _voidSkills.Find(v => v.BaseName.Contains(voidElement.ToString())).Clone();
                enemy.Skills.Add(voidSkill);
                PopulateEnemyResistanceRandom(_rng, enemy);
                enemy.Resistances.SetResistance(ResistanceType.Wk, voidElement);
                PopulateEnemySkillsRandom(_rng, enemy);
            }
            else if (ai == 6)
            {
                enemy = MakeAgroStatusEnemy(name, hp);
                PopulateEnemyResistanceRandom(_rng, enemy);
                enemy.Skills.Add(_elementalSkills[_rng.Next(_elementalSkills.Count)].Clone());
            }
            else if (ai == 7)
            {
                enemy = MakeStunStatusEnemy(name, hp);
                PopulateEnemyResistanceRandom(_rng, enemy);
                enemy.Skills.Add(_elementalSkills[_rng.Next(_elementalSkills.Count)].Clone());
            }
            else if (ai == 8)
            {
                enemy = MakePoisonEnemy(name, hp);
                PopulateEnemyResistanceRandom(_rng, enemy);
                enemy.Skills.Add(_elementalSkills[_rng.Next(_elementalSkills.Count)].Clone());
            }
            else if (ai == 9)
            {
                enemy = (_rng.Next(1) == 0) ? MakeEvilEyeEnemy(name, hp, SkillDatabase.BeastEye) : MakeEvilEyeEnemy(name, hp, SkillDatabase.DragonEye);
                PopulateEnemyResistanceRandom(_rng, enemy);
                enemy.Skills.Add(_elementalSkills[_rng.Next(_elementalSkills.Count)].Clone());
            }
            else
            {
                enemy = MakeAlternatingEnemy(name, hp);
                PopulateEnemyResistanceRandom(_rng, enemy);
                PopulateEnemySkillsRandom(_rng, enemy);
            }

            enemy.MaxHP += 3;

            foreach (var skill in skills)
                enemy.Skills.Add(skill.Clone());

            enemy.RandomEnemy = true;

            return enemy;
        }

        public Enemy GenerateBoss(int tier)
        {
            var go = PersistentGameObjects.GameObjectInstance();

            int index = go.RandomBossIndex;
            string name = _bossNames[index];

            go.RandomBossIndex++;
            if (go.RandomBossIndex >= _bossNames.Count)
                go.RandomBossIndex = 0;

            int turns = _rng.Next(2, 4);

            var bhai = MakeBossHellAI(name, turns);
            bhai.MaxHP = EntityDatabase.GetBossHPRandom(tier);

            int res = _rng.Next(2) + 1;
            List<Elements> wex = new List<Elements>();
            for (int r = 0; r < res; r++)
            {
                Elements element = GetRandomElement(_rng);
                ResistanceType type = GetRandomResistanceType(_rng);
                bhai.Resistances.SetResistance(type, element);
            }

            Elements wexElement = GetRandomElement(_rng);
            bhai.Resistances.SetResistance(ResistanceType.Wk, wexElement);
            wex.Add(wexElement);

            int addBeastEye = _rng.Next(1, 101);
            if (addBeastEye <= 45)
            {
                bhai.Resistances.SetResistance(ResistanceType.Wk, GetRandomElement(_rng));
                if (_rng.Next(2) == 0)
                {
                    bhai.Skills.Add(SkillDatabase.BeastEye);
                }
                else
                {
                    bhai.Skills.Add(SkillDatabase.DragonEye);
                }
            }

            foreach (var voidElement in wex)
            {
                int voidWex = _rng.Next(1, 101);
                if (voidWex <= 65)
                    bhai.Skills.Add(_voidSkills.Find(v => v.BaseName.Contains(voidElement.ToString())).Clone());
            }

            int addOtherAilments = _rng.Next(1, 101);
            if (addOtherAilments <= 55 && tier >= 50)
            {
                bhai.Skills.Add(_miscStatuses[_rng.Next(_miscStatuses.Count)].Clone());
            }

            int skills;
            if(turns % 2 == 0)
            {
                skills = 5;
            }
            else
            {
                skills = 6;
            }

            PopulateEnemySkillsRandom(_rng, bhai, skills);
            return bhai;
        }

        private void SetSkillsForTier(int tier)
        {
            if (tier > 125 && !_addedA)
            {
                _elementalSkills.Add(SkillDatabase.Almighty);
                _miscStatuses.Add(SkillDatabase.Bind);
                _miscStatuses.Add(SkillDatabase.Seal);

                _addedA = true;
            }

            if (tier > 150 && !_addedB)
            {
                _elementalSkills.Add(SkillDatabase.Antitichton);
                _elementalSkills.Add(SkillDatabase.Almighty1);
                _elementalSkills.Add(SkillDatabase.Eliricpaul);
                _elementalSkills.Add(SkillDatabase.ZephyrShield);
                _elementalSkills.Add(SkillDatabase.DeadlyWind);
                _elementalSkills.Add(SkillDatabase.Hellfire);
                _elementalSkills.Add(SkillDatabase.Sarawaldbet);
                _elementalSkills.Add(SkillDatabase.Fredmuelald);
                _elementalSkills.Add(SkillDatabase.Curse);
                _elementalSkills.Add(SkillDatabase.OblivionsEmbrace);
                _elementalSkills.Add(SkillDatabase.ArcFlash);
                _miscStatuses.Add(SkillDatabase.LusterCandy);
                _miscStatuses.Add(SkillDatabase.Debilitate);

                _addedB = true;
            }

            if (tier > 200 && !_addedC)
            {
                _miscStatuses.Add(SkillDatabase.BindAll);
                _miscStatuses.Add(SkillDatabase.SealAll);

                _addedC = true;
            }

            if (tier > 225 && !_addedD)
            {
                _elementalSkills.Add(SkillDatabase.DarkMadGod);
                _elementalSkills.Add(SkillDatabase.IceMadGod);
                _elementalSkills.Add(SkillDatabase.FireMadGod);
                _elementalSkills.Add(SkillDatabase.WindMadGod);
                _elementalSkills.Add(SkillDatabase.LightMadGod);
                _elementalSkills.Add(SkillDatabase.ElecMadGod);
                _miscStatuses.Add(SkillDatabase.Torpefy);
                _miscStatuses.Add(SkillDatabase.HolyGrail);

                _addedD = true;
            }

            if(tier > 250 && !_addedE)
            {
                _elementalSkills.Add(SkillDatabase.PierceDark1);
                _elementalSkills.Add(SkillDatabase.PierceElec1);
                _elementalSkills.Add(SkillDatabase.PierceFire1);
                _elementalSkills.Add(SkillDatabase.PierceWind1);
                _elementalSkills.Add(SkillDatabase.PierceIce1);
                _elementalSkills.Add(SkillDatabase.PierceLight1);
                _elementalSkills.Add(SkillDatabase.LightAllP);
                _elementalSkills.Add(SkillDatabase.DarkAllP);
                _elementalSkills.Add(SkillDatabase.IceAllP);
                _elementalSkills.Add(SkillDatabase.FireAllP);
                _elementalSkills.Add(SkillDatabase.WindAllP);
                _elementalSkills.Add(SkillDatabase.ElecAllP);
                _elementalSkills.Add(SkillDatabase.ElecMadGodAll);
                _elementalSkills.Add(SkillDatabase.FireMadGodAll);
                _elementalSkills.Add(SkillDatabase.IceMadGodAll);
                _elementalSkills.Add(SkillDatabase.WindMadGodAll);
                _miscStatuses.Add(SkillDatabase.AncientChoir);

                _addedE = true;
            }
        }

        private readonly Dictionary<string, Elements> _tikkis = new()
        {
                { EnemyNames.FireTikki, Elements.Fire },
                { EnemyNames.IceTikki, Elements.Ice },
                { EnemyNames.ElecTikki, Elements.Elec },
                { EnemyNames.WindTikki, Elements.Wind },
                { EnemyNames.DarkTikki, Elements.Dark },
                { EnemyNames.LightTikki, Elements.Light }
        };
        
        public Enemy GenerateTikkiBoss(string tikkiName, int tier)
        {
            int turns = 2;
            int skills = 2;
            ResistanceType primary = ResistanceType.Rs;

            if (_addedB)
            {
                skills = 3;
            }

            if(_addedD)
            {
                turns = 3;
                skills = 4;
                primary = ResistanceType.Nu;
            }

            if (_addedE)
            {
                turns = 3;
                skills = 5;
                primary = ResistanceType.Dr;
            }

            var tikki = MakeBossHellAI(tikkiName, turns);
            tikki.MaxHP = EntityDatabase.GetBossHPRandom(tier);
            int hp = tikki.MaxHP / 3;
            tikki.MaxHP -= hp;

            tikki.Resistances.SetResistance(primary, _tikkis[tikkiName]);
            tikki.Resistances.SetResistance(ResistanceType.Wk, SkillDatabase.ElementalOpposites[_tikkis[tikkiName]]);

            var elemental = _elementalSkills.FindAll(e => e.Element == _tikkis[tikkiName] || e.Element == Elements.Almighty);
            tikki.Skills.Add(elemental[_rng.Next(elemental.Count)].Clone());
            for (int i = 0; i < skills; i++)
            {
                if(_rng.Next(100) < 75)
                    tikki.Skills.Add(elemental[_rng.Next(elemental.Count)].Clone());
                else
                    tikki.Skills.Add(_miscStatuses[_rng.Next(_miscStatuses.Count)].Clone());
            }

            return tikki;
        }

        private void PopulateEnemyResistanceRandom(Random rng, Enemy enemy)
        {
            int res = rng.Next(2) + 1;
            for (int r = 0; r < res; r++)
            {
                Elements element = GetRandomElement(rng);
                ResistanceType type = GetRandomResistanceType(rng);
                enemy.Resistances.SetResistance(type, element);
            }
        }

        private void PopulateEnemySkillsRandom(Random rng, Enemy enemy, int num = 0)
        {
            int smax = rng.Next(2, 4);
            if (num > 0)
                smax = num;

            for (int s = 0; s < smax + 1; s++)
            {
                enemy.Skills.Add(_elementalSkills[rng.Next(_elementalSkills.Count)].Clone());
            }
        }

        private ResistanceType GetRandomResistanceType(Random rng)
        {
            return _resistances[rng.Next(_resistances.Count)];
        }

        private Elements GetRandomElement(Random rng)
        {
            return _elements[rng.Next(_elements.Count)];
        }

        private Enemy MakeAlternatingEnemy(string name, int hp)
        {
            return new AlternatingEnemy
            {
                Name = $"[AE] {name}",
                MaxHP = hp + _tierBoost,
                Image = CharacterImageAssets.GetImagePath(name),
                Resistances = new ResistanceArray()
            };
        }


        private Enemy MakeStunStatusEnemy(string name, int hp)
        {
            var statusAttackEnemy = MakeStatusAttackEnemy(name, hp);

            statusAttackEnemy.Name = $"[STN] {statusAttackEnemy.Name}";
            statusAttackEnemy.Status = new StunStatus();
            statusAttackEnemy.Skills.Add(SkillDatabase.Stun.Clone());
            statusAttackEnemy.Description = $"[STN]: {statusAttackEnemy.Description}";

            return statusAttackEnemy;
        }

        private Enemy MakePoisonEnemy(string name, int hp)
        {
            var statusAttackEnemy = MakeStatusAttackEnemy(name, hp);

            statusAttackEnemy.Name = $"[PSN] {statusAttackEnemy.Name}";
            statusAttackEnemy.Status = new PoisonStatus();
            if(_rng.Next(1) == 0)
                statusAttackEnemy.Skills.Add(SkillDatabase.Poison.Clone());
            else
                statusAttackEnemy.Skills.Add(SkillDatabase.PoisonAll.Clone());

            statusAttackEnemy.Description = $"[PSN]: {statusAttackEnemy.Description}";

            return statusAttackEnemy;
        }

        private Enemy MakeEyeEnemy(string name, int hp, EyeSkill eyeSkill)
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

        private Enemy MakeEvilEyeEnemy(string name, int hp, EyeSkill eyeSkill)
        {
            return new EvilEyeEnemy
            {
                Name = $"[EEYE] {name}",
                MaxHP = hp + _tierBoost,
                Image = CharacterImageAssets.GetImagePath(name),
                Resistances = new ResistanceArray(),
                EyeSkill = eyeSkill
            };
        }

        private Enemy MakeSupportEnemy(string name, int hp)
        {
            var support = new SupportEnemy
            {
                Name = $"[SPRT] {name}",
                MaxHP = hp + _tierBoost,
                Image = CharacterImageAssets.GetImagePath(name),
                Resistances = new ResistanceArray()
            };

            support.LevelUpCompensation(((_tierBoost / 3) * 10) / 2);

            return support;
        }

        private Enemy MakeCopyCatEnemy(string name, int hp)
        {
            return new CopyCatEnemy
            {
                Name = $"[CC] {name}",
                MaxHP = hp + _tierBoost,
                Image = CharacterImageAssets.GetImagePath(name),
                Resistances = new ResistanceArray()
            };
        }

        private Enemy MakeProtectorEnemy(string name, int hp, Elements elementToVoid)
        {
            var protector = new ProtectorEnemy()
            {
                Name = $"[PRCT] {name}",
                MaxHP = hp + _tierBoost,
                Image = CharacterImageAssets.GetImagePath(name),
                Resistances = new ResistanceArray(),
                ElementToVoid = elementToVoid
            };

            return protector;
        }

        private Enemy MakeResistanceChangerEnemy(string name, int hp, Elements resist1, Elements resist2)
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

        private Enemy MakeWeaknessHunterEnemy(string name, int hp)
        {
            return new WeaknessHunterEnemy
            {
                Name = $"[WEX] {name}",
                MaxHP = hp + _tierBoost,
                Image = CharacterImageAssets.GetImagePath(name),
                Resistances = new ResistanceArray()
            };
        }

        private Enemy MakeAgroStatusEnemy(string name, int hp)
        {
            var statusAttackEnemy = MakeStatusAttackEnemy(name, hp);

            statusAttackEnemy.Name = $"[AGRO] {statusAttackEnemy.Name}";
            statusAttackEnemy.Status = new AgroStatus();
            statusAttackEnemy.Skills.Add(SkillDatabase.AgroEnemy.Clone());
            statusAttackEnemy.Description = $"[AGRO]: {statusAttackEnemy.Description}";

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

        private BossHellAI MakeBossHellAI(string name, int turns)
        {
            var bhai = new BossHellAI()
            {
                Name = name,
                Image = CharacterImageAssets.GetImagePath(name),
                Resistances = new ResistanceArray(),
                Turns = turns
            };

            bhai.Description = $"{bhai.Name}: Will iterate through each of its skills in the order they appear (from top to bottom) throughout the fight. " +
                $"Will always prioritize hitting player weaknesses if possible and avoiding their attacks getting nulled. " +
                $"If they have a Void Skill or an Eye Skill, they will only use it if their weakness is hit (once for the eye skill, 3 times for a void skill).";

            return bhai;
        }
    }
}
