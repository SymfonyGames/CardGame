using DG.Tweening;
using UnityEngine;

namespace Tutorials.itemsUI
{
    public class TutorialHighlight : TutorialItemUI
    {
        [Header("Message setup")]
        [SerializeField] CanvasGroup messageCanvasGroup;


        protected override void OnInit()
        {
            HideInstant(messageCanvasGroup);
        }

        protected override void OnDarkScreenAppear()
        {
            Appear(messageCanvasGroup).OnComplete(EnableCloseButton);
        }



    }
}