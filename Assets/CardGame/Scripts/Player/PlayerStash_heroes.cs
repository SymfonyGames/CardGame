using System.Collections.Generic;
using System.Linq;
using DataSave;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;

namespace Player
{
    public class PlayerStash_heroes : MonoBehaviour
    {
        #region Singleton

        //-------------------------------------------------------------
        public static PlayerStash_heroes Instance;

        void Awake()
        {
            if (Application.isPlaying)
            {
                if (Instance == null) Instance = this;
                else gameObject.SetActive(false);
            }

            OnAwake();
        }

        //-------------------------------------------------------------

        #endregion

        [SerializeField] PlayerData playerData;
        [Space(10)]
        [FormerlySerializedAs("allHeroes")]
        [SerializeField] List<HeroData> all = new();
        [SerializeField] HeroData defaultHero;
        [FormerlySerializedAs("owned")]
        [Space(10)] [ReadOnly]
        [SerializeField] HeroesOwnedData save;

        const string OWNED = SaveKeys.PLAYER_HEROES_OWNED;

        public bool IsHeroOwned(CardDataHero hero)
            => save.heroes.Any(h => h.hero == hero);

        public PlayerData PlayerData => playerData;

        public HeroesOwnedData SAVE => save;

        void OnAwake() => LoadData();

        void LoadData()
        {
            if (PlayerPrefs.HasKey(OWNED))
            {
                save = JsonUtility.FromJson<HeroesOwnedData>(PlayerPrefs.GetString(OWNED));
                foreach (var save in save.heroes)
                {
                    var id = save.id;
                    var find = all.Find(s => s.id == id);
                    save.hero = find.hero;
                }

                var i = save.selected.id;
                var f = all.Find(s => s.id == i);
                save.selected.hero = f.hero;
            }
            else
            {
                defaultHero.owned = true;
                save = new HeroesOwnedData
                {
                    selected = defaultHero,
                    heroes = new List<HeroData> {defaultHero}
                };

                foreach (var data in all)
                {
                    if (data.hero == defaultHero.hero) continue;
                    save.heroes.Add(new HeroData()
                    {
                        hero = data.hero,
                        id = data.id,
                        level = 1,
                        owned = data.owned
                    });
                }
            }
        }

        public void Select(CardDataHero hero)
        {
            var find = all.Find(h => h.hero == hero);
            save.selected = find;
            playerData.selectedHero = hero;
            Save();
        }

        public void SetLevel(CardDataHero hero, int lvl)
        {
            var find = all.Find(h => h.hero == hero);
            find.level = lvl;
            Save();
        }

        public void SetExperience(CardDataHero hero, float exp)
        {
            var find = all.Find(h => h.hero == hero);
            find.experience = exp;
            Save();
        }

        public void SetAbilityLevel(CardDataHero hero, CardDataHeroAbility ability, int lvl)
        {
            var find = all.Find(h => h.hero == hero);
            var index = find.hero.ActiveAbilities.ToList().IndexOf(ability);
            switch (index)
            {
                case 0:
                    find.firstAbilityLevel = lvl;
                    break;
                case 1:
                    find.secondAbilityLevel = lvl;
                    break;
                case 2:
                    find.thirdAbilityLevel = lvl;
                    break;
            }

            Save();
        }

        void Save()
        {
            PlayerPrefs.SetString(OWNED, JsonUtility.ToJson(save));
            PlayerPrefs.Save();
        }

        public void Own(CardDataHero hero)
        {
            if (save.heroes.Any(h => h.hero == hero)) return;

            var find = all.Find(h => h.hero == hero);
            save.heroes.Add(new HeroData
            {
                hero = hero,
                id = find.id,
                level = 1,
                owned = true
            });
            Save();
        }
    }
}