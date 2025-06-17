using UnityEngine;

namespace Shop
{
    public abstract class ShopItem : MonoBehaviour
    {
        int _goldCost;
        int _gemCost;
        public int GoldCost => _goldCost;
        public int GemCost => _gemCost;
        public void SetupGoldCost(int value) => _goldCost = value;
        public void SetupGemCost(int value) => _gemCost = value;
    
        public abstract void OnPurchasedSuccessfully();

 
    }
}
