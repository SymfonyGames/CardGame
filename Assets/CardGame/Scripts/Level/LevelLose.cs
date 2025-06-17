using System.Collections;
using Managers;
using Misc;
using Player;
using Plugins.AudioManager.audio_Manager;
using UnityEngine;

namespace Level
{
    public class LevelLose : MonoBehaviour
    {
        [Header("Settings")] [SerializeField] float delayExitButton = 2;
        [Header("Setup")] [SerializeField] LoseUI loseWindow;
        [SerializeField] GameConfig gameConfig;
        [SerializeField] ExitToMainMenu exitToMenu;
        [SerializeField] SoundData loseSound;

        void Awake()
        {
            loseWindow.Init(this, gameConfig.Config.ReviveCost);
            loseWindow.gameObject.SetActive(false);
        }

        void Start()
        {
            EventManager.Instance.OnPlayerDeath += PlayerDead;
            AdsManager.Instance.OnRewardedComplete += DoRevive;
        }

        void OnDisable()
        {
            EventManager.Instance.OnPlayerDeath -= PlayerDead;
            AdsManager.Instance.OnRewardedComplete -= DoRevive;
        }

        void PlayerDead()
        {
            Lose();
        }

        public void Lose()
        {
            loseWindow.Show();
            EnableFocusCamera();
            StartCoroutine(ShowExit());
            EventManager.Instance.LevelLose();
            AudioManager.Instance.StopMusic();
            AudioManager.Instance.PlaySound(loseSound);
        }


        IEnumerator ShowExit()
        {
            loseWindow.HideExit();
            yield return new WaitForSeconds(delayExitButton);
            loseWindow.ShowExit();
        }

        public void ReviveADS()
        {
            AdsManager.Instance.ShowRewarded();
            DoRevive();
        }

        public void Revive()
        {
            PlayerStash_resources.Instance.RemoveGem(gameConfig.Config.ReviveCost.gems);
            PlayerStash_resources.Instance.RemoveGold(gameConfig.Config.ReviveCost.gold);
            DoRevive();
        }

        void DoRevive()
        {
            //loseWindow.HideRevive();
            loseWindow.Hide();
            LevelTheme.Instance.PlayMusicTheme();
            EventManager.Instance.PlayerRevive();
        }

        public void ExitToMainMenu() => exitToMenu.Exit();
        static void EnableFocusCamera() => EventManager.Instance.EnableFocusCam();
    }
}