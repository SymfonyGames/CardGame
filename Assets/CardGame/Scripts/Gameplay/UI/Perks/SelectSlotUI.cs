using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.UI.Perks
{
    public class SelectSlotUI : MonoBehaviour
    {
        [SerializeField] Image icon;
        [SerializeField] TextMeshProUGUI description;
        [SerializeField] TextMeshProUGUI nameLabel;
        [SerializeField] TextMeshProUGUI levelLabel;
        public Button lvlUpButton;
        public Button learnButton;
        public event Action<SelectSlotUI> OnClick = delegate { };

        [SerializeField] Button button;
        HeroAbilityData _data;

        public HeroAbilityData Data => _data;
        //[SerializeField] StarsPanelUI stars;

        public void Enable() => gameObject.SetActive(true);
        public void Disable() => gameObject.SetActive(false);

        void Awake()
        {
            button.onClick.AddListener(Click);
            lvlUpButton.onClick.AddListener(Click);
            learnButton.onClick.AddListener(Click);
        }

        void Click()
            => OnClick(this);

        public void Set(HeroAbilityData data)
        {
            _data = data;
            nameLabel.text = data.Ability.Name;
            description.text = data.Ability.Description;
            icon.sprite = data.Ability.Icon;

            levelLabel.text = "Level " + data.Level;
            levelLabel.enabled = data.Level != 0;

            lvlUpButton.gameObject.SetActive(data.Level != 0);
            learnButton.gameObject.SetActive(data.Level == 0);
        }
    }
}