using Managers;
using UnityEngine;

namespace Shop
{
    public class ShopButtonClose : MonoBehaviour
    {
        public void CloseShop() => EventManager.Instance.ShopClose();
    }
}
