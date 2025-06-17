using DG.Tweening;
using Misc;
using Plugins.AudioManager.audio_Manager;
using UnityEngine;
using UnityEngine.UI;

namespace Shop
{
    public class ShopPurchuaseAnimation : MonoBehaviour
    {
        [SerializeField] ShopData shopData;
        [SerializeField] Image appearImage;
        [SerializeField] Image flareImage;
        [SerializeField] Image sunRaysImage;
        [SerializeField] Button triggerButton;
        [SerializeField] SoundData closeSound;


        void Start()
        {
            Disable();
            triggerButton.onClick.AddListener(Close);
        }

        void Close()
        {
            AudioManager.Instance.PlaySound(closeSound);

            appearImage.transform.DOScale(Vector3.zero, shopData.ImageDisppearTime);
            flareImage.transform.DOScale(Vector3.zero, shopData.ImageDisppearTime);
            sunRaysImage.transform.DOScale(Vector3.zero, shopData.ImageDisppearTime).OnComplete(Disable);

        }

        void Disable()
        {
            appearImage.enabled = false;
            flareImage.enabled = false;
            sunRaysImage.enabled = false;
            triggerButton.gameObject.SetActive(false);
        }

        public void PlayAnimation(Sprite spr)
        {
            appearImage.enabled = true;
            flareImage.enabled = true;
            sunRaysImage.enabled = true;

            flareImage.transform.localScale = Vector3.one;
            sunRaysImage.transform.localScale = Vector3.one;

            var width = spr.rect.width;
            var height = spr.rect.height;

            appearImage.rectTransform.sizeDelta = new Vector2(width, height);
            appearImage.sprite = spr;

            appearImage.transform.localScale = Vector3.zero;
            appearImage.transform.DOScale(1, shopData.ImageAppearTime).SetDelay(0.05f);

            var transpColor = Color.white;
            transpColor.a = 0;
            flareImage.color = transpColor;
            sunRaysImage.color = transpColor;
            flareImage.DOColor(Color.white, shopData.ImageAppearTime / 2);
            sunRaysImage.DOColor(Color.white, shopData.ImageAppearTime / 2).OnComplete(() => triggerButton.gameObject.SetActive(true));

        }


    }
}
