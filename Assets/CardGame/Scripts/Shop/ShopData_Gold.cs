using UnityEngine;

namespace Shop
{
    [CreateAssetMenu(fileName = "Gold", menuName = "Shop/Gold")]
    public class ShopData_Gold : ScriptableObject
    {
        [SerializeField] string goldChestName;
        [SerializeField] int value;
        public int Value => value;
        public string GoldChestName => goldChestName;
    }
}