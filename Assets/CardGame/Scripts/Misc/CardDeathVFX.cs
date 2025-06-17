using DG.Tweening;
 
using UnityEngine;
using UnityEngine.UI;

namespace Misc
{
    public class CardDeathVFX : MonoBehaviour
    {
        [SerializeField] CanvasGroup group;
        [SerializeField] CanvasGroup hpGroup;
        [SerializeField] CanvasGroup cardGroup;
        [SerializeField] Image art;

        void Awake()
        {
            Hide();
        }

        public void Show(float duration)
        {
             group.DOFade(1, 0.2f);
          art.DOFade(0, 0.2f);
            hpGroup.DOFade(0, 0.1f);
          cardGroup.DOFade(0, 0.2f);// .SetDelay(duration - 0.1f);
        }


        public void Hide()
        {
            group.alpha = 0;
        }
    }
}