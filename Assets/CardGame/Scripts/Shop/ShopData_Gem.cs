using UnityEngine;

namespace Shop
{
    [CreateAssetMenu(fileName = "Gem", menuName = "Shop/Gem")]
    public class ShopData_Gem : ScriptableObject
    {
    
        [SerializeField] int value;
        [SerializeField] string gemChestName;
        public int Value => value;
        public string GemChestName => gemChestName;
    }
}
