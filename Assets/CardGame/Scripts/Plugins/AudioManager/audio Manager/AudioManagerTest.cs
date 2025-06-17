using UnityEngine;

namespace Plugins.AudioManager.audio_Manager
{
    public class AudioManagerTest : MonoBehaviour
    {
#if UNITY_EDITOR

        public AudioClip music;
        public SoundData soundByMouseClick;

        void Start()
        {
            if (music)
            {
                AudioManager.Instance.PlayMusic(music,0.5f,3);
                Debug.Log("Music started", gameObject);
            }
        }


        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (soundByMouseClick)
                {
                    AudioManager.Instance.PlaySound(soundByMouseClick);
                    Debug.Log("Click sound played", gameObject);
                }
            }
        }

#endif
    }
}