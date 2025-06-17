using UnityEngine;
using UnityEngine.SceneManagement;

namespace Tutorials
{
    public class TutorialLoad : MonoBehaviour
    {
        [SerializeField] string tutorialLevelName;
        private const string tutorialCompleteKey = "tutorialCompleteKey";
    
        void Start()
        {
            if (PlayerPrefs.GetInt(tutorialCompleteKey) != 1)
                SceneManager.LoadScene(tutorialLevelName);
        }
    
    }
}