using System;
using UnityEngine;
using static Utilities.Extensions.UIExtensions;

namespace MenuTabs
{
    public class Tab : MonoBehaviour
    {
        [SerializeField] TabUI ui;
        [SerializeField] Canvas canvas;
        [SerializeField] CanvasGroup canvasGroup;
        [SerializeField] Transform container;

        public event Action OnTabEnabled = delegate { };
        public event Action OnTabDisabled = delegate { };
        public event Action<Tab> OnClick = delegate { };

        public TabUI UI => ui;


        void Awake()
        {
            ui.OnClick += Click;
            Disable();
            ResetOffset();
        }

        void OnDisable() => ui.OnClick -= Click;
        public void Click() => OnClick(this);

        void ResetOffset()
        {
            if (container)
                container.localPosition = Vector3.zero;
        }


        public void Enable()
        {
            if (canvas) canvas.enabled = true;
            if (canvasGroup) EnableGroup(canvasGroup);
            ui.SelectedAnim();
            OnTabEnabled();
        }

        public void Disable()
        {
            if (canvas) canvas.enabled = false;
            if (canvasGroup) DisableGroup(canvasGroup);
            ui.UnSelectedAnim();
            OnTabDisabled();
        }
    }
}