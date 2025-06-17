using System;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MenuHeroSelect
{
    public class HeroCardUI : MonoBehaviour
    {
        //  [SerializeField] [ReadOnly] HeroSO so;
        [SerializeField] Image back;
        [SerializeField] Sprite backSelected;
        [SerializeField] Sprite backSelectedNotOwned;
        [SerializeField] Sprite backOwned;
        [SerializeField] Sprite backNotOwned;
        [SerializeField] Image selectedIcon;
        [SerializeField] Image footer;
        [SerializeField] Image icon;
        [SerializeField] Image lvlFill;
        [SerializeField] CanvasGroup lvlContainer;
        [SerializeField] TextMeshProUGUI lvlText;
        [SerializeField] TextMeshProUGUI notOwnedText;
        [SerializeField] TextMeshProUGUI heroName;
        [SerializeField] Button clickButton;
        HeroData _data;

        public event Action<HeroCardUI> OnClick = delegate { };

        //   public HeroSO SO => so;
        public HeroData Data => _data;

        public Material grayScale;
        public Material normalScale;

        public void Refresh(HeroData data, int lvlMax)
        {
            _data = data;
            //  so = data.SO;

            icon.sprite = data.hero.Art;
            heroName.text = data.hero.Name;

            //  lvlText.text = data.level + "/" + lvlMax;
            lvlText.text = "Lvl " + data.level;
            lvlFill.fillAmount = data.level == lvlMax
                ? 1
                : (float) (data.level - 1) / lvlMax;

            footer.enabled = data.owned;

            icon.material = data.owned
                ? normalScale
                : grayScale;

            notOwnedText.enabled = !data.owned;
            lvlText.enabled = data.owned;

            var clr = Color.white;
            clr.a = data.owned ? 1 : 0.5f;
            heroName.color = clr;

            lvlContainer.alpha = (data.owned) ? 1 : 0.5f;

            back.sprite = data.owned
                ? backOwned
                : backNotOwned;
        }

        void OnEnable()
        {
            clickButton.onClick.AddListener(Click);
        }

        void OnDisable()
        {
            clickButton.onClick.RemoveListener(Click);
        }

        void Click()
        {
            OnClick(this);
        }

        public void Select( )
        {
            selectedIcon.enabled = _data.owned;
            back.sprite = _data.owned ? backSelected : backSelectedNotOwned;
        }

        public void Deselect()
        {
            selectedIcon.enabled = false;
            back.sprite = _data.owned
                ? backOwned
                : backNotOwned;
        }
    }
}