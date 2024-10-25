using AscendedZ.battle.battle_state_machine;
using AscendedZ.battle;
using AscendedZ.resistances;
using AscendedZ.skills;
using System.Collections.Generic;
using AscendedZ.entities.battle_entities;
using AscendedZ.game_object;
using System.Linq;

namespace AscendedZ.entities.enemy_objects.special_bosses
{
    public class Mhaarvosh : Enemy
    {
        private int _turns = 0;
        private int _currentScriptIndex;

        private ISkill _ancientChoir, _techPlus;
        private ISkill[] _startScript, _script, _buffScript;
        private ISkill[] _currentScript;
        private bool _startOfTurnScript;

        public Mhaarvosh() : base()
        {
            Name = EnemyNames.Mhaarvosh;
            Image = CharacterImageAssets.GetImagePath(Name);
            MaxHP = 8000;
            Turns = 3;
            _isBoss = true;
            _startOfTurnScript = true;

            Resistances.SetResistance(ResistanceType.Wk, Elements.Ice);
            Resistances.SetResistance(ResistanceType.Nu, Elements.Fire);
            Resistances.SetResistance(ResistanceType.Nu, Elements.Wind);
            Resistances.SetResistance(ResistanceType.Dr, Elements.Dark);
            Resistances.SetResistance(ResistanceType.Dr, Elements.Light);

            _ancientChoir = SkillDatabase.AncientChoir;
            _techPlus = SkillDatabase.TechBuff;

            var fireP = SkillDatabase.FireAllP;
            var windP = SkillDatabase.WindAllP;
            var fireMG = SkillDatabase.FireMadGod;
            var windMG = SkillDatabase.WindMadGod;
            var almighty = SkillDatabase.Almighty;

            var buffBoost = SkillDatabase.BuffBoost;
            var debuffBoost = SkillDatabase.DebuffBoost;
            var buff = SkillDatabase.HolyGrail;
            var beastEye = SkillDatabase.BeastEye;

            _currentScriptIndex = 0;

            _startScript = [beastEye, buffBoost, debuffBoost, _ancientChoir, _ancientChoir];
            _script = [fireMG, windP, _ancientChoir, fireP, windMG, _ancientChoir];
            _buffScript = [fireP, buff, buff];
            _currentScript = _startScript;

            Skills.AddRange(
            [
                fireP, windP, fireMG, windMG
            ]);

            Description = $"{Name}: Wonderful! In your desperation you've come here for answers! Submit yourself to me and you might find them! Hoo hoo hoo!";
        }

        public override List<ISkill> GetDisplaySkillList()
        {
            var list = base.GetDisplaySkillList();

            list.AddRange([SkillDatabase.BuffBoost, SkillDatabase.DebuffBoost, SkillDatabase.HolyGrail, _ancientChoir, SkillDatabase.BeastEye]);

            return list;
        }

        public override EnemyAction GetNextAction(BattleSceneObject battleSceneObject)
        {
            var skill = _currentScript[_currentScriptIndex];
            var target = FindTarget(skill, battleSceneObject);

            if (!_startOfTurnScript)
            {
                if (_currentScriptIndex + 1 % 3 == 0 && HP <= MaxHP / 2)
                {
                    skill = _techPlus;
                    target = this;
                }
            }

            EnemyAction action = new EnemyAction()
            {
                Skill = skill,
                Target = target
            };

            _currentScriptIndex++;
            if (_currentScriptIndex >= _currentScript.Length)
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
                if(skill.Id == SkillId.Eye)
                {
                    target = this;
                }
                else
                {
                    target = FindTargetForStatus((StatusSkill)skill, battleSceneObject);
                }
            }

            if (target == null)
                target = this;

            return target;
        }

        public override void ResetEnemyState()
        {
            _currentScriptIndex = 0;
            _startOfTurnScript = false;
            _turns++;
            if (_currentScript == _startScript)
                _currentScript = _script;

            if (_turns == 3)
            {
                _currentScript = _buffScript;
                _turns = 0;
            }
            else
                _currentScript = _script;
        }
    }
}
