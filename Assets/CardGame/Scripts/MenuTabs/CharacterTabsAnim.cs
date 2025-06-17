using UnityEngine;
using UnityEngine.UI;

namespace MenuTabs
{
    public class CharacterTabsAnim : TabStateAnim
    {
        [SerializeField] Sprite active;
        [SerializeField] Sprite inactive;
        [SerializeField] Image image;
        
        public override void Active()
        {
            image.sprite = active;
        }

        public override void Inactive()
        {
            image.sprite = inactive;
        }
    }
}
