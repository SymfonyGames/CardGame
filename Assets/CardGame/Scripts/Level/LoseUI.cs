using Managers;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Level
{
    public class LoseUI : MonoBehaviour
    {
        [Header("Texts")]
        [SerializeField] TextMeshProUGUI goldTxt;
        [SerializeField] TextMeshProUGUI gemTxt;
        //  [SerializeField] TextMeshProUGUI reviveLabel;

        [Header("Components")]
        public TextMeshProUGUI reviveHint;
        public GameObject popup;

        [Header("Buttons")]
        [SerializeField] Button watchButton;

        [SerializeField] Button exitButton;
        [SerializeField] GameObject gemCont;
        [SerializeField] GameObject goldCont;

        LevelLose _loseController;
        ResourceData _reviveCost;

        public void Init(LevelLose controller, ResourceData reviveCost)
        {
            _loseController = controller;
            _reviveCost = reviveCost;
            RefreshText(reviveCost);
        }

        public void Show()
        {
            RefreshButtons(_reviveCost);
            gameObject.SetActive(true);
        }

        public GameObject revive;

        void RefreshButtons(ResourceData reviveCost)
        {
            var gold = PlayerStash_resources.Instance.Gold;
            var gem = PlayerStash_resources.Instance.Gold;
            var goldPrice = reviveCost.gold;
            var gemPrice = reviveCost.gems;

            if (gold < goldPrice || gold == 0) goldCont.gameObject.SetActive(false);
            if (gem < gemPrice || gemPrice == 0) gemCont.gameObject.SetActive(false);
            if (gold < goldPrice || gem < gemPrice) revive.SetActive(false);

            watchButton.gameObject.SetActive(AdsManager.Instance.isRewardedReady);
        }

        void RefreshText(ResourceData reviveCost)
        {
            goldTxt.text = reviveCost.gold.ToString();
            gemTxt.text = reviveCost.gems.ToString();
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void ReviveADS() => _loseController.ReviveADS();
        public void Revive() => _loseController.Revive();
        public void ExitToMainMenu() => _loseController.ExitToMainMenu();

        public void HideExit() => exitButton.gameObject.SetActive(false);
        public void ShowExit() => exitButton.gameObject.SetActive(true);


        public void HideRevive()
        {
            popup.gameObject.SetActive(false);
        }
    }
}