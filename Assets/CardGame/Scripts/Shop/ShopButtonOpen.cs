using Managers;
using UnityEngine;

namespace Shop
{
   public class ShopButtonOpen : MonoBehaviour
   {
      public void OpenShop() => EventManager.Instance.ShopOpen();
   }
}
