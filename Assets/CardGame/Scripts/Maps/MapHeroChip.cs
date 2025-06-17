using UnityEngine;
using UnityEngine.UI;

namespace Maps
{
    public class MapHeroChip : MonoBehaviour
    {
        [SerializeField] private Image heroIcon;
        [SerializeField] private Image cardFrame;
        [SerializeField] private Image glow;
        [SerializeField] private Image shadow;

        public Image Glow => glow;
        public Image Shadow => shadow;
        public Image CardFrame => cardFrame;
        public Image HeroIcon => heroIcon;


        public void SetIcon(Sprite sprite)
        {
            heroIcon.sprite = sprite;
        }

    }
}
