using System.Collections.Generic;
using UnityEngine;

namespace BossGame
{
    public class CellsUI : MonoBehaviour
    {
        [SerializeField] List<ActionCell> cells = new();

        public IReadOnlyList<ActionCell> Cells => cells;
    }
}
