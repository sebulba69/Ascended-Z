using AscendedZ.entities.partymember_objects;
using AscendedZ.game_object;
using AscendedZ.skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.screens.skill_transfer_screen
{
    public class EntitySelectionSceneObject
    {
        private List<OverworldEntity> _available;

        public List<OverworldEntity> Available { get { return _available; } }
        public OverworldEntity Selected { get; set; }

        public EntitySelectionSceneObject(List<OverworldEntity> available)
        {
            _available = available;
            Selected = _available[0];
        }

        public void ChangeSelected(int index)
        {
            Selected = _available[index];
        }
    }
}
