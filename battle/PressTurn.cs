using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.battle
{
    public class PressTurn
    {
        private int _turns = 0;
        public bool TurnEnded { get; set; }

        private List<int> _turnIcons;
        public List<int> TurnIcons { get => _turnIcons; }

        public PressTurn()
        {
            _turnIcons = new List<int>();
        }

        /// <summary>
        /// Set the turn icons for the battle.
        /// </summary>
        /// <param name="turns"></param>
        public void SetTurns(int turns)
        {
            _turnIcons.Clear();
            for (int i = 0; i < turns; i++)
            {
                _turnIcons.Add(2);
            }

            if (turns == 0)
                TurnEnded = true;
        }

        public void HandleTurns(BattleResultType resultType)
        {
            switch (resultType)
            {
                case BattleResultType.Dr:
                    EndTurn();
                    break;
                case BattleResultType.Evade:
                case BattleResultType.Nu:
                    NullTurn();
                    break;
                case BattleResultType.Wk:
                case BattleResultType.Tech:
                case BattleResultType.TechWk:
                    HalfTurn();
                    break;
                case BattleResultType.Pass:
                    PassTurn();
                    break;
                case BattleResultType.Retreat:
                    break;
                case BattleResultType.BeastEye:
                    DoEyeTurn(2);
                    break;
                case BattleResultType.DragonEye:
                    DoEyeTurn(4);
                    break;
                default:
                    FullTurn();
                    break;
            }
        }

        private void DoEyeTurn(int halfTurns)
        {
            _turnIcons.RemoveAt(0);

            for (int t = 0; t < halfTurns; t++)
            {
                if(_turnIcons.Count == 0)
                    _turnIcons.Add(1);
                else
                    _turnIcons.Insert(0, 1);
            }
        }
        /// <summary>
        /// Find the first 2 and turn it into a 1. If no 2 exists, then do a full turn.
        /// </summary>
        private void HalfTurn()
        {
            int index = _turnIcons.FindIndex(x => x == 2);
            if(index == -1)
            {
                FullTurn();
            }
            else
            {
                _turnIcons[index] = 1;
            }
        }

        /// <summary>
        /// If the first icon is a 2, make it a 1, else remove it from the list.
        /// </summary>
        private void PassTurn()
        {
            if (_turnIcons[0] == 2)
                _turnIcons[0] = 1;
            else
                FullTurn();
        }

        private void FullTurn()
        {
            if(_turnIcons.Count > 0)
            {
                _turnIcons.RemoveAt(0);
                if (_turnIcons.Count == 0)
                    EndTurn();
            }
            else
            {
                EndTurn();
            }

        }

        private void NullTurn()
        {
            if (_turnIcons.Count >= 2)
            {
                for (int i = 0; i < 2; i++)
                {
                    FullTurn();
                }
            }
            else
            {
                EndTurn();
            }    
        }

        private void EndTurn()
        {
            _turnIcons.Clear();
            TurnEnded = true;
        }

    }
}
