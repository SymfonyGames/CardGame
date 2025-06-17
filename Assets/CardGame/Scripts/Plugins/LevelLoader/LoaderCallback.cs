using UnityEngine;

namespace Plugins.LevelLoader
{
    public class LoaderCallback : MonoBehaviour
    {
        bool isFirstUpdate=true;

        void Update()
        {
            if (isFirstUpdate)
            {
                isFirstUpdate = false;
                LoaderEngine.LoaderCallback();
            }
        }
    }
}
