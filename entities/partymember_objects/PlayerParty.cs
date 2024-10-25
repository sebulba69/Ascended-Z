using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.entities.partymember_objects
{
    public class PlayerParty
    {
        private const int MAX = 4;
        private OverworldEntity[] _party;

        public OverworldEntity[] Party
        {
            get { return _party; }
            set { _party = value; }
        }

        public int Count
        {
            get
            {
                int count = 0;

                foreach (var member in Party)
                    if (member != null)
                        count++;

                return count;
            }
        }

        public int Max { get => MAX; }

        public PlayerParty()
        {
            _party = new OverworldEntity[MAX];
        }

        public void RefreshPartyMember(OverworldEntity entity)
        {
            for(int i = 0; i < _party.Length; i++)
            {
                if (_party[i] != null && _party[i].Name.Equals(entity.Name))
                {
                    _party[i] = entity;
                    break;
                }
            }
        }

        public void AddPartyMember(OverworldEntity entity)
        {
            if(Count < MAX)
            {
                int index = 0;

                for (int i = 0; i < _party.Length; i++)
                {
                    if (_party[i] == null)
                    {
                        index = i;
                        break;
                    }    
                }

                entity.IsInParty = true;
                _party[index] = entity;
            }
        }

        public void RemovePartyMember(OverworldEntity entity)
        {
            for (int i = 0; i < _party.Length; i++)
            {
                if (_party[i] != null && _party[i].Name.Equals(entity.Name))
                {
                    entity.IsInParty = false;
                    _party[i] = null;
                    break;
                }
            }
        }
    }
}
