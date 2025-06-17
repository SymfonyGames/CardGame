using Firebase.Analytics;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Analytics
{
    public class FirebaseLevelAnalytics : MonoBehaviour
    {
        int _sceneIndex;
        string _sceneName;

        void Start()
        {
            var activeScene = SceneManager.GetActiveScene();
            _sceneIndex = activeScene.buildIndex;
            _sceneName = activeScene.name;

            FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventLevelStart,
                new Parameter (FirebaseAnalytics.ParameterLevel, _sceneIndex),
                new Parameter (FirebaseAnalytics.ParameterLevelName, _sceneName));
        }

        void OnDestroy()
        {
            FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventLevelEnd,
                new Parameter(FirebaseAnalytics.ParameterLevel, _sceneIndex),
                new Parameter(FirebaseAnalytics.ParameterLevelName, _sceneName));
        }
    }
}
