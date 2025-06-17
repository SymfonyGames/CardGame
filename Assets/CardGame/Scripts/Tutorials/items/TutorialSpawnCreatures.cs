using System;
using System.Collections.Generic;
using Managers;
using UnityEngine;

namespace Tutorials.items
{
    [System.Serializable]
    public enum CellsType
    {
        RootCells,
        AggressiveCells,
        AttackedCells
    }

    public class TutorialSpawnCreatures : MonoBehaviour, ITutorialItem
    {
        [SerializeField] [Range(0.3f, 5f)] float createDelay;
        [SerializeField] MoveSystem moveSystem;
        [SerializeField] CellsType moveToCells;
        [Header("Cards")]
        [SerializeField]
        CardDataCreature leftCard;
        [SerializeField] CardDataCreature midCard;
        [SerializeField] CardDataCreature rightCard;

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
            var creatures = new List<CardDataCreature>
            {
                leftCard, midCard, rightCard
            };

            GeneratorSystem.Instance.SpawnCreatures(creatures);

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