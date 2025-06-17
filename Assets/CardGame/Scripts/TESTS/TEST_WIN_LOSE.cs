using Level;
using UnityEngine;

namespace TESTS
{
    public class TEST_WIN_LOSE : MonoBehaviour
    {
        [Header("Test")]
        [SerializeField] bool win;
        [SerializeField] bool lose;
        [Header("Setup")]
        [SerializeField] private LevelWin levelWin;
        [SerializeField] private LevelLose levelLose;

#if UNITY_EDITOR
        void Update()
        {
            if (win)
            {
                win = false;
                levelWin.Win();
            }

            if (lose)
            {
                lose = false;
                levelLose.Lose();
            }
        }
#endif
    }
}