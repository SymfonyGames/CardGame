using System;
using System.Collections.Generic;
using Managers;
using UnityEngine;

namespace Tutorials.items
{
    public class TutorialSpawnArtifacts : MonoBehaviour, ITutorialItem
    {
        [SerializeField] [Range(0.3f, 5f)] private float createDelay;
        [SerializeField] MoveSystem moveSystem;
        [SerializeField] CellsType moveToCells;
        [Header("Cards")]
        [SerializeField] private CardDataArtifact leftCard;
        [SerializeField] private CardDataArtifact midCard;
        [SerializeField] private CardDataArtifact rightCard;

        public float Delay => createDelay;

        public event Action OnComplete;

        public void Init(TutorialConfig tutorial)
        {
        }

        public void StartItem()
        {
            SpawnLine();
        }

        void SpawnLine()
        {
            var creatures = new List<CardDataArtifact>
            {
                leftCard, midCard, rightCard
            };

            GeneratorSystem.Instance.SpawnArtifacts(creatures);

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