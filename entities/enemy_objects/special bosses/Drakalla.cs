using AscendedZ.battle.battle_state_machine;
using AscendedZ.battle;
using AscendedZ.resistances;
using AscendedZ.skills;
using System.Collections.Generic;
using AscendedZ.entities.battle_entities;
using AscendedZ.game_object;
using System.Linq;
using System;

namespace AscendedZ.entities.enemy_objects.special_bosses
{
    public class Drakalla : Enemy
    {
        private List<ISkill> _script, _startScript, _currentScript;
        private Dictionary<Elements, ISkill> _skillMap;
        private int _currentScriptIndex = 0;
        private ISkill _phase2Move, _phase3Move, _holyGrail;
        private bool _phase1Watched, _phase2Watched, _phase3Watched, _phase1, _phase2, _phase3;
        private HashSet<Elements> _usedElements = new();

        private Dictionary<Elements, ResistanceType> _wexMap = new() 
        {
            {Elements.Fire, ResistanceType.Dr },
            {Elements.Ice, ResistanceType.Dr },
            {Elements.Elec, ResistanceType.Dr },
            {Elements.Wind, ResistanceType.Dr},
            {Elements.Light, ResistanceType.Dr },
            {Elements.Dark, ResistanceType.Dr }
        };

        private Dictionary<Elements, int> _wexCountMap = new()
        {
            {Elements.Fire, 0 },
            {Elements.Ice, 0},
            {Elements.Elec, 0 },
            {Elements.Wind, 0},
            {Elements.Light, 0 },
            {Elements.Dark, 0}
        };

        private readonly string[] _phase1Dialog =
        [
            "100 years ago, I dedicated my life to\ncreating a world devoid of magic.",
            "Sorcerers could not be allowed to rise to power\nand dominate those who were not as gifted.",
            "The existence of the Ascended game\nposed a threat to that dream.",
            "Things got worse when I discovered that\nDraco became a contestant.",
            "I was given no choice.\nI had to put a stop to it.",
            "Short on time, Netalla and I went through\na set of castle gates similar to the ones\nyou entered...",
            "I had to beat the game before Draco, or\nelse the world as we knew it would end!",
            "And so, my journey began..."
        ];

        private readonly string[] _phase2Dialog =
        [
            "I had done it. I had reached\nthe final floor.",
            "The game was trivial for me.\nMy magic far exceeded the challenges\nthe game had to offer.",
            "But, before I could go on to\nface the final floor guardian...",
            "A revelation struck me. What was the\npurpose of all these supposed tests?",
            "The Ascended game I took part in was\nobsessed with deaming my worth, but why?",
            "It was then that I came to an\nanswer: The reality of the Elders'\ninfluence over the game.",
            "Across the multiverse, they were\ntoying with people like yourself with\nthis ridiculous quest for power.",
            "From then on, my purpose was set.",
            "I had to remove these malevolent\ngods from their thrones.",
            "In order to do so, I had to use\na similar tactic to yourself as my\nrun had to be frozen in time.",
            "That would enable me to prepare\nfor the battle ahead.",
            "100 years later, the game rebelled against\nmy magic, and contorted itself into\nthe game you participated in.",
            "I had no choice but to re-Ascend with\nNetalla's help to gain an audience\nwith the Elders who run the game...",
            "...and slay them once and for all!",
        ];

        private readonly string[] _phase3Dialog =
        [
            "The battle didn't last long.\nWith a swiftness, the Elders ended\nmy attempts at thwarting their game.",
            "I suppose Draco was right, I was too\nnaive to think I stood a chance\nagainst them.",
            "The game I was in, the one\nyou traversed to face off against\nthe very Elders who slayed me...",
            "It was frozen in time as a result\nof my attempt at violating a\nuniversal taboo.",
            "The remnant of my soul you came to know as\nFittotu was cast out of the game to be used in\nanother Ascended's journey.",
            "To add insult to injury, Draco tried\nto exploit my failure by going on a\nquest to join the ranks of the other Elders.",
            "And Netalla, she was forced to\nbecome the figure-head of your game.",
            "Of course, by that point, my plan\nhad already been set into motion.",
            "The Elders devised a game with the intent of\ngaining power so I used that to my advantage.",
            "I used the remnants of my magic\nto nurture a game that would create\na competitor worthy of ending the Elders.",
            "They're nothing but a bunch of bloodthirsty\ngods, so I knew they'd be eager to face\nsomeone who could rival their strength.",
            "And here we are now, facing off\nto the death at the world's end for\nthe greatest power in the universe.",
            "My mistake of restricting the world around\nme during my time ends with this\nfinal battle!",
            "Let us face each other until our last\nbreaths...",
            "So the victor may ASCEND!!!"
        ];


