using Plugins.AudioManager.audio_Manager;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Plugins.AudioManager.audio_options
{
    public class OptionsToogleMusic : MonoBehaviour
    {

        [Header("Mixer group")]
        public AudioMixerGroup mixer;

        [Header("Setup icon")]
        public Image img;

        bool isActive;
        const string KEY = AudioManagerKeys.MIXER_VOLUME_MUSIC; //MUSIC
        const string FIRST_TIME = AudioManagerKeys.FIRST_TIME_LAUNCH;

        void Start()
        {
            bool first = !PlayerPrefs.HasKey(FIRST_TIME + KEY);
        
            if (first) PlayerPrefs.SetInt(FIRST_TIME + KEY, 1);
            isActive = first || PlayerPrefs.GetFloat(KEY) == 1;
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


        void SaveData(bool act) => PlayerPrefs.SetFloat(KEY, act ? 1 : 0);
        void MixerSetupVolume(bool act) => mixer.audioMixer.SetFloat(KEY, act ? 0 : -80f);
        void ToogleImageColor(bool act)
        {
            var clr = img.color;
            clr.a = act ? 1 : 0.5f;
            img.color = clr;
        }
    
    }
}
