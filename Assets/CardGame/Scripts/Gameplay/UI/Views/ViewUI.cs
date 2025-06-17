using System;
 
using UnityEngine;

namespace Gameplay.UI.Views
{
    public class ViewUI : MonoBehaviour
    {
        public CanvasGroup canvasGroup;

        public event Action OnHide = delegate { };
        public event Action OnShow = delegate { };

        void Awake()
        {
            Hide();
        }

        protected virtual void OnShowUI() { }

        protected virtual void OnHideUI() { }

        public void Show()
        {
         //   Log.UIShow(gameObject.name, gameObject);
            canvasGroup.alpha = 1;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
            OnShow();
            OnShowUI();
        }

        public void Hide()
        {
          //  Log.UIHide(gameObject.name);
            canvasGroup.alpha = 0;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            OnHide();
            OnHideUI();
        }
    }
}