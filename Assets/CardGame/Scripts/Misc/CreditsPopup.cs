using UnityEngine;
using UnityEngine.UI;

namespace Misc
{
    public class CreditsPopup : MonoBehaviour
    {
        [SerializeField] UrlData url;
        [SerializeField] Button musicButton;
        [SerializeField] Button artButton;
        [SerializeField] GameObject creditsPopup;

        void Awake()
        {
            musicButton.onClick.AddListener(MusicUrl);
            artButton.onClick.AddListener(ArtUrl);
            creditsPopup.SetActive(false);
        }

        void MusicUrl()
        {
            Application.OpenURL(url.MusicUrl);
        }

        void ArtUrl()
        {
            Application.OpenURL(url.MusicUrl);
        }
    }
}