using Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Ads
{
    public class LevelAds : MonoBehaviour
    {
        void Start()
        {
            var id = SceneManager.GetActiveScene().buildIndex;
            if (id == 2) return;
            if (id % 2 > 0) return;
            AdsManager.Instance.ShowInterstitial();
        }
    }
}