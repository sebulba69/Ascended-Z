using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// This file contains a collection of Skill-related enums
/// </summary>

namespace AscendedZ.skills
{
    // Target types -- each TargetType is from the perspective of the user.
    // all opponents, all teammates, single opponent, single teammate
    public enum TargetTypes
    {
        SINGLE_OPP, SINGLE_TEAM, SINGLE_TEAM_DEAD, TEAM_ALL, OPP_ALL, SELF, TEAM_ALL_DEAD, BOTH
    };

    // Elements
    public enum Elements
    {
        Fire, Ice, Elec, Wind, Dark, Light, Almighty
    }
}
