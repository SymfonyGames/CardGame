using System;
using DG.Tweening;
using Managers;
using Plugins.AudioManager.audio_Manager;
using TMPro;
using UnityEngine;

namespace Player
{
    public class PlayerCoinUI : MonoBehaviour
    {
        [Header("Text reference")] [SerializeField]
        TextMeshProUGUI goldText;
        [SerializeField] SoundData sound;
        int _amount;
        public bool showOnlySceneAmount;
        int triger;

        void Start()
        {
            if (showOnlySceneAmount)
                triger = PlayerStash_resources.Instance.Gold;

            RefreshText();
            EventManager.Instance.OnResourcesChanged += RefreshText;
        }

        void OnDisable()
        {
            EventManager.Instance.OnResourcesChanged -= RefreshText;
        }

        void RefreshText()
        {
            if (_amount == PlayerStash_resources.Instance.Gold && _amount != 0) return;
            _amount = PlayerStash_resources.Instance.Gold;
            goldText.text = (_amount - triger).ToString();


            if (Time.time < 0.2f) return;

            var animTime = 0.2f;

            goldText.transform.DOScale(1.3f, animTime / 2)
                .OnComplete(() =>
                    goldText.transform.DOScale(1, animTime / 2));

            goldText.DOColor(Color.yellow, 0.3f);
            goldText.DOColor(Color.white, 0.3f).SetDelay(animTime);

            AudioManager.Instance.PlaySound(sound);
        }
    }
}