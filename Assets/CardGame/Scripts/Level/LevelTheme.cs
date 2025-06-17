using Plugins.AudioManager.audio_Manager;
using UnityEngine;

namespace Level
{
    [ExecuteInEditMode]
    public class LevelTheme : MonoBehaviour
    {
        #region Singleton

        //-------------------------------------------------------------
        public static LevelTheme Instance;

        void Awake()
        {
            if (Instance == null) Instance = this;
            else gameObject.SetActive(false);
        }

        void OnDisable() => Instance = null;
        //-------------------------------------------------------------

        #endregion

        [SerializeField] private bool autoPlayThemeMusic = true;
        [SerializeField] LevelData data;
        public LevelData Data => data;

        void Start()
        {
            if (!autoPlayThemeMusic) return;
            
            if (Application.isPlaying)
            {
                PlayMusicTheme();
            }
        }

        public void PlayMusicTheme()
        {
            AudioManager.Instance.PlayMusic(data.Theme.BackgroundMusic,0.8f,5);
        }

        #region Editor

        void Update()
        {
            if (!Instance) Instance = this;
        }

        #endregion
    }
}