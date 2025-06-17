using System.Collections.Generic;
using Managers;
using Misc;

namespace HeroAbilities
{
    public class GoldIncome : HeroAbilityPassive

    {
        public List<int> incomePerAction = new();


        void Start()
        {
            EventManager.Instance.OnInteract += AddGold;
        }

        void AddGold(Card card)
        {
            var gold = incomePerAction[Lvl - 1];
            EventManager.Instance.CreateGoldVFX(Hero.transform.position, gold);
        }
    }
}