        public Drakalla() : base()
        {
            Name = EnemyNames.Drakalla;
            Image = CharacterImageAssets.GetImagePath(Name);
            MaxHP = 15000;
            Turns = 2;
            _isBoss = true;

            Resistances.SetResistance(ResistanceType.Dr, Elements.Fire);
            Resistances.SetResistance(ResistanceType.Dr, Elements.Ice);
            Resistances.SetResistance(ResistanceType.Dr, Elements.Wind);
            Resistances.SetResistance(ResistanceType.Dr, Elements.Elec);
            Resistances.SetResistance(ResistanceType.Dr, Elements.Dark);
            Resistances.SetResistance(ResistanceType.Dr, Elements.Light);

            var mgFireP = SkillDatabase.FireMadGodAllP;
            var mgIceP = SkillDatabase.IceMadGodAllP;
            var mgWindP = SkillDatabase.WindMadGodAllP;
            var mgElecP = SkillDatabase.ElecMadGodAllP;
            var mgDarkP = SkillDatabase.DarkMadGodAllP;
            var mgLightP = SkillDatabase.LightMadGodAllP;
            var almighty = SkillDatabase.Almighty;

            // phase 1
            var buffBoost = SkillDatabase.BuffBoost;
            var debuffBoost = SkillDatabase.DebuffBoost;

            // phase 2
            _phase2Move = SkillDatabase.Confusion;
            _phase3Move = SkillDatabase.Stun;
            _holyGrail = SkillDatabase.HolyGrail;
            _phase1 = true;
            // phase 3
            var ancientChoir = SkillDatabase.AncientChoir;
            var dragonEye = SkillDatabase.DragonEye;

            _script = [almighty, ancientChoir];
            _startScript = [dragonEye, buffBoost, debuffBoost, ancientChoir, ancientChoir, ancientChoir];

            _skillMap = new()
            {
                { Elements.Fire, mgFireP },
                { Elements.Ice, mgIceP },
                { Elements.Wind, mgWindP },
                { Elements.Elec, mgElecP },
                { Elements.Dark, mgDarkP },
                { Elements.Light, mgLightP },
            };

            Skills.AddRange(
            [
                mgFireP, mgIceP, mgWindP, mgElecP, mgLightP, mgDarkP, almighty,
                buffBoost, debuffBoost, _phase2Move, _phase3Move, _holyGrail, ancientChoir, dragonEye
            ]);

            _currentScript = _startScript;

            Description = $"{Name}: Think as much as you like. It will not change the outcome of this fight.";
        }

        public override BattleResult ApplyElementSkill(BattleEntity user, ElementSkill skill)
        {
            BattleResult result = base.ApplyElementSkill(user, skill);
            var res = Resistances.GetResistance(skill.Element);
            if(res == ResistanceType.Dr)
            {
                Resistances.SetResistance(ResistanceType.Nu, skill.Element);
            }
            else if (res == ResistanceType.Nu)
            {
                Resistances.SetResistance(ResistanceType.None, skill.Element);
            }
            else if(res == ResistanceType.None)
            {
                Elements opposite = SkillDatabase.ElementalOpposites[skill.Element];
                _script.Insert(0, _skillMap[opposite]);
                Turns++;
                Resistances.SetResistance(ResistanceType.Wk, skill.Element);
            }

            HandlePhaseSelect();

            return result;
        }

        public override EnemyAction GetNextAction(BattleSceneObject battleSceneObject)
        {
            var skill = _currentScript[_currentScriptIndex];
            var target = FindTarget(skill, battleSceneObject);

            EnemyAction action = new EnemyAction()
            {
                Skill = skill,
                Target = target
            };

            if (!_phase1Watched && _phase1)
            {
                _phase1Watched = true;
                PlayDialog?.Invoke(this, _phase1Dialog);
            }

            if (!_phase2Watched && _phase2)
            {
                _phase2Watched = true;
                PlayDialog?.Invoke(this, _phase2Dialog);
            }

            if (!_phase3Watched && _phase3)
            {
                _phase3Watched = true;
                PlayDialog?.Invoke(this, _phase3Dialog);
            }

            _currentScriptIndex++;
            if (_currentScriptIndex >= _currentScript.Count)
                _currentScriptIndex = 0;

            return action;
        }

        private BattleEntity FindTarget(ISkill skill, BattleSceneObject battleSceneObject)
        {
            BattleEntity target;
            if (skill.Id == SkillId.Elemental)
            {
                target = FindElementSkillTarget((ElementSkill)skill, battleSceneObject);
            }
            else
            {
                if (skill.Id == SkillId.Eye)
                {
                    target = this;
                }
                else
                {
                    target = FindTargetForStatus((StatusSkill)skill, battleSceneObject);
                }
            }

            return target;
        }

        public override void ResetEnemyState()
        {
            _currentScriptIndex = 0;
            if (_currentScript == _startScript)
                _currentScript = _script;
        }

        private bool _confusionAdded, _modAdded;
        private void HandlePhaseSelect()
        {
            int hpThreshold = MaxHP / 3;
            int hpThreshold2x = hpThreshold * 2;

            if (HP <= hpThreshold2x && HP > hpThreshold)
            {
                _phase1 = false;
                _phase2 = true;
                _phase3 = false;

                if (!_confusionAdded)
                {
                    _confusionAdded = true;
                    Turns++;
                    _currentScript.Insert(_currentScript.Count - 2, _phase2Move);
                }
            }
            //else if (HP <= hpThreshold)
            else if (HP <= hpThreshold)
            {
                _phase1 = false;
                _phase2 = false;
                _phase3 = true;

                if (!_modAdded)
                {
                    _modAdded = true;
                    Turns++;
                    _currentScript.Insert(_currentScript.Count - 2, _phase3Move);
                }
            }
            else
            {
                _phase1 = true;
                _phase2 = false;
                _phase3 = false;
            }
        }
    }
}
