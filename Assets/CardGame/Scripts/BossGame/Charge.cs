using Managers;
using UnityEngine;

namespace BossGame
{
    public class Charge : MonoBehaviour
    {
        GamePlayer _owner;
        float _maxCharge;
        float charge;

        public void Init(GamePlayer owner, float maxCharge)
        {
            _maxCharge = maxCharge;
            _owner = owner;
            EventManager.Instance.OnChargePlayer += AddCharge;
            RefreshUI();
        }

        void AddCharge(GamePlayer player, float value)
        {
            if (player != _owner) return;
            charge += value;
            if (charge > _maxCharge) charge = _maxCharge;

            RefreshUI();
        }

        void RefreshUI()
        {
            var v = charge / _maxCharge;
 
            _owner.UI.RefreshCharge(v);
        }
    }
}