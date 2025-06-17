using UnityEngine;

namespace TESTS
{
    public class TEST_TIMESPEED : MonoBehaviour
    {
        public bool useCustomTime;
        [Range(0, 10)]
        public float timeSpeed;
        // Start is called before the first frame update
        void Start()
        {
#if UNITY_EDITOR
            if (useCustomTime)
                Time.timeScale = timeSpeed;
#endif
        }


    }
}
