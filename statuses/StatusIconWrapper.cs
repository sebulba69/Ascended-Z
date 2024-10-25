using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.statuses
{
    /// <summary>
    /// Wrapper for generating a display Icon for our statuses.
    /// </summary>
    public partial class StatusIconWrapper : GodotObject
    {
        private int _counter = 0;
        private Color _counterColor = Colors.White;
        
        public string Icon { get; set; }
        public int Counter 
        { 
            get => _counter; 
            set => _counter = value; 
        }

        public string Description { get; set; }

        public bool SetInvisible { get; set; }

        /// <summary>
        /// The color we want the counter to be.
        /// </summary>
        public Color CounterColor { get => _counterColor; set => _counterColor = value; }
    }
}
