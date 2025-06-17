using System.Collections.Generic;
using System.Linq;
using Gameplay.UI.Perks;
using Managers;
using Misc;
using UnityEngine;

namespace Player
{
    public class HeroAbilities : MonoBehaviour
    {
        public AbilitySelectUI selectUI;
        public List<HeroAbilityUI> uis = new();
        public List<HeroAbility> abilities = new();
        public List<HeroAbilityData> abilitiesData = new();
        CardHero _hero;

        void Start()
        {
            foreach (var ui in uis)
            {
            ui.Hide();
            }

            selectUI.Hide();
            selectUI.OnSelected += UpgradeAbility;
            EventManager.Instance.OnPlayerCreated += Set;
            EventManager.Instance.OnUsePotion += Add;
            EventManager.Instance.OnHeroLevelUp += ShowSelectUI;
        }

        void UpgradeAbility(SelectSlotUI slot)
        {
            var data = slot.Data;
            var ui = uis.FirstOrDefault(u => u.Data == data);
            ui.Data.Level++;
            ui.SetLevel(ui.Data.Level);
            ui.Show();
        }

        void OnDisable()
        {
            EventManager.Instance.OnPlayerCreated -= Set;
            EventManager.Instance.OnUsePotion -= Add;
            EventManager.Instance.OnHeroLevelUp -= ShowSelectUI;
        }

        void ShowSelectUI(int obj)
        {
            Invoke(nameof(ShowUI), 2f);
        }

        void ShowUI()
        {
            selectUI.Refresh(abilitiesData);
            selectUI.Show();
        }

        void Create(CardHero hero, List<CardDataHeroAbilityPassive> list, List<int> levels)
        {
            for (var i = 0; i < list.Count; i++)
            {
                var ability = list[i];

                var a = Instantiate(ability.Prefab, transform);
                a.name = ability.Name;
                a.Init(hero, ability, levels[i]);
            }
        }

        void Create(List<CardDataHeroAbility> list, List<int> levels)
        {
            for (var i = 0; i < list.Count; i++)
            {
                var ability = list[i];

                var a = Instantiate(ability.Prefab, transform);
                a.name = ability.Name;
               // a.Set(ability, uis[i], levels[i]);
                //a.Init(ability, uis[i], levels[i]);
            }
        }

        void Add(PotionData potion)
        {
            foreach (var ability in abilities.Where(ability => ability.PotionType == potion.type))
            {
                ability.AddPotion(potion);
            }
        }

        void Set(CardHero hero)
        {
            _hero = hero;
        

            foreach (var ability in hero.Data.ActiveAbilities)
            {
                var data = new HeroAbilityData
                {
                    Ability = ability
                };

                abilitiesData.Add(data);
            }
            for (var i = 0; i < abilities.Count; i++)
            {
                var ability = abilities[i];
                ability.Set(hero, abilitiesData[i]);
            }
        }
    }
}