using UnityEngine;

namespace Level
{
    public class LevelMusic : MonoBehaviour
    {
        public AudioSource _audioSource;
        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        public void StopAudioSource()
        {
            if (_audioSource.isPlaying)
            {
                _audioSource.Stop();
            }
        }
        /*
    LevelTheme levelTheme;
    private bool isPlaying;
    void Start()
    {
        levelTheme = LevelTheme.Instance;
        isPlaying = false;
    }
    public void PlaySound()
    {
        isPlaying = true;
        AudioManager.Instance.PlaySound(levelTheme.Data.Theme.BackgroundMusic);
    }

    private void Update()
    {
        if (expr)
        {
            
        }
        {
            PlaySound();
        }
    }
    */
    }
}
