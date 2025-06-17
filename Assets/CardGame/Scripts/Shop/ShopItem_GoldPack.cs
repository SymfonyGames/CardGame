using Player;
using Plugins.AudioManager.audio_Manager;
using UnityEngine;
using UnityEngine.UI;

namespace Shop
{
    public class ShopItem_GoldPack : ShopItem
    {
        [SerializeField] ShopData_Gold pack;
        [SerializeField] Text nameLabel;
        [SerializeField] Text valueLabel;
        [SerializeField] SoundData sound;
        [SerializeField] GameObject vfx;
        [SerializeField] ShopPurchuaseImage shopPurchuaseImage;
    
        void Start()
        {
            Init();
        }

        void Init()
        {
            nameLabel.text = pack.GoldChestName;
            valueLabel.text = pack.Value.ToString();
        }
    
        public override void OnPurchasedSuccessfully()
        {
            PlayerStash_resources.Instance.AddGold(pack.Value);
            AudioManager.Instance.PlaySound(sound);

            if (vfx) 
                Instantiate(vfx, transform);
 
            if (shopPurchuaseImage)
                shopPurchuaseImage.PlayAnimation();
        }
    }
}
