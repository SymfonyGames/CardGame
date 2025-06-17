using System;
using Managers;
using UnityEngine;

namespace Tutorials.items
{
    public class TutorialCreateHero : MonoBehaviour, ITutorialItem
    {
        [SerializeField] [Range(0.3f, 5f)] private float createDelay;
        [SerializeField] MoveSystem moveSystem;

        public float Delay => createDelay;
        public event Action OnComplete;

        public void Init(TutorialConfig tutorial)
        {
        }

        public void StartItem()
        {
            CreateHero();
        }

        void CreateHero()
        {
            GeneratorSystem.Instance.SpawnHero();
            moveSystem.MoveAllAtStart();
            moveSystem.MoveAllAtStart();
            moveSystem.MoveAllAtStart();
            OnComplete?.Invoke();
        }
    }
}