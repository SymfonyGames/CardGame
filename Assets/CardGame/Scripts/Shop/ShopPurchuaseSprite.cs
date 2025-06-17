using UnityEngine;

namespace Shop
{
    public class ShopPurchuaseSprite : MonoBehaviour
    {
        [SerializeField] Sprite appearSprite;
        [SerializeField] ShopPurchuaseAnimation shopAnimation;
        [Header("Test")]
        public bool test;

        public void PlayAnimation()
        {
            shopAnimation.PlayAnimation(appearSprite);
        }

#if UNITY_EDITOR
        void Update()
        {
            if (test)
            {
                test = false;
                shopAnimation.PlayAnimation(appearSprite);
            }
        }
#endif
    }
}
