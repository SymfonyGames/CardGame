using System;
using Level;
using UnityEngine;

namespace Tutorials.items
{
    public class TutorialAddProgressIcon : MonoBehaviour, ITutorialItem
    {
        [SerializeField] [Range(0.3f, 5f)] private float createDelay;
        [Header("Setup")]
        public LevelProgressUI progressUI;
        public Sprite icon;
        [Range(0, 1)] public float progressValue;

        public float Delay => createDelay;
        public event Action OnComplete;

        public void Init(TutorialConfig tutorial)
        {
        }

        public void StartItem()
        {
            progressUI.AddIcon(icon, progressValue);
            OnComplete?.Invoke();
        }
    }
}