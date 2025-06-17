using Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HeroSelect
{
  //  [RequireComponent(typeof(HeroSelectOwned))]
  //  [ExecuteInEditMode]
    public class HeroSelectPurchuase : MonoBehaviour
    {
        [SerializeField] GameObject purchuaseButton;
        [SerializeField] Material liveMaterial;
        [SerializeField] Material grayMaterial;
        [SerializeField] Image art;
        [SerializeField] TextMeshProUGUI goldCostText;
        [SerializeField] TextMeshProUGUI gemCostText;
        [SerializeField] GameObject goldCostComponent;
        [SerializeField] GameObject gemCostComponent;
        public CanvasGroup infoGroup;
        public CanvasGroup selectGroup;

        [SerializeField] HeroSelectOwned ownedScript;

        void Start()
        {
            Invoke(nameof(DelayedCheck), 0.2f);
        }

        void DelayedCheck()
        {
            if (!ownedScript)
                ownedScript = GetComponent<HeroSelectOwned>();
            if (!ownedScript.HeroSelect) return;
            Refresh();
        }

        public void TryPurchuaseHero()
        {
            Debug.Log("Try purchuase hero");
            var playerGold = PlayerStash_resources.Instance.Gold;
            var playerGem = PlayerStash_resources.Instance.Gems;
            var goldCost = ownedScript.HeroSelect.Hero.GoldCost;
            var gemCost = ownedScript.HeroSelect.Hero.GemCost;
            if (playerGold >= goldCost && playerGem >= gemCost)
                Purchuase(goldCost, gemCost);
        }

        void Purchuase(int goldCost, int gemCost)
        {
            PlayerStash_resources.Instance.RemoveGold(goldCost);
            PlayerStash_resources.Instance.RemoveGem(gemCost);
            PlayerStash_heroes.Instance.Own(ownedScript.HeroSelect.Hero);
            ownedScript.SetOwn(true);
            purchuaseButton.SetActive(false);
            art.material = liveMaterial;
            infoGroup.alpha = 1;
            selectGroup.alpha = 1;

            Refresh();
        }

        void Refresh()
        {
            var gold = ownedScript.HeroSelect.Hero.GoldCost;
            var gem = ownedScript.HeroSelect.Hero.GemCost;
            goldCostText.text = gold.ToString();
            gemCostText.text = gem.ToString();
            goldCostComponent.SetActive(gold > 0);
            gemCostComponent.SetActive(gem > 0);
            if (PlayerStash_heroes.Instance)
            {
                art.material = ownedScript.isOwned ? liveMaterial : grayMaterial;
                purchuaseButton.SetActive(!ownedScript.isOwned);
                infoGroup.alpha = ownedScript.isOwned ? 1 : 0.5f;
                selectGroup.alpha = ownedScript.isOwned ? 1 : 0f;
            }
        }


//#if UNITY_EDITOR
//    void Update()
//    {
//        if (!ownedScript) ownedScript = GetComponent<HeroSelectOwned>();
//        if (!ownedScript) return;
//        if (!ownedScript.HeroSelect) return;
//        if (!ownedScript.HeroSelect.Hero) return;

//        if (PlayerStash_heroes.Instance) purchuaseButton.SetActive(!ownedScript.isOwned);

//        if (ownedScript.HeroSelect.Hero == null)
//        {
//            purchuaseButton.SetActive(false);
//            return;
//        }

//        if (PlayerStash_heroes.Instance && ownedScript.isOwned)
//        {
//            Refresh();
//        }
//    }
//#endif
    }
}