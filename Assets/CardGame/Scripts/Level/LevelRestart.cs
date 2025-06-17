using UnityEngine;
using UnityEngine.SceneManagement;

namespace Level
{
    public class LevelRestart : MonoBehaviour
    {
        public void Restart()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}