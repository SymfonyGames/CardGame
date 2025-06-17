using UnityEngine;

namespace Plugins.App
{
    public class AndroidSettings : MonoBehaviour
    {

        [SerializeField] bool neverSleepScreen = true;
        [SerializeField] bool androidBackButtonQuit = true;

        void Start()
        {
            if (neverSleepScreen) Screen.sleepTimeout = SleepTimeout.NeverSleep;

            if (!androidBackButtonQuit) gameObject.SetActive(false);
        }

        void Update()
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                if (Input.GetKey(KeyCode.Escape))
                {
                    Application.Quit();
                }
            }
        }
    }
}
