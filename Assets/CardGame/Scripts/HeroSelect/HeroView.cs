using System.Collections.Generic;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HeroSelect
{
    public class HeroView : MonoBehaviour
    {
        public Image art;
        public Material gray;
        public Material normal;
        public Button buyButtonGold;
        public Button buyButtonGem;
        public TextMeshProUGUI buyTextGold;
        public TextMeshProUGUI buyTextGem;
        public GameObject buyContainer;

        public TextMeshProUGUI nameLabel;
        public TextMeshProUGUI lvlLabel;
        public TextMeshProUGUI hp;
        public TextMeshProUGUI hpBonus;
        public TextMeshProUGUI activeLabel;
        public TextMeshProUGUI passiveLabel;

        public List<HeroViewAbility> passive = new();
        public List<HeroViewAbility> abilities = new();
        public List<HeroViewAbilityStars> abilitiesStars = new();

        public void Refresh(HeroData data)
        {
            if (!data.owned)
            {
                buyContainer.SetActive(true);
                if (data.hero.GoldCost > 0)
                {
                    buyButtonGold.gameObject.SetActive(true);
                    buyTextGold.text = data.hero.GoldCost.ToString();
                }
                else
                {
                    buyButtonGold.gameObject.SetActive(false); 
                }
                if (data.hero.GemCost > 0)
                {
                    buyButtonGem.gameObject.SetActive(true);
                    buyTextGem.text = data.hero.GemCost.ToString();
                }
                else
                {
                    buyButtonGem.gameObject.SetActive(false);
                }
            }
            else
            {
                buyContainer.SetActive(false);
            }

            art.sprite = data.hero.Art;

            nameLabel.text = data.hero.Name;
            lvlLabel.text = "Level " + data.level;
            hp.text = data.hero.Health.ToString();
            var bonusHP = (data.level - 1) * data.hero.HealthPerLevel;
            hpBonus.text = "(+" + bonusHP + ") ";

            art.material = data.owned ? normal : gray;

            if (data.hero.PassiveAbilities.Count > 0)
            {
                var p = data.hero.PassiveAbilities[0];
                passive[0].Refresh(p.Icon, p.Description, data.owned);
                passive[0].gameObject.SetActive(true);
                passiveLabel.gameObject.SetActive(true);
            }
            else
            {
                passive[0].gameObject.SetActive(false);
                passiveLabel.gameObject.SetActive(false);
            }


            for (var i = 0; i < data.hero.ActiveAbilities.Count; i++)
            {
                var ability = data.hero.ActiveAbilities[i];
                abilities[i].Refresh(ability.Icon, ability.Description, data.owned);
            }


            if (data.hero.ActiveAbilities.Count > 0)
            {
                abilitiesStars[0].gameObject.SetActive(true);
                abilitiesStars[0].SetIcon(data.hero.ActiveAbilities[0].Icon);
                abilitiesStars[0].SetLevel(data.firstAbilityLevel);

                abilities[0].gameObject.SetActive(true);
                activeLabel.gameObject.SetActive(true);
            }
            else
            {
                abilitiesStars[0].Hide();
                abilities[0].gameObject.SetActive(false);
                activeLabel.gameObject.SetActive(false);
            }

            if (data.hero.ActiveAbilities.Count > 1)
            {
                abilitiesStars[1].gameObject.SetActive(true);
                abilitiesStars[1].SetIcon(data.hero.ActiveAbilities[1].Icon);
                abilitiesStars[1].SetLevel(data.secondAbilityLevel);

                abilities[1].gameObject.SetActive(true);
            }
            else
            {
                abilitiesStars[1].Hide();
                abilities[1].gameObject.SetActive(false);
            }

            if (data.hero.ActiveAbilities.Count > 2)
            {
                abilitiesStars[2].Hide();
                abilitiesStars[2].SetIcon(data.hero.ActiveAbilities[2].Icon);
                abilitiesStars[2].SetLevel(data.thirdAbilityLevel);

                abilities[2].gameObject.SetActive(true);
            }
            else
            {
                abilitiesStars[2].gameObject.SetActive(false);
                abilities[2].gameObject.SetActive(false);
            }

            if (!data.owned)
            {
                abilitiesStars[0].Hide();
                abilitiesStars[1].Hide();
                abilitiesStars[2].Hide();
            }
            else
            {
                if (data.firstAbilityLevel > 0) abilitiesStars[0].Show();
                if (data.secondAbilityLevel > 0) abilitiesStars[1].Show();
                if (data.thirdAbilityLevel > 0) abilitiesStars[2].Show();
            }
        }
    }
}