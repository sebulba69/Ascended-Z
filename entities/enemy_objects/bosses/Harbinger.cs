using AscendedZ.battle;
using AscendedZ.battle.battle_state_machine;
using AscendedZ.entities.battle_entities;
using AscendedZ.resistances;
using AscendedZ.skills;
using AscendedZ.statuses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.entities.enemy_objects.bosses
{
    public class Harbinger : Enemy
    {
        private int _currentMove;
        private int _currentScript;

        private const int STUN = 3;

        private ISkill _pickedSkill;
        private BattlePlayer _stunnedPlayer;

        /// <summary>
        /// Script =
        /// WEX - ELEC, WEX - ICE, WEX - DARK, Stun
        /// WEX on Stun x2, WEX ON REST
        /// </summary>
        public Harbinger() : base()
        {
            _isBoss = true;

            Name = EnemyNames.Harbinger;

            MaxHP = EntityDatabase.GetBossHP(Name);
            Image = CharacterImageAssets.GetImagePath(Name);

            Resistances = new ResistanceArray();

            Resistances.SetResistance(ResistanceType.Wk, Elements.Wind);

            Skills.Add(SkillDatabase.Elec1.Clone()); // 0
            Skills.Add(SkillDatabase.Ice1.Clone()); // 1
            Skills.Add(SkillDatabase.Dark1.Clone()); // 2
            Skills.Add(SkillDatabase.Stun.Clone()); // 3

            _currentMove = 0;
            _currentScript = 0;
            Turns = 2;

            Description = $"{Name}: Will attempt to exploit party weaknesses using their skills (in the order they appear from top to bottom) to gain more Press Turns before using a Stun skill on the 4th turn.";
        }

        public override EnemyAction GetNextAction(BattleSceneObject battleSceneObject)
        {
            return new EnemyAction
            {
                Skill = GetNextMove(battleSceneObject),
                Target = GetNextTarget(battleSceneObject)
            };
        }

        public ISkill GetNextMove(BattleSceneObject battleSceneObject)
        {
            _pickedSkill = this.Skills[_rng.Next(STUN)];

            if(battleSceneObject.AlivePlayers.Count > 1)
            {
                if (_currentScript == 0)
                {
                    _pickedSkill = this.Skills[_currentMove];
                }
                else
                {
                    if (_currentMove < 2)
                    {
                        // at this point someone has the stun status
                        _stunnedPlayer = battleSceneObject.AlivePlayers.Find(p => p.StatusHandler.HasStatus(StatusId.StunStatus));
                        if (_stunnedPlayer != null)
                        {
                            for (int i = 0; i < STUN; i++)
                            {
                                var element = (ElementSkill)this.Skills[i];
                                if (_stunnedPlayer.Resistances.IsWeakToElement(element.Element))
                                {
                                    _pickedSkill = element;
                                    break;
                                }
                            }
                        }
                    }
                }
                _currentMove++;
            }

            return _pickedSkill;
        }

        /// <summary>
        /// Harbinger's target is decided when picking a skill
        /// </summary>
        /// <param name="gameState"></param>
        /// <returns></returns>
        public BattleEntity GetNextTarget(BattleSceneObject battleSceneObject)
        {
            var alivePlayers = battleSceneObject.AlivePlayers;
            if(_currentScript == 0)
            {
                if (_pickedSkill.Equals(this.Skills[STUN]))
                {
                    return alivePlayers[_rng.Next(alivePlayers.Count)];
                }
                else
                {
                    return GetWeaknessPlayerOrLowestHPPlayer(alivePlayers);
                }
            }
            else
            {
                return GetWeaknessPlayerOrLowestHPPlayer(alivePlayers);
            }
        }

        private BattlePlayer GetWeaknessPlayerOrLowestHPPlayer(List<BattlePlayer> alivePlayers)
        {
            var player = GetWeaknessForPickedSkill(alivePlayers);
            if (player == null)
                return GetPlayerWithLowestHP(alivePlayers);
            else
                return player;
        }

        private BattlePlayer GetWeaknessForPickedSkill(List<BattlePlayer> alivePlayers)
        {
            BattlePlayer player = null;
            if (_pickedSkill.GetType().Equals(typeof(ElementSkill)))
            {
                foreach(var alive in alivePlayers)
                {
                    var element = (ElementSkill)_pickedSkill;
                    if (alive.Resistances.IsWeakToElement(element.Element))
                    {
                        player = alive;
                        break;
                    }
                }
            }
            return player;
        }

        private BattlePlayer GetPlayerWithLowestHP(List<BattlePlayer> alivePlayers)
        {
            BattlePlayer lowestHP = alivePlayers[0];

            foreach (var p in alivePlayers)
            {
                if (p.HP < lowestHP.HP)
                    lowestHP = p;
            }
            return lowestHP;
        }

        public override void ResetEnemyState()
        {
            _currentMove = 0;
            _currentScript = 0;
            _pickedSkill = null;
            _stunnedPlayer = null;
        }
    }
}
