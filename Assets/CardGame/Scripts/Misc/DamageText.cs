using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Misc
{
    public class DamageText : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI txt;
        [SerializeField] CanvasGroup group;
        [SerializeField] Color easyClr;
        [SerializeField] Color normalClr;
        [SerializeField] Color hardClr;
        [SerializeField] Color hpClr;
        [SerializeField] Image dangerImg;

        void Awake()
        {
            Hide();
        }

        public void ShowHeal(int amount, float delay)
        {
            txt.text = "+" + amount;
            dangerImg.color = hpClr;
            Invoke(nameof(Show), delay);
        }

        public void ShowDelayed(float damage, CreatureDangerous danger, float delay)
        {
            txt.text = "-" + damage;
            dangerImg.color = hardClr;
            // dangerImg.color = danger switch
            // {
            //     CreatureDangerous.Easy => easyClr,
            //     CreatureDangerous.Normal => normalClr,
            //     CreatureDangerous.Hard => hardClr,
            //     _ => Color.white
            // };
            Invoke(nameof(Show), delay);
        }

        public float showDuration = 1f;
        void Show()
        {
            group.alpha = 1;

            group.DOFade(0, 0.2f).SetDelay(showDuration);
        }

        public void Hide()
        {
            group.alpha = 0;
        }
    }
}