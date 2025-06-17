using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Plugins.LevelLoader
{
    public static class LoaderEngine
    {
        class LoadingMonoBehaviour : MonoBehaviour { }

        static Action onLoaderCallback;
        static AsyncOperation loadingAsyncOperation;
 
        public static void Load(string scene, bool useLoadScene)
        {
            if (useLoadScene)
            {
                //Set the loader callback action to load the target scene
                onLoaderCallback = () =>
                {
                    GameObject loadingGameobject = new GameObject("Loading Game Object");
                    loadingGameobject.AddComponent<LoadingMonoBehaviour>().StartCoroutine(LoadSceneAsync(scene));

                };
            
                SceneManager.LoadScene("LoaderLoading");
                Debug.LogError("Put loading scene name here");
            }
            else
            {
                SceneManager.LoadScene(scene);
            }

        }


        static IEnumerator LoadSceneAsync(string scene)
        {
            yield return null;

            loadingAsyncOperation = SceneManager.LoadSceneAsync(scene.ToString());

            while (!loadingAsyncOperation.isDone)
            {
                yield return null;
            }
        }


        public static float GetLoadingProgress()
        {
            if (loadingAsyncOperation != null)
            {
                float progress = Mathf.Clamp01(loadingAsyncOperation.progress / .9f);
                return progress;
            }
            else
                return 0f;
        }

        public static void LoaderCallback()
        {
            //Trigered after the first update withc lest the screen refresh
            //Execute the loader callback action wich will load the target 
            if (onLoaderCallback != null)
            {
                onLoaderCallback();
                onLoaderCallback = null;
            }
        }
    }
}
