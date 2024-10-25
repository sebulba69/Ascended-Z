using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.currency
{
    /// <summary>
    /// Exception thrown when currency is at its maximum.
    /// This exception should be caught and handled in any location
    /// where a player's currency is changed.
    /// </summary>
    public partial class MaxCurrencyException : Exception
    {
        public MaxCurrencyException() : base("Maximum amount of currency obtained. Cannot hold more.")
        {
        }
    }
}
