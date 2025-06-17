using DG.Tweening;
using Managers;
using Misc;
using Plugins.AudioManager.audio_Manager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    public class PlayerHealButton : MonoBehaviour
    {
        [SerializeField] int healAmount;
        [SerializeField] int goldCost;
        [SerializeField] int goldCostIncrement;
        [SerializeField] TextMeshProUGUI goldCostText;
        [SerializeField] private TextMeshProUGUI healamountText;
        [SerializeField] Image goldCostImage;
        [SerializeField] private Image glowImage;
        [SerializeField] private Image disactiveImage;
 
        [SerializeField] SoundData healSound;
        [SerializeField] ConfigData config;

        [Header("Animation")]
        public Button healButton;
        public float increaseSize = 1.1f;
        public float animTime = 0.75f;
        [Header("Editor Test")]
        public bool testAnim;
        public bool stopAnim;
        void Start()
        {
            healamountText.text = healAmount.ToString();
            EventManager.Instance.OnResourcesChanged += ResrouceChanged;
            EventManager.Instance.OnPlayerHitpointsChanged += PlayerHPchanged;
            RefreshText();

            Invoke(nameof(RefreshButton), 0.1f);
        }

        void RefreshButton()
        {
            if (PlayerStash_resources.Instance.Gold >= goldCost)
            {
                healButton.interactable = true;
                disactiveImage.enabled = false;
            }
            else
            {
                healButton.interactable = false;
                disactiveImage.enabled = true;
            }

            float a = PlayerStash_resources.Instance.Gold >= goldCost ? 1 : 0.5f;
            goldCostImage.color = new Color(1, 1, 1, a);
        }

        private void ResrouceChanged()
        {
            RefreshButton();
        }

        public void Heal()
        {
            if (goldCost <= PlayerStash_resources.Instance.Gold)
            {
                EventManager.Instance.PlayerHeal(healAmount);
                PlayerStash_resources.Instance.RemoveGold(goldCost);

                goldCost += goldCostIncrement;
                PlaySound(healSound);
                RefreshText();
            }
        }

        void RefreshText() => goldCostText.text = goldCost.ToString();

        void PlaySound(SoundData sound) => AudioManager.Instance.PlaySound(sound);

        void PlayHealAnimation()
        {
            glowImage.DOColor(Color.red, animTime).SetLoops(-1, LoopType.Yoyo);
            healButton.transform.DOScale(1.1f, animTime).SetLoops(-1, LoopType.Yoyo);
        }
        void StopHealAnimation()
        {
            glowImage.color = Color.white;
            healButton.transform.localScale = Vector3.one;
            glowImage.DOKill();
        }

        void PlayerHPchanged(float currentHp)
        {
            if (currentHp <= config.HpMinHealAnimation &&
                goldCost <= PlayerStash_resources.Instance.Gold)
            {
                PlayHealAnimation();
            }
            else
            {
                StopHealAnimation();
            }
        }


#if UNITY_EDITOR
        void Update()
        {
            if (testAnim)
            {
                testAnim = false;
                PlayHealAnimation();
            }
            if (stopAnim)
            {
                stopAnim = false;
                StopHealAnimation();
            }
        }
#endif
    }
}
