using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.currency
{
    /// <summary>
    /// An object meant for storing complex currency objects.
    /// </summary>
    public partial class Wallet
    {
        public Dictionary<string, Currency> Currency { get; set; } = new();

        public Wallet() 
        {
            if(Currency == null)
                Currency = new Dictionary<string, Currency>();
        }

        public void AddCurrency(Currency currency)
        {
            if (Currency.ContainsKey(currency.Name))
            {
                Currency[currency.Name].Amount += currency.Amount;
            }
            else
            {
                Currency.Add(currency.Name, currency);
            }
        }
    }
}
