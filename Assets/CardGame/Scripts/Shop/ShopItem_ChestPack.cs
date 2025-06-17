using Managers;
using Player;
using Plugins.AudioManager.audio_Manager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Shop
{
    public class ShopItem_ChestPack : ShopItem
    {
        [SerializeField] ShopData_Chest pack;
        [SerializeField] TextMeshProUGUI nameLabel;
        [SerializeField] TextMeshProUGUI costLabel;
        [SerializeField] Text revardLabel;
        [SerializeField] SoundData sound;
        [SerializeField] GameObject vfx;
        [SerializeField] ShopPurchuaseSprite purchuaseAnimation;
    
        void Start()
        {
            Init();
        }

        void Init() {  
            
            nameLabel.text = pack.name;
            costLabel.text = pack.GemCost.ToString();
            revardLabel.text = pack.GoldValue.ToString();
        }
    
        public override void OnPurchasedSuccessfully()
        {
            var res = PlayerStash_resources.Instance;
            if (res.Gems >= pack.GemCost)
            {
                PlayerStash_resources.Instance.RemoveGem(pack.GemCost);
                PlayerStash_resources.Instance.AddGold(pack.GoldValue);
                AudioManager.Instance.PlaySound(sound);

                if (vfx) 
                    Instantiate(vfx, transform);
 
                if (purchuaseAnimation)
                    purchuaseAnimation.PlayAnimation();
            }
   
        }

        public void OnPurchasedSuccessfulleWatchAds()
        {
            var res = PlayerStash_resources.Instance;
       
            if (!AdsManager.Instance.isRewardedReady) return;
            AdsManager.Instance.ShowRewarded();

            //PlayerStash_resources.Instance.GemSubtract(pack.GemCost);
            PlayerStash_resources.Instance.AddGold(pack.GoldValue);

            AudioManager.Instance.PlaySound(sound);

            if (vfx)
                Instantiate(vfx, transform);

            if (purchuaseAnimation)
                purchuaseAnimation.PlayAnimation();       
       
        }
    }
}
