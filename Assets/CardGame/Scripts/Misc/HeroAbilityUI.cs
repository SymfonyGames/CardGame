using System.Collections.Generic;
using DG.Tweening;
using Gameplay.UI.Perks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Utilities.Extensions.UIExtensions;

namespace Misc
{
    public class HeroAbilityUI : MonoBehaviour
    {
        public TextMeshProUGUI cdTxt;

        public Image cdImg;
        public GameObject cdObject;
        public Image frame;
        public List<Sprite> frameImages;
        public TextMeshProUGUI lvlText;
        public TextMeshProUGUI manaCostText;
        public CanvasGroup group;
        [SerializeField] Material grayMat;
        [SerializeField] Material activeMat;
        [SerializeField] Image icon;
        public List<Sprite> sprites = new();
        public List<CardDataHero> heroes = new();
        [SerializeField] Image fillImg;
        [SerializeField] Image backImg;
        [SerializeField] Color backColor;
        [SerializeField] Color backColorDisabled;
        [SerializeField] Color fillColor;
        [SerializeField] Color activeColor;
        [SerializeField] Transform container;
        [SerializeField] TextMeshProUGUI txt;
        [SerializeField] Button button;
        [SerializeField] Transform potCont;
        public Button ClickButton => button;
        Color mpColorDefault;
        public Color mpColorOutOf;

        public void RefreshMP(int mp, int manaCost)
        {
            manaCostText.color = mp < manaCost ? mpColorOutOf : mpColorDefault;
            manaCostText.text = manaCost.ToString();
        }

        public void SetCooldown(int cd, float value)
        {
            if (cd > 0)
            {
                cdObject.SetActive(true);
                cdTxt.text = cd.ToString();
                //  cdImg.fillAmount = value;
                // Debug.LogError(value);
                DOVirtual.Float(cdImg.fillAmount, value, 1f, f => cdImg.fillAmount = f);
            }
            else
            {
                if (cdObject.activeSelf)
                    DOVirtual.Float(cdImg.fillAmount, 0, 0.3f, f => cdImg.fillAmount = f)
                        .OnComplete(() => cdObject.SetActive(false));
            }
        }

        public void Hide()
        {
            frame.enabled = false;
            frame.color = Color.black;
            frame.DOFade(0.3f, 0);
            icon.material = grayMat;
            lvlText.transform.parent.gameObject.SetActive(false);
            manaCostText.transform.parent.gameObject.SetActive(false);
            // DisableGroup(group);
            group.DOFade(0.8f, 0f);
        }

        public void Show()
        {
            frame.color = Color.white;
            frame.DOFade(1, 0);
            frame.enabled = true;
            icon.material = null;
            EnableGroup(group);
            lvlText.transform.parent.gameObject.SetActive(true);
            manaCostText.transform.parent.gameObject.SetActive(true);
        }

        public void Init(CardDataHero data, HeroAbilityData ability)
        {
            _data = data;
            this._ability = ability;
            icon.sprite = ability.Ability.Icon;
            mpColorDefault = manaCostText.color;
            return;
            if (heroes.Count == 0) return;
            if (heroes.Contains(data))
            {
                var id = heroes.IndexOf(data);
                icon.sprite = sprites[id];
            }
        }

        public void Enable()
        {
            button.interactable = true;
            backImg.color = backColor;
        }

        void Reset()
        {
            container.localScale = Vector3.one;
            potCont.localScale = Vector3.one;
        }

        public void Disable()
        {
            Debug.LogError("DISABLE");
            button.interactable = false;
            backImg.color = backColorDisabled;
            Reset();
        }

        public void StopAnim()
        {
            if (!_isAnimated) return;
            _isAnimated = false;
            container.DOKill();
        }

        bool _isAnimated;
        CardDataHero _data;
        HeroAbilityData _ability;

        public HeroAbilityData Data => _ability;

        void Animate()
        {
            if (_isAnimated) return;

            _isAnimated = true;
            container.DOScale(1.3f, 0.7f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InQuad);
        }

        public void SetLevel(int lvl) => lvlText.text = "Lvl. " + lvl.ToString();

        public void SetProgress(float value, int current, int max)
        {
            return;
            fillImg.fillAmount = value;
            fillImg.color = value < 1 ? fillColor : activeColor;
            icon.material = value < 1 ? grayMat : activeMat;

            if (value >= 1)
            {
                Animate();
            }
            else
            {
                container.DOScale(1.2f, 0.2f).OnComplete(() =>
                    container.DOScale(1, 0.2f));
                StopAnim();
            }

            txt.text = current + "/" + max;

            if (Time.time > 1)
            {
                potCont.DOScale(1.5f, 0.2f).OnComplete(()
                    => potCont.DOScale(1f, 0.2f));
            }
        }
    }
}