using DataSave;
using Managers;
using Maps;
using Plugins.AudioManager.audio_Manager;
using Plugins.LevelLoader;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Level
{
    public class LevelWin : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float loadDelay;
        [SerializeField] int coins;
        [SerializeField] int _totalLines;
        [SerializeField] LevelProgress levelProgress;
        [SerializeField] SoundData winSound;

        [Header("Setup")]
        [SerializeField] WinUI winWindow;
        [SerializeField] Map map;
        [SerializeField] bool thisIsTutorialLevel;
        [SerializeField] bool openMapAfterWin = true;

        bool onceTimedClaim;
        int _goldReward, _gemReward;

        void Awake()
        {
            winWindow.Hide();
        }

        void Start()
        {
            levelProgress.OnProgressChanged += CheckCondition;
        }

        void CheckCondition()
        {
            if (levelProgress.IsProgressComplete)
                Win();
        }

        public void Win()
        {
            EventManager.Instance.EnableFocusCam();
            EventManager.Instance.LevelWin();

            winWindow.winScript = this;
            winWindow.Show();

            var lvlID = SceneManager.GetActiveScene().buildIndex;
            var from = 2;
            var to = 11;
            var max = to - from + 1;
            var cur = lvlID - from + 1;

            winWindow.SetHeart(cur, max);

            Debug.LogWarning("TODO: setup rewards from SO");
            winWindow.RewardStars(3);

            GoldReward(Loader.Instance.GetGoldReward());
            GemReward(Loader.Instance.GetGemReward());

            AudioManager.Instance.StopMusic(); //stop LevelBackgroundMusic if Win
            AudioManager.Instance.PlaySound(winSound);

            if (thisIsTutorialLevel)
                PlayerPrefs.SetInt(SaveKeys.TUTORIAL_COMPLETE, 1);
        }


        public void ClaimRewards(float mult)
        {
            if (onceTimedClaim) return;
            onceTimedClaim = true;

            if (_goldReward > 0)
                EventManager.Instance.CreateGoldVFX(winWindow.RewardGoldItem.transform.position,
                    (int) (_goldReward * mult));

            if (_gemReward > 0)
                EventManager.Instance.GemCreate(winWindow.RewardGemItem.transform.localPosition,
                    (int) (_gemReward * mult));

            if (openMapAfterWin)
            {
                if (thisIsTutorialLevel)
                    map.ShowAfterTutorial();
                else
                    Invoke(nameof(OpenMap), loadDelay);
            }
            else
            {
                Invoke(nameof(BackToMainMenu), loadDelay);
            }
        }


        void GoldReward(int value)
        {
            winWindow.RewardGold(value);
            _goldReward = value;
        }


        void GemReward(int value)
        {
            winWindow.RewardGem(value);
            _gemReward = value;
        }

 
        void BackToMainMenu() => Loader.Instance.LoadMainMenu();
        void OpenMap() => map.ShowAtLevelComplete();
    }
}