using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Plugins.AudioManager.audio_Manager;
using UnityEngine;
using UnityEngine.UI;

namespace Tutorials
{
    [ExecuteInEditMode]
    public abstract class TutorialItemUI : MonoBehaviour, ITutorialItem
    {
        [Header("Preview")]
        [SerializeField] private bool editorPreview;
        [Header("Base setup")]
        [SerializeField] private Image cutoutImage;
        [SerializeField] private Image backgroundImage;
        [SerializeField] private Canvas canvas;
        [SerializeField] private CanvasGroup darkScreen;
        [SerializeField] private Button closeButton;
        [Header("Sounds")]
        [SerializeField] private SoundData completeSound;
        private float _appearTime;

        public float Delay => delay;

        public float delay;

        public void Init(TutorialConfig config)
        {
            _appearTime = config.uiAppearTime;
            HideInstant(darkScreen);
            OnInit();
        }

        protected abstract void OnInit();


        public void StartItem()
        {
            canvas.enabled = true;
            AppearDarkScreen();
        }

        private void AppearDarkScreen()
        {
            var tween = Appear(darkScreen);
            if (tween != null)
                tween.OnComplete(AppearComplete);
            else
                OnDarkScreenAppear();
        }

        void AppearComplete()
        {
            OnDarkScreenAppear();
            ShowInstant(darkScreen);
        }

        protected abstract void OnDarkScreenAppear();


        protected void EnableCloseButton()
        {
            closeButton.onClick.AddListener(OnClose);
        }

        void OnClose()
        {
            OnComplete?.Invoke();
            closeButton.onClick.RemoveListener(OnClose);
            canvas.enabled = false;
            AudioManager.Instance.PlaySound(completeSound);
        }

        protected void ForceClose()
        {
            OnClose();
        }


        public event Action OnComplete;


        protected TweenerCore<float, float, FloatOptions> Appear(CanvasGroup canvasGroup)
        {
            return canvasGroup == null ? null : canvasGroup.DOFade(1, _appearTime);
        }

        protected static void ShowInstant(CanvasGroup canvasGroup)
        {
            if (canvasGroup == null) return;
            canvasGroup.alpha = 1;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }

        protected static void HideInstant(CanvasGroup canvasGroup)
        {
            if (canvasGroup == null) return;
            canvasGroup.alpha = 0;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }

#if UNITY_EDITOR
        private void Update()
        {
            if (Application.isPlaying) return;

            if (editorPreview)
            {
                canvas.enabled = true;
            }
            else
            {
                canvas.enabled = false;
            }
        }
#endif
    }
}