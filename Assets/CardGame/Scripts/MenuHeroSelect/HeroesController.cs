using System.Collections.Generic;
using HeroSelect;
using Maps;
using Misc;
using NaughtyAttributes;
using Player;
using UnityEngine;

namespace MenuHeroSelect
{
    public class HeroesController : MonoBehaviour
    {
        [Header("Pool")]
        [SerializeField] Map map;
        [SerializeField] HeroCardUI prefab;
        [SerializeField] Transform prefabsContainer;
        [SerializeField] [ReadOnly] List<HeroCardUI> cards = new();
        PlayerStash_heroes _stash;
        public HeroView view;
        public ConfigData config;

        void Start()
        {
            _stash = PlayerStash_heroes.Instance;
            LoadItems();
        }

        void LoadItems()
        {
            var heroes
                = _stash.SAVE.heroes;
            foreach (var hero in heroes)
                CreateItem(hero);

            foreach (var card in cards)
                card.Deselect();

            var selectedHero = _stash.SAVE.selected;
            var selectedUI = cards.Find(c => c.Data.hero == selectedHero.hero);
            selectedUI.Select();
            lastSelected = selectedUI;
            view.Refresh(selectedHero);
            map.SetHeroIcon(selectedHero.hero);
        }

        void CreateItem(HeroData data)
        {
            var card = Instantiate(prefab, prefabsContainer);
            card.Refresh(data, config.HeroLevelMax);
            card.OnClick += OnClick;
            cards.Add(card);

#if UNITY_EDITOR
            var itemName = data.hero.Name != string.Empty
                ? (string) data.hero.Name
                : data.hero.name;
            card.gameObject.name = "[Inventory] Item - " + itemName;
#endif
        }

        void OnClick(HeroCardUI clicked)
        {
            // if (!clicked.Data.owned)
            // {
            //     //HERE OPEN BUY WINDOW 
            //     //  var hero = _storage.GetData(clicked.SO);
            //     //   characterScan.ShowBuyWindow(hero);
            //
            //     return;
            // }

            Select(clicked);
        }

        HeroCardUI lastSelected;

        public void Select(HeroCardUI clicked)
        {
            if (clicked == lastSelected)
            {
                view.Refresh(clicked.Data);
                foreach (var card in cards)
                {
                    if (card == clicked) continue; 
                    card.Deselect();
                }
                return;
            }

            foreach (var card in cards)
            {
                if (card == clicked)
                {
                    card.Select();
                    if (clicked.Data.owned)
                    {
                        lastSelected.Deselect();
                        lastSelected = card;
                        map.SetHeroIcon(card.Data.hero);
                    }
 
                }
                else
                {
                    if (card != lastSelected)
                        card.Deselect();
                }
            }


            if (clicked.Data.owned)
            {
                _stash.Select(clicked.Data.hero);
            }

            view.Refresh(clicked.Data);
        }
    }
}

// public class HeroCardUIData
// {
//     HeroCardUIData UIData(HeroCardData hero)
//         => new()
//         {
//             SO = hero.so,
//             Lvl = hero.lvl,
//             LvlMax = 30,
//             Owned = hero.owned
//         };
// }