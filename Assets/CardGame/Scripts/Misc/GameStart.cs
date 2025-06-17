using Managers;
using UnityEngine;

namespace Misc
{
    public class GameStart : MonoBehaviour
    {
        [Header("Setup")]
        [SerializeField] MoveSystem moveSystem;
        GeneratorSystem generator;

        void Start()
        {
            generator = GeneratorSystem.Instance;
            Invoke(nameof(Create), 0.3f);
        }
    
        void Create()
        {
            CreateHero();

            moveSystem.MoveAllAtStart();
            CreateCards();

            moveSystem.MoveAllAtStart();
            CreateCards();

            moveSystem.MoveAllAtStart();
            CreateCards();
        }

        void CreateHero() => generator.SpawnHero();
        void CreateCards() => generator.SpawnRandomCards();
    }
}