using System;
using UnityEngine;
using UnityEngine.UI;

namespace BossGame
{
    public class ActionCardUI : MonoBehaviour
    {
        [SerializeField] Image art;
        [SerializeField] Button button;
        public event Action OnClick = delegate { };

        void Start() 
            => button.onClick.AddListener(Click);

        void Click() => OnClick();

        public void Set(Sprite icon)
        {
            art.sprite = icon;
        }
    }
}