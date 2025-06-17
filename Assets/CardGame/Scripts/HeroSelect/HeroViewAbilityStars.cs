using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HeroSelect
{
    public class HeroViewAbilityStars : MonoBehaviour
    {
        public List<Image> stars = new();
        public Sprite activeSprite;
        public Sprite passiveSprite;
        public Button levelUp;
        public TextMeshProUGUI price;
        public Image icon;

        public void SetIcon(Sprite sprite)
        {
            icon.sprite = sprite;
        }

        public void SetLevel(int lvl)
        {
            var id = lvl - 1;
            for (int i = 0; i < stars.Count; i++)
            {
                stars[i].sprite = i < id ? activeSprite : passiveSprite;
            }
        }

        public CanvasGroup group;
        public void Hide() => group.alpha = 0;
        public void Show() => group.alpha = 1;

        public void SetPrice(int cost)
        {
            price.text = cost.ToString();
        }

        public void EnableLevelup()
        {
            levelUp.interactable = true;
        }

        public void DisableLevelup()
        {
            levelUp.interactable = false;
        }
    }
}