using AscendedZ.battle;
using AscendedZ.battle.battle_state_machine;
using AscendedZ.entities.battle_entities;
using AscendedZ.skills;
using AscendedZ.statuses;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AscendedZ.entities.enemy_objects.bosses
{
    public class AncientNodys : Enemy
    {
        private Dictionary<Elements, ISkill> _weaknessDictionary;
        private Dictionary<BattlePlayer, ISkill> _playerSkillMap;
        private bool _skillsMapped;

        private int _currentTarget;

        private ISkill _buff, _debuff;

        public AncientNodys() : base()
        {
            Name = EnemyNames.Ancient_Nodys;
            Image = CharacterImageAssets.GetImagePath(Name);
            MaxHP = EntityDatabase.GetBossHPDC(Name);

            Turns = 3;
            _isBoss = true;
            _skillsMapped = false;

            _currentTarget = 0;

            var fire = SkillDatabase.PierceFire1;
            var ice = SkillDatabase.PierceIce1;
            var elec = SkillDatabase.PierceElec1;
            var wind = SkillDatabase.PierceWind1;
            var dark = SkillDatabase.PierceDark1;
            var light = SkillDatabase.PierceLight1;

            _buff = SkillDatabase.HolyGrail;
            _debuff = SkillDatabase.Torpefy;

            _weaknessDictionary = new()
            {
                {Elements.Fire, fire},
                {Elements.Ice, ice},
                {Elements.Elec, elec},
                {Elements.Wind, wind},
                {Elements.Dark, dark},
                {Elements.Light, light}
            };

            _playerSkillMap = new();

            Skills.AddRange([fire, ice, elec, wind, dark, light]);

            Description = $"{Name}: Will always find the first alive player and specifically use their weakness against them if possible. There are 2 ways to overrite this behavior:\n\n1. If player buffs are greater than 0, then a debuff skill will be used.\n\n2. If {Name} is not at max buffs, it will try to max them out before attacking.";
        }

        public override List<ISkill> GetDisplaySkillList()
        {
            var list = base.GetDisplaySkillList();
            list.AddRange([_buff, _debuff]);

            return list;
        }

        public override EnemyAction GetNextAction(BattleSceneObject battleSceneObject)
        {
            if (!_skillsMapped)
            {
                List<Elements> elements = Enum.GetValues<Elements>().ToList();
                foreach (var p in battleSceneObject.AlivePlayers)
                {
                    foreach (var element in elements)
                    {
                        if (p.Resistances.IsWeakToElement(element))
                        {
                            _playerSkillMap.Add(p, _weaknessDictionary[element]);
                            break;
                        }
                    }
                }
                _skillsMapped = true;
            }

            EnemyAction action = new EnemyAction();
            BattlePlayer target = null;
            if (battleSceneObject.AlivePlayers.Count > 0)
            {
                while (battleSceneObject.Players[_currentTarget].HP == 0)
                {
                    _currentTarget++;
                    if (_currentTarget == battleSceneObject.Players.Count)
                        _currentTarget = 0;
                }

                target = battleSceneObject.Players[_currentTarget];
            }

            if (target != null)
            {
                action.Target = target;
                action.Skill = _playerSkillMap[target];
            }

            foreach (var p in battleSceneObject.AlivePlayers)
            {
                if (p.StatusHandler.GetStatus(StatusId.DefChangeStatus).GetStacks() > 0)
                {
                    action.Skill = _debuff;
                    break;
                }
            }

            if (action.Skill != _debuff)
            {
                // if we aren't buffed
                if (StatusHandler.GetStatus(StatusId.DefChangeStatus).GetStacks() <= 0)
                {
                    action.Skill = _buff;
                    action.Target = this;
                }
            }

            _currentTarget++;
            if (_currentTarget == battleSceneObject.Players.Count)
                _currentTarget = 0;

            return action;
        }

        public override void ResetEnemyState()
        {
        }
    }
}