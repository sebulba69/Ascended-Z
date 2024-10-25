using AscendedZ.currency;
using AscendedZ.entities;
using AscendedZ.entities.partymember_objects;
using AscendedZ.game_object;
using Godot.NativeInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.screens.upgrade_screen
{
    public class UpgradeItemObject
    {
        private Currency _vorpex, _partyCoins, _bountyKeys;
        private OverworldEntity _entity;
        private GameObject _gameObject;
        public OverworldEntity Entity { get => _entity; }

        public UpgradeItemObject(GameObject gameObject)
        {
            _gameObject = gameObject;
            var wallet = _gameObject.MainPlayer.Wallet;
            _vorpex = wallet.Currency[SkillAssets.VORPEX_ICON];
            _partyCoins = wallet.Currency[SkillAssets.PARTY_COIN_ICON];
            if(wallet.Currency.ContainsKey(SkillAssets.BOUNTY_KEY))
                _bountyKeys = wallet.Currency[SkillAssets.BOUNTY_KEY];
        }

        public void Initialize(OverworldEntity entity)
        {
            _entity = entity;
        }

        public bool CanRefund()
        {
            return _gameObject.MainPlayer.ReserveMembers.Count > 3;
        }

        public void Upgrade()
        {
            int cost = _entity.VorpexValue;

            if (_vorpex.Amount >= cost)
            {
                _vorpex.Amount -= cost;
                _entity.LevelUp();
                PersistentGameObjects.Save();
            }
        }

        public void UpgradeHP()
        {
            if (_bountyKeys.Amount >= 1)
            {
                _bountyKeys.Amount -= 1;
                _entity.HPBoost();
                PersistentGameObjects.Save();
            }
        }

        public void Refund()
        {
            int cost = _entity.RefundReward;
            _partyCoins.Amount += cost;

            int boost = _entity.FusionGrade;
            int vorpexGain = _entity.VorpexValue;

            if (boost > 0)
            {
                vorpexGain *= boost;
            }
            
            var gameObject = PersistentGameObjects.GameObjectInstance();
            _vorpex.Amount += vorpexGain;

            if (_entity.IsInParty)
            {
                var party = gameObject.MainPlayer.Party;
                party.RemovePartyMember(_entity);
            }

            foreach(var sigil in _entity.Sigils)
                sigil.Index = -1;

            _gameObject.MainPlayer.ReserveMembers.Remove(_entity);

            PersistentGameObjects.Save();
        }
    }
}
