
using DG.Tweening;
using Level;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Plugins.LevelLoader
{
    [ExecuteInEditMode]
    public class Loader : MonoBehaviour
    {
        #region Singleton

        //-------------------------------------------------------------
        public static Loader Instance;



        void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                gameObject.SetActive(false);
        }

        //-------------------------------------------------------------

        #endregion

        [Header("Load scene")] bool useLoadScene;

        [Header("Current Level")]
        [SerializeField] int currentLevel;

        [Header("MainLevels")]
        [SerializeField] private LevelData mainMenu;
        [SerializeField] private LevelData heroSelect;
        [Header("Levels")]
        [SerializeField] private LevelSequenceData currentSequence;
        public LevelSequenceData CurrentSequence => currentSequence;
        public int CurrentLevel => currentLevel;
    
    

        void Start()
        {
            if (Application.isPlaying)
            {
                transform.parent = null;
                DontDestroyOnLoad(gameObject);
            }
        }

        public void LoadMainMenu()
        {
            DOTween.KillAll();
            currentLevel = 0;
            LoaderEngine.Load(mainMenu.LevelName, useLoadScene);
        }

        public void LoadHeroSelect()
        {
            DOTween.KillAll();
            currentLevel = 0;
            LoaderEngine.Load(heroSelect.LevelName, useLoadScene);
        }

        public void LoadNextLevel()
        {
            DOTween.KillAll();
            currentLevel = CurrentLevel + 1;
            var levelID = CurrentLevel - 1;

            if (levelID < CurrentSequence.Count)
            {
                var sceneName = CurrentSequence.GetLevelName(levelID);
                LoaderEngine.Load(sceneName, useLoadScene);
            }
            else
            {
                Debug.LogError("There's no more level to load in list");
            }
        }


        public void LoadLevel(string levelName)
        {
            DOTween.KillAll();
            LoaderEngine.Load(levelName, useLoadScene);
        }

        public void LoadLevel(LevelSequenceData sequence, int levelId)
        {
            DOTween.KillAll();

            currentSequence = sequence;

            var levelName = CurrentSequence.GetLevelName(levelId);

            if (string.IsNullOrEmpty(levelName))
            {
                Debug.LogError("There's no scene with such ID in data");
                return;
            }

            currentLevel = levelId + 1;
            LoaderEngine.Load(levelName, useLoadScene);
        }

    
    
        public int GetTotalEnemyLines()
        {
            var id = CurrentLevel - 1;
            if (currentSequence == null)
            {
                Debug.LogWarning("Loader sequence has not been set");
                return 3;
            }
            return id < 0 ? 3 : CurrentSequence.GetTotalEnemyLines(id);
        }

        public int GetGoldReward()
        {
            var id = CurrentLevel - 1;
            if (currentSequence == null)
            {
                Debug.LogWarning("Loader sequence has not been set");
                return 0;
            }
        
            return id < 0 ? 100 : CurrentSequence.GoldReward(id);
        }

        public int GetGemReward()
        {
            var id = CurrentLevel - 1;
            if (currentSequence == null)
            {
                Debug.LogWarning("Loader sequence has not been set");
                return 0;
            }
            return id < 0 ? 1 : CurrentSequence.GemReward(id);
        }

        #region Editor mode

#if UNITY_EDITOR

        void Update()
        {
            if (Application.isPlaying) return;
            FindCurrentScene();
        }

        void FindCurrentScene()
        {
            var currentSceneName = SceneManager.GetActiveScene().name;
            if (!CurrentSequence) return;
        
            for (var i = 0; i < CurrentSequence.Count; i++)
            {
                if (currentSceneName.Equals(CurrentSequence.GetLevelName(i)))
                {
                    var id = i + 1;
                    if (CurrentLevel.Equals(id)) return;

                    currentLevel = id;
                    EditorUtility.SetDirty(gameObject);
                    return;
                }
            }
        }

#endif

        #endregion
    
    }
}