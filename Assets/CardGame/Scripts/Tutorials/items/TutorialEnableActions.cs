using System;
using Managers;
using UnityEngine;

namespace Tutorials.items
{
    public class TutorialEnableActions : MonoBehaviour, ITutorialItem
    {
        [SerializeField] private InteractSystem interactSystem;
        public bool enablePlayerActions;
        public float Delay => createDelay;
        [SerializeField] [Range(0.3f, 5f)] private float createDelay;

        public event Action OnComplete;

        public void Init(TutorialConfig tutorial)
        {

        }

        public void StartItem()
        {
            if (enablePlayerActions)
                interactSystem.enablePlayerActions = true;

            EventManager.Instance.EnablePlayer();
            OnComplete?.Invoke();
        }
    }
}
