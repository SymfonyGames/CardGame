using Managers;
using UnityEngine;

namespace Misc
{
    public class Cell : MonoBehaviour
    {
        InteractSystem _interactSystem;
        [SerializeField] Cell nextCell;
        [SerializeField] Cell attackingCells;
        [SerializeField] Cell[] attackCells;

        void Start()
        {
            _interactSystem = InteractSystem.Instance;
        }

        public InteractSystem InteractSystem => _interactSystem;
        public Cell NextCell => nextCell;
        public Cell AttackingCell => attackingCells;
        public Cell[] AttackCells => attackCells;
        public Card Card { get; set; }

        public Vector3 Position => transform.position;


        public bool Empty => !Card;
    }
}