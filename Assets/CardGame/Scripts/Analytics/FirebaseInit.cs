using Firebase;
using Firebase.Analytics;
using UnityEngine;

namespace Analytics
{
    public class FirebaseInit : MonoBehaviour
    {

        void Start()
        {
            DontDestroyOnLoad(this.gameObject);
            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
            {
                FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
            });
        }
    }
}