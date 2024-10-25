using AscendedZ.entities.battle_entities;
using AscendedZ.entities.enemy_objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.battle
{
    /// <summary>
    /// A set of values used to update the UI.
    /// These values should be set once from the BattleSceneObject
    /// and read from by the UI upon being received.
    /// </summary>
    public class BattleUIUpdate
    {
        private bool _userCanInput = true;
        public BattleResult Result { get; set; }
        public bool UserCanInput { get => _userCanInput; set => _userCanInput = value; }
        public string LogString { get; set; }

    }
}
