using System;
using Managers;
using UnityEngine;

namespace Tutorials.items
{
    public class TutorialSpawnMiniBoss : MonoBehaviour, ITutorialItem
    {
        [SerializeField] [Range(0.3f, 5f)] private float createDelay;
        [SerializeField] MoveSystem moveSystem;
        [SerializeField] CellsType moveToCells;
        [Header("Cards")]
        [SerializeField] private CardDataBoss card;
 


        public float Delay => createDelay;
        public event Action OnComplete;

        public void Init(TutorialConfig tutorial)
        {
    
        }

        public void StartItem()
        {
            SpawnMiniBoss();
        }

        void SpawnMiniBoss()
        {
            GeneratorSystem.Instance.SpawnMiniBoss(card);
            EventManager.Instance.DisablePlayer();

            if (moveToCells == CellsType.AggressiveCells)
            {
                moveSystem.MoveRootToAggressive();
            }

            if (moveToCells == CellsType.AttackedCells)
            {
                moveSystem.MoveRootToAttacked();
            }

            OnComplete?.Invoke();
        }
    }
}