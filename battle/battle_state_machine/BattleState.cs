using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.battle.battle_state_machine
{
    public interface IBattleState
    {
        void StartState(BattleSceneObject battleSceneObject);
        void ChangeActiveEntity(BattleSceneObject battleSceneObject);
        void EndState(BattleSceneObject battleSceneObject);
    }
}
