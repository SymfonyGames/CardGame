using Level;
using Managers;
using Plugins.AudioManager.audio_Manager;
using UnityEngine;

namespace Misc
{
    public class OptionsMenu : MonoBehaviour
    {
        [SerializeField] Canvas optionsCanvas;
        [SerializeField] Canvas pauseCanvas;
        [SerializeField] SoundData openSound;
        [SerializeField] SoundData closeSound;

        void Start()
        {
            CloseSettings();
            Continue();
        }

        public void Pause()
        {
            pauseCanvas.gameObject.SetActive(true);
            EventManager.Instance.EnableFocusCam();
            PlaySound(openSound);
            AudioManager.Instance.StopMusic();
        }

        public void Continue()
        {
            pauseCanvas.gameObject.SetActive(false);
            EventManager.Instance.CameraFocusDisable();
            PlaySound(closeSound);
            if (LevelTheme.Instance)
                LevelTheme.Instance.PlayMusicTheme();
        }

        public void OpenSettings()
        {
            if (optionsCanvas)
                optionsCanvas.gameObject.SetActive(true);
            PlaySound(openSound);
        }

        public void CloseSettings()
        {
            if (optionsCanvas)
                optionsCanvas.gameObject.SetActive(false);
            PlaySound(closeSound);
        }

        void PlaySound(SoundData sound) => AudioManager.Instance.PlaySound(sound);
    }
}