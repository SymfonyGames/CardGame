using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Misc
{
    [RequireComponent(typeof(Button))]
    public class AdsRelatedButton : MonoBehaviour
    {
        [SerializeField] bool _checkResurrectAd;
        Button _button;


        void Awake()
        {
            _button = GetComponent<Button>();
        }

        void OnEnable()
        {
            OnAdsManagerStateChanged();
            AdsManager.Instance.AddOnStateChangedListener(OnAdsManagerStateChanged);
        }

        void OnDisable()
        {
            AdsManager.Instance.RemoveOnStateChangedListener(OnAdsManagerStateChanged);
        }

        void OnAdsManagerStateChanged()
        {
            var ready = true;
            if (_checkResurrectAd) 
                ready &= AdsManager.Instance.isRewardedReady;
            _button.interactable = ready;
        }
    }
}