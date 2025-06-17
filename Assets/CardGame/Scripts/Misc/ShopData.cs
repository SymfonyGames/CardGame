 
using UnityEngine;

namespace Misc
{
    [CreateAssetMenu(fileName = "Shop Data", menuName = "Game/Shop Data")]
    public class ShopData : ScriptableObject
    {
        [Header("Purchuase animation")]
        [SerializeField]
        float imageAppearTime;
        [SerializeField] float imageDisppearTime;

        public float ImageAppearTime => imageAppearTime;
        public float ImageDisppearTime => imageDisppearTime;
    }
}
