using System;
using Managers;
using UnityEngine;

namespace Tutorials.items
{
    public class TutorialBlockActions : MonoBehaviour, ITutorialItem
    {
        public float Delay => createDelay;
        [SerializeField] [Range(0.3f, 5f)] private float createDelay;

        public event Action OnComplete;

        public void Init(TutorialConfig tutorial)
        {

        }

        public void StartItem()
        {
            //Debug.LogError("Actions blocked!");
            EventManager.Instance.DisablePlayer();
            OnComplete?.Invoke();
        }




    }
}
