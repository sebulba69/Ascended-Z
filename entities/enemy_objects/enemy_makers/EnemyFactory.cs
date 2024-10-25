using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.entities.enemy_objects.enemy_makers
{
    public class EnemyFactory
    {
        protected Dictionary<string, Func<Enemy>> _functionDictionary;

        protected int _tierBoost;

        public EnemyFactory()
        {
            _functionDictionary = new Dictionary<string, Func<Enemy>>();
        }

        public virtual void SetTier(int tier)
        {
            _tierBoost = (tier/10) * 3;
        }

        public Enemy GetEnemyByName(string name)
        {
            if (_functionDictionary.ContainsKey(name))
            {
                return _functionDictionary[name].Invoke();
            }
            else
            {
                return null;
            }
        }
    }
}
