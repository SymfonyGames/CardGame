using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Plugins.AudioManager.audio_Manager
{
    public class AudioManagerPool : MonoBehaviour
    {
    
        #region Init

        [SerializeField] AudioManagerSource prefab = null;
        readonly Queue<AudioSource> _myQueue = new Queue<AudioSource>();
        AudioMixerGroup _sfxMixerGroup;
        public void SetSfxMixer(AudioMixerGroup mixer) => _sfxMixerGroup = mixer;

        #region Editor

#if (UNITY_EDITOR)
        int poolLenght = 0;
        string defaultName;
        void Start() => defaultName = name;
#endif

        #endregion

        #endregion

        public AudioSource GetSource()
        {
            if (_myQueue.Count <= 0) return CreateSource();

            var src = _myQueue.Dequeue();
            src.gameObject.SetActive(true);
            return src;
        }

        AudioSource CreateSource()
        {
            var src = Instantiate(prefab, transform);
            src.audioSource.outputAudioMixerGroup = _sfxMixerGroup;
            src.Init(this);

            #region Editor

#if (UNITY_EDITOR)
            //Write number of pooled object to the name of Pool (n).
            poolLenght++;
            gameObject.name = defaultName + " (" + poolLenght + ")";
#endif

            #endregion

            return src.audioSource;
        }

        public void ReturnToPool(AudioSource src)
        {
            _myQueue.Enqueue(src);
            src.gameObject.SetActive(false);
        }
    }
}