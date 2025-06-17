using UnityEngine;
using UnityEngine.UI;

namespace Shop
{
    public class ShopPurchuaseImage : MonoBehaviour
    {
        [SerializeField] Image usedImage;
        [SerializeField] ShopPurchuaseAnimation shopAnimation;
        [Header("Test")]
        public bool test;

        public void PlayAnimation()
        {
            shopAnimation.PlayAnimation(usedImage.sprite);
        }

#if UNITY_EDITOR
        void Update()
        {
            if (test)
            {
                test = false;
                shopAnimation.PlayAnimation(usedImage.sprite);
            }
        }
#endif
    }
}
