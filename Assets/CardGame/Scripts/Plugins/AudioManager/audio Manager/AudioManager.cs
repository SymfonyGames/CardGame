using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Plugins.AudioManager.audio_Manager
{
    [RequireComponent(typeof(AudioManagerPool))]
    public class AudioManager : MonoBehaviour
    {
        #region Singleton

        public static AudioManager Instance;

        #endregion

        #region Init

        [Header("Mixers")] [SerializeField] AudioMixerGroup musicMixerGroup;
        [SerializeField] AudioMixerGroup sfxMixerGroup;

        AudioManagerPool pool;
        AudioSource musicSource;

        List<AudioClip> frequencyList = new List<AudioClip>();

        #endregion


        void Awake()
        {
            musicSource = gameObject.AddComponent<AudioSource>();
            musicSource.loop = true;
            musicSource.outputAudioMixerGroup = musicMixerGroup;

            pool = GetComponent<AudioManagerPool>();
            pool.SetSfxMixer(sfxMixerGroup);

            if (Instance == null)
            {
                Instance = this;
                if (transform.parent) transform.SetParent(null);
                DontDestroyOnLoad(gameObject);
                Debug.Log("<color=yellow>Audio Manager</color> инициализирован и сделан DontDestroyOnLoad", gameObject);
            }
            else
            {
                Debug.Log("несколько Audio Manager на сцене",
                    gameObject);
                gameObject.SetActive(false);
            }
        }
        public void PlayMusic(SoundData musicData, float vol = 1, float transitionTime = 1)
        {
            if (musicData == null)
            {
                Debug.Log("MusicData = null");
                return;
            }
            StartCoroutine(UpdateMusicWithFade(musicData.GetClip(), vol, transitionTime));
        }
        public void PlayMusic(AudioClip musicClip, float vol = 1, float transitionTime = 1)
        {
            if (musicClip == null)
            {
                Debug.Log("MusicClip = null");
                return;
            }
            StartCoroutine(UpdateMusicWithFade(musicClip, vol, transitionTime));
        }
        public void StopMusic() => StartCoroutine(StopMusicFade());

        IEnumerator StopMusicFade()
        {
            float volume = musicSource.volume;
            while (volume > 0)
            {
                volume -= Time.deltaTime;
                musicSource.volume = volume;
                yield return null;
            }

            musicSource.Stop();
        }
        IEnumerator UpdateMusicWithFade(AudioClip music, float musicVolume, float transitionTime)
        {
            float volume;
            if (musicSource.isPlaying)
            {
                //FADE MUSIC
                volume = musicSource.volume;
                while (volume > 0)
                {
                    volume -= Time.deltaTime;
                    musicSource.volume = volume;
                    yield return null;
                }

                musicSource.Stop();
            }

            musicSource.clip = music;
            musicSource.Play();

            volume = 0f;
            while (volume < musicVolume)
            {
                //INCREASE VOLUME
                volume += Time.deltaTime / transitionTime;
                musicSource.volume = volume;
                yield return null;
            }

            musicSource.volume = musicVolume;
        }

        public void PlaySound(SoundData sound)
        {
            if (!sound) return;
            var src = pool.GetSource();
            src.spatialBlend = 0;
            sound.Play(src);
        }
        public void PlaySound(SoundData sound,float delay)
        {
            if (!sound) return;
            StartCoroutine(PlaySoundDelayed(sound, delay));
        }

        IEnumerator PlaySoundDelayed(SoundData sound,float delay)
        {
            yield return new WaitForSeconds(delay);
            PlaySound(sound);
        }

        public void PlaySound3D(SoundData sound, Vector3 soundPosition)
        {
            if (!sound) return;
            var src = pool.GetSource();
            src.spatialBlend = 1;
            src.transform.position = soundPosition;
            sound.Play(src);
        }
        public void PlayAudioClip(AudioClip clip, float volume = 1)
        {
            var src = pool.GetSource();
            src.clip = clip;
            src.volume = volume;
            src.spatialBlend = 0;
            src.Play();
        }

        public bool ClipIsBlocked(AudioClip clip, float sfxFreq)
        {
            if (frequencyList.Contains(clip))
                return true;
            else
            {
                StartCoroutine(FreqListJob(clip, sfxFreq));
                return false;
            }
        }
        IEnumerator FreqListJob(AudioClip clip, float freq)
        {
            frequencyList.Add(clip);
            yield return new WaitForSeconds(freq);
            frequencyList.Remove(clip);
        }
    
    }
}