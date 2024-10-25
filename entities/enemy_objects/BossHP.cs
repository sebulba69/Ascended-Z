using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.entities.enemy_objects
{
    public class BossHP
    {
        /// <summary>
        /// Max limit of HP per bar
        /// </summary>
        private const int HP_DEFAULT = 10000;
        private int _maxHPPerBar, _subBarDefault;
        
        private readonly string BGCOLOR = "000000";
        private readonly string[] HPFGColors = 
        {
            "99b655", "9aff91",
            "ffff6b", "ffb86b",
            "13376b", "9c150b",
            "9c1582", "8fa582",
            "879bd4", "bfcbeb",
            "b4d4ca", "44e6e4",
            "98ddbe", "71bc8c"
        };

        private int _bars, _currentDisplayHP, _maxHPBarDisplay, _totalHP;

        private Color _fgColor, _bgColor;

        public BossHP()
        {
            // by default, the max hp per bar is 5k
            _maxHPPerBar = HP_DEFAULT;
            _maxHPBarDisplay = _maxHPPerBar;
            _subBarDefault = HPFGColors.Length * 2;
            _bars = 0;
        }

        public void Setup(int startingHP)
        {
            int max = _maxHPPerBar * _subBarDefault;
            if(startingHP >= max)
            {
                _maxHPPerBar = startingHP / _subBarDefault;
                while (startingHP >= max)
                {
                    _maxHPPerBar += HP_DEFAULT;
                    max = _maxHPPerBar * _subBarDefault;
                }
            }

            _maxHPPerBar = Math.Abs(_maxHPPerBar);
            _maxHPBarDisplay = _maxHPPerBar;
        }

        public void InitializeBossHP(int hp, bool doNotSetMaxHPValue = false)
        {
            _totalHP = hp;
            _bars = 0;

            // if the total HP is less than MAX_HP_PER_BAR, then we
            // will just set up a custom bar with custom HP values
            // there's no need to adjust the algorithm for back colors
            if (_totalHP < _maxHPPerBar && !doNotSetMaxHPValue)
            {
                _maxHPBarDisplay = _totalHP;
                _currentDisplayHP = _totalHP;
            }
            else
            {
                int totalHP = _totalHP;
                while (totalHP > _maxHPPerBar)
                {
                    _bars++;
                    totalHP -= _maxHPPerBar;
                }

                _currentDisplayHP = totalHP;
            }

            SetBarColors();
        }

        public void ChangeHP(int hp)
        {
            if (_totalHP == hp)
                return;

            if(_bars == 0)
            {
                _currentDisplayHP = hp;
            }
            else
            {
                InitializeBossHP(hp, true);
            }
        }

        private void SetBarColors()
        {
            if(_bars > 0)
            {
                int fgIndex = (_bars % HPFGColors.Length);
                int bgIndex = fgIndex - 1;
                if (bgIndex < 0)
                {
                    bgIndex = HPFGColors.Length - 1;
                }

                _fgColor = new Color(HPFGColors[fgIndex]);
                _bgColor = new Color(HPFGColors[bgIndex]);
            }
            else
            {
                _fgColor = new Color(HPFGColors[0]);
                _bgColor = new Color(BGCOLOR);
            }
        }

        public BossHPStatus GetBossHPUIValues()
        {
            return new BossHPStatus()
            {
                CurrentBarHP = _currentDisplayHP,
                MaxBarHP = _maxHPBarDisplay,
                FG = _fgColor,
                BG = _bgColor,
                NumBars = _bars
            };
        }
    }
}
