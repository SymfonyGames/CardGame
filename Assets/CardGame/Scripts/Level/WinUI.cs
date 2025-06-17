using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Managers;
using Plugins.AudioManager.audio_Manager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;

namespace Level
{
    public class WinUI : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float starsDelay;
        [SerializeField] float starsFrequency;
        [SerializeField] SoundData starSound;
        [Header("Rewards")]
        [SerializeField] RewardItem rewardGold;
        [SerializeField] RewardItem rewardGem;
        [SerializeField] RewardItem rewardGift;
        [Header("Stars")]
        [SerializeField] List<GameObject> stars = new();
        [SerializeField] List<GameObject> vfxs = new();
        [HideInInspector] public LevelWin winScript;
        [SerializeField] Button claimButton;
        [SerializeField] Button claimButtonADS;
        [SerializeField] TextMeshProUGUI heartTxt;
        [SerializeField] Image heartImg;
        public RewardItem RewardGoldItem => rewardGold;
        public RewardItem RewardGemItem => rewardGem;
        public RewardItem RewardGiftItem => rewardGift;

        public void SetHeart(int amount, int max)
        {
            var last = (float) (amount - 1) / max;
            var to = (float) amount / max;
            //    heartImg.fillAmount = to;
            DOVirtual.Float(last, to, 1, v => { heartImg.fillAmount = v; });
            heartTxt.text = amount + "/" + max;
        }

        void Awake()
        {
            foreach (var item in stars)
                item.SetActive(false);

            foreach (var item in vfxs)
                item.SetActive(false);

            //      if (AdsManager.Instance)
            //       AdsManager.Instance.OnRewardedComplete += ClaimADS;
        }

        void OnDisable()
        {
            //  if (AdsManager.Instance)
            //      AdsManager.Instance.OnRewardedComplete -= ClaimADS;
        }

        public void Show()
        {
            gameObject.SetActive(true);
            if (!AdsManager.Instance.isRewardedReady)
                claimButtonADS.gameObject.SetActive((false));
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public float adsMultiplier = 3;

        public event Action OnAdsClaim = delegate { };

        public void ClaimADS()
        {
            if (!AdsManager.Instance.isRewardedReady) return;
            AdsManager.Instance.OnRewardedComplete += GetReward;
            AdsManager.Instance.ShowRewarded();
        }

        void GetReward()
        {
            AdsManager.Instance.OnRewardedComplete -= GetReward;
            claimButton.gameObject.SetActive(false);
            claimButtonADS.gameObject.SetActive(false);
            winScript.ClaimRewards(adsMultiplier);
            OnAdsClaim();
        }

        public void Claim()
        {
            claimButton.gameObject.SetActive(false);
            claimButtonADS.gameObject.SetActive(false);
            winScript.ClaimRewards(1);
        }

        public void RewardGold(int count)
        {
            if (count != 0)
                rewardGold.EnableReward(count);
        }

        public void RewardGem(int count)
        {
            if (count != 0)
                rewardGem.EnableReward(count);
        }

        public void RewardGift(int count)
        {
            rewardGift.EnableReward(count);
        }

        public void RewardStars(int count)
        {
            StartCoroutine(ActivateStars(count));
        }

        IEnumerator ActivateStars(int count)
        {
            yield return new WaitForSeconds(starsDelay);

            if (count > 3) count = 3;
            for (int i = 0; i < count; i++)
            {
                stars[i].SetActive(true);
                vfxs[i].SetActive(true);
                AudioManager.Instance.PlaySound(starSound);
                yield return new WaitForSeconds(starsFrequency);
            }
        }
    }
}