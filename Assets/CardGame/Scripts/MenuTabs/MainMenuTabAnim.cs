using DG.Tweening;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MenuTabs
{
    public class MainMenuTabAnim : TabStateAnim
    {
        [SerializeField] Material selectedMaterial;
        [SerializeField] Material defaultMaterial;
        [SerializeField] LayoutElement size;
        [SerializeField] Image icon;
        [SerializeField] Image back;
        [SerializeField] Image pattern;
        [SerializeField] TextMeshProUGUI nameLabel;

        [Header("DEBUG")]
        [SerializeField] MainMenuTabAnimData _anim;
        [SerializeField, ReadOnly] bool isActive;
        
        public void Init(MainMenuTabAnimData animationData)
        {
            _anim = animationData;
        }


        public override void Active()
        {
            isActive = true;
            DOVirtual.Float(0, _anim.tabExtraWidth, _anim.duration, (x) => size.minWidth = x);
            icon.transform.DOScale(_anim.iconSize, _anim.duration);
            nameLabel.DOFade(1, _anim.duration);
            back.color = _anim.selectColor;
            pattern.enabled = true;
            icon.material = selectedMaterial;
        }


        public override void Inactive()
        {
            isActive = false;
            DOVirtual.Float(size.minWidth, 0, _anim.duration, (x) => size.minWidth = x);
            icon.transform.DOScale(1, _anim.duration);
            nameLabel.DOFade(0, 0);
            back.color = _anim.unselectColor;
            pattern.enabled = false;
            icon.material = defaultMaterial;
        }
    }
}