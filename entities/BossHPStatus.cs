using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.entities
{
    public class BossHPStatus
    {
        public Color FG { get; set; }
        public Color BG { get; set; }
        public int MaxBarHP { get; set; }
        public int CurrentBarHP { get; set; }
        public int NumBars { get; set; }

        public override string ToString()
        {
            return $"{CurrentBarHP}/{MaxBarHP} -- {NumBars}";
        }
    }
}
