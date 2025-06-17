using Plugins.AudioManager.audio_Manager;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Plugins.AudioManager.audio_options
{
    public class OptionsToogleSfx : MonoBehaviour
    {
        [Header("Mixer group")]
        public AudioMixerGroup mixer;

        [Header("Setup icon")]
        public Image img;

        bool isActive;
        const string key = AudioManagerKeys.MIXER_VOLUME_SFX; //SFX
        const string firstTime = AudioManagerKeys.FIRST_TIME_LAUNCH;

        void Start()
        {
            bool first = !PlayerPrefs.HasKey(firstTime + key);
            if (first) PlayerPrefs.SetInt(firstTime + key, 1);
            isActive = first || PlayerPrefs.GetFloat(key) == 1;

            ToggleVolume(isActive);
        }


        public void Toggle()
        {
            isActive = !isActive;
            ToggleVolume(isActive);
        }
        void ToggleVolume(bool act)
        {
            MixerSetupVolume(act);
            SaveData(act);
            ToogleImageColor(act);
        }


        void SaveData(bool act) => PlayerPrefs.SetFloat(key, act ? 1 : 0);
        void MixerSetupVolume(bool act) => mixer.audioMixer.SetFloat(key, act ? 0 : -80f);
        void ToogleImageColor(bool act)
        {
            Color clr = img.color;
            clr.a = act ? 1 : 0.5f;
            img.color = clr;
        }
    }
}
