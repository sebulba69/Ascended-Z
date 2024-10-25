using AscendedZ.battle;
using AscendedZ.entities.enemy_objects;
using AscendedZ.entities.enemy_objects.enemy_ais;
using AscendedZ.resistances;
using AscendedZ.skills;
using AscendedZ.statuses;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace AscendedZ.entities.battle_entities
{
    public enum EntityType
    {
        Player, Enemy
    }

    /// <summary>
    /// This class exists exclusively in the Battle Scene and will get disposed of
    /// when the battle ends.
    /// </summary>
    public class BattleEntity
    {
        private int _maxHP;
        private int _hp;
        private bool _isActiveEntity = false;
        public int Turns { get; set; }

        public EntityType Type { get; protected set; }

        public int HP
        {
            get => _hp;
            set
            {
                _hp = value;
                if (_hp < 0)
                {
                    _hp = 0;
                }

                if (_hp > MaxHP)
                    _hp = MaxHP;
            }
        }
        public int MaxHP
        {
            get => _maxHP;
            set
            {
                _maxHP = value;
                HP = _maxHP;
            }
        }
        public double DefenseModifier { get; set; }
        public string Name { get; set; }
        public string BaseName { get; set; }
        public string Image { get; set; }
        public bool CanAttack { set; get; }
        public List<ISkill> Skills { get; set; } = new();
        public ResistanceArray Resistances { get; set; } = new();
        public BattleEntityStatuses StatusHandler { get; set; } = new();
        public bool IsActiveEntity { get => _isActiveEntity; set => _isActiveEntity = value; }

        public double[] ElementDamageModifiers { get; set; }

        public BattleEntity()
        {
            this.CanAttack = true;
            ElementDamageModifiers = new double[Enum.GetNames(typeof(Elements)).Length];
            for(int i = 0; i < ElementDamageModifiers.Length; i++)
                ElementDamageModifiers[i] = 0;
        }

        public virtual BattleResult ApplyElementSkill(BattleEntity user, ElementSkill skill)
        {
            int damage = skill.Damage;
            damage = (int)(damage + (damage * user.ElementDamageModifiers[(int)skill.Element]));
            damage = (int)(damage - (damage * DefenseModifier));

            var confusion = user.StatusHandler.GetStatus(StatusId.Confusion);
            var technical = user.StatusHandler.GetStatus(StatusId.TechnicalStatus);
            var focus = user.StatusHandler.GetStatus(StatusId.FocusStatus);

            BattleResult result = new BattleResult()
            {
                HPChanged = damage,
                Target = this,
                SkillUsed = skill
            };

            if(focus != null)
            {
                damage = damage + (int)(skill.Damage * 0.5);
            }

            if (confusion != null)
            {
                result.HPChanged = 0;
                result.ResultType = BattleResultType.Evade;
            }
            else if (this.Resistances.IsDrainElement(skill.Element))
            {
                this.HP += damage;
                result.ResultType = BattleResultType.Dr;
            }
            else if (this.Resistances.IsNullElement(skill.Element) && !skill.Piercing)
            {
                result.HPChanged = 0;
                result.ResultType = BattleResultType.Nu;
            }
            else if (StatusHandler.HasStatus(StatusId.GuardStatus))
            {
                damage = (int)(damage * 0.75);
                this.HP -= damage;

                result.HPChanged = damage;
                result.ResultType = BattleResultType.Guarded;
            }
            else if (this.Resistances.IsResistantToElement(skill.Element) && !skill.ForceWex && !skill.Piercing)
            {
                if (technical != null && technical.Active)
                {
                    damage += (int)(damage * 0.5);
                    result.ResultType = BattleResultType.Tech;
                }
                else
                {
                    damage = (int)(damage * 0.75);
                    result.ResultType = BattleResultType.Rs;
                }
                this.HP -= damage;
                result.HPChanged = damage;
            }
            else if (this.Resistances.IsWeakToElement(skill.Element) || skill.ForceWex)
            {
                damage = damage + (int)(skill.Damage * 0.5);
                if(technical != null && technical.Active)
                {
                    damage += (int)(damage * 0.25);
                    result.ResultType = BattleResultType.TechWk;
                }
                else
                {
                    result.ResultType = BattleResultType.Wk;
                }

                this.HP -= damage;
                result.HPChanged = damage;
            }
            else
            {
                if (technical != null && technical.Active)
                {
                    damage += (int)(damage * 0.5);
                    result.ResultType = BattleResultType.Tech;
                }
                else
                {
                    result.ResultType = BattleResultType.Normal;
                } 
                this.HP -= damage;
                result.HPChanged = damage;
            }

            if (skill.BaseName == SkillDatabase.DracoTherium.BaseName) 
            {
                StatusHandler.RemoveStatus(this, StatusId.PoisonStatus);
            }

            if(HP == 0)
            {
                StatusHandler.Clear();
                DefenseModifier = 0;
                for(int i = 0; i < ElementDamageModifiers.Length; i++)
                    ElementDamageModifiers[i] = 0;
            }

            return result;
        }

        public virtual BattleResult ApplyHealingSkill(HealSkill skill)
        {
            bool dead = HP == 0;

            int healAmount = skill.HealAmount;
            if (this.StatusHandler.HasStatus(StatusId.BindStatus)) 
            {
                int twentyFivePercent = MaxHP / 4;
                int finalHP = HP + healAmount;

                if(HP >= twentyFivePercent)
                {
                    healAmount = 0;
                }
                else if (finalHP > twentyFivePercent) 
                {
                    // heal for the difference
                    healAmount = twentyFivePercent - HP;
                }
                else
                {
                    // don't do anything to heal amount
                    healAmount = healAmount;
                }
            }

            this.HP += healAmount;

            var result = new BattleResult()
            {
                HPChanged = healAmount,
                Target = this,
                SkillUsed = skill,
                ResultType = BattleResultType.HPGain
            };

            if(dead)
            {
                // add back our statuses
                StatusHandler.AddStatus(this, SkillDatabase.AtkBuff.Status.Clone());
                StatusHandler.AddStatus(this, SkillDatabase.DefBuff.Status.Clone());
            }

            return result;
        }
    }
}
