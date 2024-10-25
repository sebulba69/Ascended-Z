using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.battle
{
    public partial class BattleEffectWrapper : GodotObject
    {
        private bool _isEntitySkillUser = false;

        /// <summary>
        /// How we process animations changes based on whether or not we're the skill user.
        /// A lot of our result effects hinge on Result.Target. The user just tells us where our start animations play.
        /// </summary>
        public bool IsEntitySkillUser { get => _isEntitySkillUser; set => _isEntitySkillUser = value; }
        public BattleResult Result { get; set; }
    }
}
