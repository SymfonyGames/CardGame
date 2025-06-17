using DG.Tweening;
using Managers;
using Player;
using UnityEngine;
using UnityEngine.UI;

namespace Tutorials.itemsUI
{
    [System.Serializable]
    public enum ActionType
    {
        None,
        OnPlayerInteract
    }

    public class TutorialAnimatedActions : TutorialItemUI
    {
        [Header("Action")]
        [SerializeField] ActionType actionType;
        [SerializeField] PlayerController playerController;
    
        [Header("Message setup")]
        [SerializeField]
        CanvasGroup actionCanvasGroup;
        [SerializeField] CanvasGroup messageCanvasGroup;
        [SerializeField] GraphicRaycaster canvasRaycaster;


        protected override void OnInit()
        {
            EventManager.Instance.DisablePlayer();

            if (actionCanvasGroup) actionCanvasGroup.gameObject.SetActive(false);
            HideInstant(messageCanvasGroup);
            canvasRaycaster.enabled = false;
        }

        protected override void OnDarkScreenAppear()
        {
            if (actionCanvasGroup)  actionCanvasGroup.gameObject.SetActive(true);
            Appear(messageCanvasGroup).OnComplete(ActionSubscribe);
        }

        void ActionSubscribe()
        {
            EventManager.Instance.EnablePlayer();
        
            if (actionType == ActionType.None)
            {
                EnableCloseButton();
            }

            if (actionType == ActionType.OnPlayerInteract)
            {
                EventManager.Instance.OnPlayerInteractWith += OnPlayerInteract;
            }
        }

        void OnPlayerInteract(Card interactCard)
        {
            EventManager.Instance.OnPlayerInteractWith -= OnPlayerInteract;
            ForceClose();
        }
    }
}