using AscendedZ.battle.battle_state_machine;
using AscendedZ.battle;
using AscendedZ.resistances;
using AscendedZ.skills;
using System.Collections.Generic;
using AscendedZ.entities.battle_entities;
using AscendedZ.game_object;

namespace AscendedZ.entities.enemy_objects.special_bosses
{
    public class Nettala : Enemy
    {
        private readonly Elements[] _elements = [Elements.Fire, Elements.Ice, Elements.Wind, Elements.Elec, Elements.Dark, Elements.Light];
        private readonly Dictionary<Elements, List<ISkill>> _elementalScripts;

        private Elements _currentElement;
        private int _currentMainElementIndex;
        private int _currentScriptIndex;

        private List<ISkill> _wipeScript;
        private List<ISkill> _currentScript;
        private bool _swapDialogPlayed = false;
        private bool _wipeDialogPlayed = false;

        private readonly string[] _preWipeDialog = 
        [
            "Ha ha ha ha haaaa!",
            "It seems as though you've failed\nto grasp the rules of this game!",
            "In accordance with your failure,\nI will put an end to your misguided\nattempts at defeating me!"
        ];

        private readonly string[] _preSwapDialog = 
        [
            "I have witnessed the rise and fall of\nmany promising candidates for the Ascension...",
            $"But you, {PersistentGameObjects.GameObjectInstance().MainPlayer.Name}, have\ngone beyond my expectations!",
            "In honor of your effort, I will set\nthe stage to unleash your full potential!",
        ];

        public Nettala() : base()
        {
            Name = EnemyNames.Nettala;
            Image = CharacterImageAssets.GetImagePath(Name);
            MaxHP = 5500;
            Turns = 5;
            _isBoss = true;

            _currentElement = Elements.Fire;
            _currentMainElementIndex = 0;
            _currentScriptIndex = 0;
            _elementalScripts = new Dictionary<Elements, List<ISkill>>();

            var holyGrail = SkillDatabase.HolyGrail;
            var torpefy = SkillDatabase.Torpefy;
            var wipeAlmighty = SkillDatabase.Almighty;

            _wipeScript = [wipeAlmighty, wipeAlmighty, wipeAlmighty, wipeAlmighty, SkillDatabase.DragonEye, wipeAlmighty, wipeAlmighty, wipeAlmighty, wipeAlmighty];

            // fire
            var mgFire = SkillDatabase.FireMadGod;
            var pFire = SkillDatabase.PierceFire1;
            var fireA = SkillDatabase.FireAll;
            var fire1 = SkillDatabase.Fire1;

            // ice
            var mgIce = SkillDatabase.IceMadGod;
            var pIce = SkillDatabase.PierceIce1;
            var iceA = SkillDatabase.IceAll;
            var ice1 = SkillDatabase.Ice1;

            // wind
            var mgWind = SkillDatabase.WindMadGod;
            var pWind = SkillDatabase.PierceWind1;
            var windA = SkillDatabase.WindAll;
            var wind1 = SkillDatabase.Wind1;

            // elec
            var mgElec = SkillDatabase.ElecMadGod;
            var pElec = SkillDatabase.PierceElec1;
            var elecA = SkillDatabase.ElecAll;
            var elec1 = SkillDatabase.Elec1;

            // dark
            var mgDark = SkillDatabase.DarkMadGod;
            var pDark = SkillDatabase.PierceDark1;
            var darkA = SkillDatabase.DarkAll;
            var dark1 = SkillDatabase.Dark1;

            // light
            var mgLight = SkillDatabase.LightMadGod;
            var pLight = SkillDatabase.PierceLight1;
            var lightA = SkillDatabase.LightAll;
            var light1 = SkillDatabase.Light1;

            Skills.AddRange(
            [
                mgFire, mgIce, mgWind, mgElec, mgLight, mgDark,
                pFire, pIce, pWind, pElec, pDark, pLight,
                fire1, ice1, wind1, elec1, dark1, light1, wipeAlmighty,
                fireA, iceA, windA, elecA, darkA, lightA
            ]);

            _elementalScripts.Add(Elements.Fire, [ice1, pIce, mgIce, iceA, torpefy]);
            _elementalScripts.Add(Elements.Ice,  [fire1, pFire, mgFire, fireA, torpefy]);
            _elementalScripts.Add(Elements.Wind, [elec1, pElec, mgElec, elecA, holyGrail]);
            _elementalScripts.Add(Elements.Elec, [wind1, pWind, mgWind, windA, holyGrail]);
            _elementalScripts.Add(Elements.Dark, [light1, pLight, mgLight, lightA, torpefy]);
            _elementalScripts.Add(Elements.Light,[dark1, pDark, mgDark, darkA, holyGrail]);

            _currentScript = _wipeScript;

            SetElementalResistances();
            Resistances.HideResistances = false;

            Description = $"{Name}: Will use elemental skills in the following order {string.Join(", ", _elements)}. You must hit its weakness every turn or else you die.";
        }

        public override BattleResult ApplyElementSkill(BattleEntity user, ElementSkill skill)
        {
            BattleResult result = base.ApplyElementSkill(user, skill);

            if (result.ResultType == BattleResultType.Wk || result.ResultType == BattleResultType.TechWk)
            {
                if(_currentScript == _wipeScript)
                {
                    _currentScript = _elementalScripts[_currentElement];
                }
            }

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

            CheckHPThreshold();

            if (!_wipeDialogPlayed && _currentScript == _wipeScript)
            {
                _wipeDialogPlayed = true;
                PlayDialog?.Invoke(this, _preWipeDialog);
            }

            _currentScriptIndex++;
            if (_currentScriptIndex >= _currentScript.Count)
                _currentScriptIndex = _currentScript.Count - 2;

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

        private void SetElementalResistances()
        {
            Resistances.ClearResistances();

            foreach(var element in _elements)
            {
                Resistances.SetResistance(ResistanceType.Dr, element);
            }

            Resistances.SetResistance(ResistanceType.Wk, _currentElement);
        }

        private void CheckHPThreshold()
        {
            int hpThreshold = MaxHP / 2;

            if (HP <= hpThreshold)
            {
                // hide resistances
                if (!_swapDialogPlayed)
                {
                    PlayDialog?.Invoke(this, _preSwapDialog);
                    _swapDialogPlayed = true;
                }

                Resistances.HideResistances = true;
            }
            else
            {
                Resistances.HideResistances = false;
            }
        }

        public override void ResetEnemyState()
        {
            _currentScriptIndex = 0;
            _currentMainElementIndex++;

            if (_currentMainElementIndex >= _elements.Length)
                _currentMainElementIndex = 0;

            _currentScript = _wipeScript;
            _wipeDialogPlayed = false;
            _currentElement = _elements[_currentMainElementIndex];

            SetElementalResistances();
        }
    }
}
