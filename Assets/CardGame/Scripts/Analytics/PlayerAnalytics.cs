using Firebase.Analytics;
using Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Analytics
{
    public class PlayerAnalytics : MonoBehaviour
    {
        void Start()
        {
            EventManager.Instance.OnPlayerDeath += SendDead;
            EventManager.Instance.OnPlayerRevive += SendRevive;
        }

        void OnDisable()
        {
            if (EventManager.Instance) EventManager.Instance.OnPlayerDeath -= SendDead;
            if (EventManager.Instance) EventManager.Instance.OnPlayerRevive -= SendRevive;
        }

        void SendRevive()
        {
            var activeScene = SceneManager.GetActiveScene();
            var _sceneName = activeScene.name;

            FirebaseAnalytics.LogEvent("Revive",
                new Parameter("Player Revive", _sceneName));
        }

        void SendDead()
        {
            var activeScene = SceneManager.GetActiveScene();
            var _sceneName = activeScene.name;

            FirebaseAnalytics.LogEvent("Lose",
                new Parameter("Player Death", _sceneName));
        }
    }
}