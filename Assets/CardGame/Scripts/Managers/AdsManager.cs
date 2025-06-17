using System;
using CAS;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class AdsManager : MonoBehaviour
    {
        const string ADS_DISABLED_KEY = "Ads.Disabled";

        [SerializeField] float _adShowInterval = 60f;
        [SerializeField] UnityEvent _onStateChanged;
        [SerializeField] UnityEvent _onResurrectRewardEarned;
        static AdsManager _instance;
        float _lastAdShowTime;
        IMediationManager _manager;
        bool _adsDisabled;


        public bool IsInterstitialReady =>
            !_adsDisabled
            // && (Time.realtimeSinceStartup - _lastAdShowTime) > _adShowInterval
            && _manager != null
            && _manager.IsReadyAd(AdType.Interstitial)
            && SceneManager.GetActiveScene().name != "_Tutorial";

        public bool isRewardedReady
            => _manager != null && _manager.IsReadyAd(AdType.Rewarded);


        public static AdsManager Instance
        {
            get
            {
                if (_instance != null) return _instance;
                _instance = FindObjectOfType<AdsManager>();

                if (_instance != null) return _instance;
                var obj = new GameObject("Ads Manager");
                _instance = obj.AddComponent<AdsManager>();

                return _instance;
            }
        }


        void Awake()
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }

        void Start()
        {
            //var request = CAS.UserConsent.UserConsent.BuildRequest();
            //request.WithCallback(InitializeSDK);
            _adsDisabled = Convert.ToBoolean(PlayerPrefs.GetInt(ADS_DISABLED_KEY, 0));
        }

        public void DisableAds()
        {
            PlayerPrefs.SetInt(ADS_DISABLED_KEY, 1);
            PlayerPrefs.Save();
            _adsDisabled = true;
        }

        public void Initialize()
        {
            MobileAds.settings.isExecuteEventsOnUnityThread = true;
            _manager = MobileAds.BuildManager().Initialize();
            _manager.OnRewardedAdCompleted += OnRewardedEarned;
            _manager.OnRewardedAdClosed += OnRewardedAdClosed;
            _manager.OnInterstitialAdClosed += OnInterstitialAdClosed;
            //_manager.OnLoadedAd += t => _onStateChanged.Invoke();
            _manager.OnInterstitialAdLoaded += () => _onStateChanged.Invoke();
        }

        public void AddOnStateChangedListener(UnityAction listener)
            => _onStateChanged.AddListener(listener);

        public void RemoveOnStateChangedListener(UnityAction listener)
        {
            if (_onStateChanged == null) return;
            _onStateChanged.RemoveListener(listener);
        }

        public void AddOnResurrectRewardEarnedListener(UnityAction listener)
            => _onResurrectRewardEarned?.AddListener(listener);

        public void RemoveOnResurrectRewardEarnedListener(UnityAction listener)
        {
            _onResurrectRewardEarned?.RemoveListener(listener);
        }

        // Ads 

        void OnRewardedAdClosed() => _lastAdShowTime = Time.realtimeSinceStartup;

        void OnInterstitialAdClosed() => _lastAdShowTime = Time.realtimeSinceStartup;

        public void ResetAdsTimer() => _lastAdShowTime = Time.realtimeSinceStartup;
        public event Action OnRewardedComplete = delegate { };

        void OnRewardedEarned()
        {
            _onResurrectRewardEarned.Invoke();
            _onStateChanged.Invoke();
            OnRewardedComplete();
        }

        // Public methods
        // ������ 195 � Interactsystem
        public void ShowInterstitial()
        {
            if (!IsInterstitialReady) return;
#if UNITY_EDITOR
            Debug.Log("ADS: Show Interstitial");
#endif
            _manager.ShowAd(AdType.Interstitial);
        }

        public void ShowRewarded()
        {
            if (!isRewardedReady) return;
#if UNITY_EDITOR
            Debug.Log("ADS: Show Rewarded");
#endif
            _manager.ShowAd(AdType.Rewarded);
        }
    }
}