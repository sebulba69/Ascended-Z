using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.currency
{
    /// <summary>
    /// Base class for all forms of sub currency besides gold.
    /// </summary>
    public class Currency
    {
        private const int MAX = 999999;

        private int _amount = 0;

        public string Name { get; set; }
        
        public int Amount 
        {
            get
            {
                return _amount;
            }
            set
            {
                if(value > MAX)
                {
                    _amount = MAX;
                }
                else
                {
                    _amount = value;
                }
            }
        }

        public string Icon { get; set; }
    }
}
