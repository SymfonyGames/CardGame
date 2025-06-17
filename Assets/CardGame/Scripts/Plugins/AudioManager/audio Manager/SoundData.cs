using Plugins.AudioManager.ranged_float;
using UnityEngine;

namespace Plugins.AudioManager.audio_Manager
{
    [CreateAssetMenu(menuName = "Audio Manager/Sound")]
    public class SoundData : ScriptableObject
    {
        public AudioClip[] sounds;
        [Header("Settings")] public RangedFloat volume;
        [MinMaxRange(0, 3)] public RangedFloat pitch;
        [SerializeField] float maximumFrequency = 0f;
        bool SoundIsBlocked(int id) => AudioManager.Instance.ClipIsBlocked(sounds[id], maximumFrequency);

        public AudioClip GetClip()
        {
            if (sounds.Length == 0)
            {
                return null;
            }
        
            var r = Random.Range(0, sounds.Length);
            return sounds[r];
        }

        public void Play(AudioSource source)
        {
            if (sounds.Length == 0) return;
            var id = Random.Range(0, sounds.Length);
            if (SoundIsBlocked(id)) return;

            source.volume = Random.Range(volume.minValue, volume.maxValue);
            source.pitch = Random.Range(pitch.minValue, pitch.maxValue);
            source.clip = sounds[id];
            source.Play();
        }

        public void EditorTest(AudioSource source)
        {
            int id = Random.Range(0, sounds.Length);

            source.volume = Random.Range(volume.minValue, volume.maxValue);
            source.pitch = Random.Range(pitch.minValue, pitch.maxValue);
            source.PlayOneShot(sounds[id]);
        }
    

    }
}