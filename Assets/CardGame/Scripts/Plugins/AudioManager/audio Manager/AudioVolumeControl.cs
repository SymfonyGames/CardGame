using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Plugins.AudioManager.audio_Manager
{
    public class AudioVolumeControl : MonoBehaviour
    {
        [SerializeField] AudioMixerGroup mixer;
        [SerializeField] Slider sfx;
        [SerializeField] Slider music;
        [SerializeField] SoundData sfxSample;
        const string KEY = "AudioVolumeControl"; //MUSIC
        const string FIRST_TIME = AudioManagerKeys.FIRST_TIME_LAUNCH;
        bool soundReady;
    
        void Awake()
        {
            var firstLaunch = !PlayerPrefs.HasKey(FIRST_TIME + KEY);
            if (!firstLaunch)
            {
           
                sfx.value = PlayerPrefs.GetFloat("VolumeSFX");
                music.value = PlayerPrefs.GetFloat("VolumeMusic");
            }
            else
            {
                PlayerPrefs.SetInt(FIRST_TIME+KEY,1);
                sfx.value = 1;
                music.value = 1;
            }

            StartCoroutine(MakeSomeMagic());
        }



        IEnumerator MakeSomeMagic()
        {
            soundReady = false;
            yield return new WaitForSeconds(1f);
            soundReady = true;
        }

        void Start()
        {
            ChangeSFXvolume(sfx.value);
            ChangeMusicVolume(music.value);
        }

        public void ChangeSFXvolume(float volume)
        {
            var mixerVolume = volume != 0 ? Mathf.Log10(volume) * 20f : -80f;
            mixer.audioMixer.SetFloat("VolumeSFX", mixerVolume);

            PlayerPrefs.SetFloat("VolumeSFX", volume);

            if (soundReady)
                AudioManager.Instance.PlaySound(sfxSample);
        }

        public void ChangeMusicVolume(float volume)
        {
            var mixerVolume = volume != 0 ? Mathf.Log10(volume) * 20f : -80f;
            mixer.audioMixer.SetFloat("VolumeMusic", mixerVolume);

            PlayerPrefs.SetFloat("VolumeMusic", volume);
        }
    }
}