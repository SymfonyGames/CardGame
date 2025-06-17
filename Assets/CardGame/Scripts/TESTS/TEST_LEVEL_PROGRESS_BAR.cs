using Level;
using UnityEngine;

namespace TESTS
{
    public class TEST_LEVEL_PROGRESS_BAR : MonoBehaviour
    {
        [Header("Test")]
        public bool createdIcon;
        [Header("Setup")]
        public LevelProgressUI progressUI;
        public Sprite testSprite;
        [Range(0, 1)] public float testProgress;


#if UNITY_EDITOR
        private void Update()
        {
            if (createdIcon)
            {
                createdIcon = false;
                Debug.Log("Trying to add icon to progress bar");
                progressUI.AddIcon(testSprite, testProgress);
            }
        }
#endif
    }
}