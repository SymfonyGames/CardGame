using Plugins.LevelLoader;
using UnityEngine;

namespace TESTS
{
    public class TEST_LOADLEVELS : MonoBehaviour
    {
        void Start()
        {
            DontDestroyOnLoad(gameObject);   
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Loader.Instance.LoadNextLevel();
            }
        }
    }
}
