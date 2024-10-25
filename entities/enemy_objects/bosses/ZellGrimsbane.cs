using AscendedZ.battle;
using AscendedZ.battle.battle_state_machine;
using AscendedZ.entities.battle_entities;
using AscendedZ.skills;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static Godot.WebSocketPeer;

namespace AscendedZ.entities.enemy_objects.bosses
{
    public class ZellGrimsbane : Enemy
    {
        private readonly List<Elements> _elementCycle = [Elements.Fire, Elements.Ice, Elements.Elec, Elements.Wind, Elements.Dark, Elements.Light];
        private List<ISkill> _elementSkills;
        private ISkill _fire, _ice, _elec, _wind, _dark, _light, _almighty, _atkBuff, _defDebuff, _techBuff;

        private List<ISkill> _wipeScript, _reactionScript, _primaryScript;

        private int _current;

        public ZellGrimsbane() : base()
        {
            Name = EnemyNames.Zell_Grimsbane;
            Image = CharacterImageAssets.GetImagePath(Name);
            MaxHP = EntityDatabase.GetBossHP(Name);
            Turns = 5;
            _isBoss = true;

            _fire = SkillDatabase.Fire1;
            _ice = SkillDatabase.Ice1;
            _elec = SkillDatabase.Elec1;
            _wind = SkillDatabase.Wind1;
            _dark = SkillDatabase.Dark1;
            _light = SkillDatabase.Light1;
            _techBuff = SkillDatabase.TechBuff;
            _atkBuff = SkillDatabase.AtkBuff;
            _defDebuff = SkillDatabase.DefDebuff;
            _almighty = SkillDatabase.Almighty;

            _elementSkills = [_fire, _ice, _elec, _wind, _dark, _light];

            foreach (var element in _elementCycle)
            {
                Resistances.SetResistance(resistances.ResistanceType.Wk, element);
            }

            _current = 0;

            _reactionScript = new List<ISkill>();

            Skills.AddRange(
                [
                    _fire, _ice, _elec, _wind, _dark, _light, _almighty,
                    _techBuff, _atkBuff, _defDebuff
                ]);

            _wipeScript = [_atkBuff, _atkBuff, _techBuff, _almighty, _techBuff, _almighty, _almighty];

            _primaryScript = _wipeScript;

            Description = $"{Name}: Each time its weakness is hit, {Name} will set its resistance to that element to Null and use that element the following turn. If all its resistances are null, it will use almighty Attacks";
        }

        public override BattleResult ApplyElementSkill(BattleEntity user, ElementSkill skill)
        {
            BattleResult result = base.ApplyElementSkill(user, skill);

            if (result.ResultType == BattleResultType.Wk)
            {
                int elementIndex = (int)skill.Element;
                if (elementIndex < _elementCycle.Count) 
                {
                    Resistances.SetResistance(resistances.ResistanceType.Nu, skill.Element);
                    _reactionScript.Add(_elementSkills[elementIndex]);
                    if (_primaryScript != _reactionScript)
                    {
                        _primaryScript = _reactionScript;
                        _current = 0;
                    }
                }
            }

            Turns = _reactionScript.Count + 1;

            return result;
        }

        public override EnemyAction GetNextAction(BattleSceneObject battleSceneObject)
        {
            EnemyAction action = new EnemyAction();

            // default to the wipe script
            if (_primaryScript.Count == 0)
                _primaryScript = _wipeScript;

            if(Turns == 1)
            {
                action.Skill = SkillDatabase.DragonEye;
                action.Target = this;
                Turns = 5;
            }
            else
            {
                action.Skill = _primaryScript[_current];

                if (action.Skill.Id == SkillId.Elemental)
                    action.Target = FindElementSkillTarget((ElementSkill)action.Skill, battleSceneObject);
                else
                    action.Target = FindTargetForStatus((StatusSkill)action.Skill, battleSceneObject);

                _current++;

                if (_current >= _primaryScript.Count)
                {
                    _current = 0;
                }
            }

            return action;
        }

        public override void ResetEnemyState()
        {
            if(_primaryScript == _wipeScript)
            {
                foreach (var element in _elementCycle)
                {
                    Resistances.SetResistance(resistances.ResistanceType.Wk, element);
                }
            }

            _current = 0;
            bool nullAll = true;
            foreach(var element in _elementCycle)
            {
                if (Resistances.IsWeakToElement(element))
                {
                    nullAll = false;
                    break;
                }
            }

            _reactionScript.Clear();

            if (nullAll)
            {
                _primaryScript = _wipeScript;
                Turns = 5;
            }
                
        }
    }
}
