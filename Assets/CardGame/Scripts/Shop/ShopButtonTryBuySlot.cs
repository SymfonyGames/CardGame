using UnityEngine;

namespace Shop
{
    public class ShopButtonTryBuySlot : MonoBehaviour
    {
        [SerializeField] ShopItem shopItem;
 
        void Start()
        {
            if (!shopItem) shopItem = transform.parent.GetComponent<ShopItem>();
        }

        public void TryBuy() => shopItem.OnPurchasedSuccessfully();
    
    }
}
