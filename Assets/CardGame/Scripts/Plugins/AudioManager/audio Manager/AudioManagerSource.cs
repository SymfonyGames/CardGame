using System.Collections;
using UnityEngine;

namespace Plugins.AudioManager.audio_Manager
{
    public class AudioManagerSource : MonoBehaviour
    {
        public AudioSource audioSource;
        public void Init(AudioManagerPool pool) => myPool = pool;
        AudioManagerPool myPool;
        void OnEnable() => StartCoroutine(Kostil());

        IEnumerator Kostil()
        {
            yield return new WaitForEndOfFrame();
            AudioClip clp = audioSource.clip;
            float delay;
            if (clp == null)
                delay = 1f;
            else
                delay = audioSource.clip.length / audioSource.pitch;
            yield return new WaitForSeconds(delay * Time.timeScale);
            gameObject.SetActive(false);
            myPool.ReturnToPool(audioSource);
        }
    }
}