using Firebase.Analytics;
using Level;
using Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Analytics
{
    public class AdsAnalytics : MonoBehaviour
    {
        public WinUI win;
        void Start()
        {
            win.OnAdsClaim += Send;
        }

        void OnDisable()
        {
            win.OnAdsClaim -= Send;
        }

        void Send()
        {
            var activeScene = SceneManager.GetActiveScene();
            var _sceneName = activeScene.name;

            FirebaseAnalytics.LogEvent("Win ADS claim",
                new Parameter("Win ADS claim x3", _sceneName));
        }
   
    }
}
