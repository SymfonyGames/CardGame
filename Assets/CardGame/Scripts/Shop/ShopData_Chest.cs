using UnityEngine;

namespace Shop
{
    [CreateAssetMenu(fileName = "Chest", menuName = "Shop/Chest")]
    public class ShopData_Chest : ScriptableObject
    {
        [SerializeField] int goldValue;
        [SerializeField] int gemCost;

        public int GoldValue => goldValue;

        public int GemCost => gemCost;

    }
}
