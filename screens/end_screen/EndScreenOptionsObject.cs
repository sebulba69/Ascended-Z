using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.screens.end_screen
{
    public class EndScreenOptionsObject
    {
        public int SelectedIndex { get; set; }
        public List<EndScreenItem> Items { get; set; }

        public void InvokeSelected()
        {
            Items[SelectedIndex].ItemSelected?.Invoke(null, EventArgs.Empty);
        }
    }
}
