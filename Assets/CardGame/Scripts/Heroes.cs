using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using Misc;
using Player;
using UnityEngine;

public class Heroes : MonoBehaviour
{
    public ConfigData config;
    public List<HeroData> heroes = new();
    PlayerStash_heroes _stash;

    void Start()
    {
        _stash = PlayerStash_heroes.Instance;
        EventManager.Instance.OnInteract += AddExperience;
    }

    void OnDisable()
    {
        EventManager.Instance.OnInteract -= AddExperience;
    }

    void AddExperience(Card card)
    {
        var exp = card is CardBoss b
            ? b.IsMiniBoss
                ? config.ExpPerMiniBoss
                : config.ExpPerBoss
            : config.ExpPerStep;

        Add(exp);
    }

    void Add(float exp)
    {
        foreach (var data in heroes)
        {
            data.experience += exp;

            var id = data.level - 1;
            var require = config.heroExperienceTable[id];

            if (data.experience >= require)
            {
                data.level++;
                data.experience -= require;
                _stash.SetLevel(data.hero, data.level);

                if (data.firstAbilityLevel == 0 && data.level >= config.FirstAbilityRequire)
                {
                    data.firstAbilityLevel = 1;
                    if (data.hero.ActiveAbilities.Count > 0)
                        _stash.SetAbilityLevel(data.hero, data.hero.ActiveAbilities[0], 1);
                }

                if (data.secondAbilityLevel == 0 && data.level >= config.SecondAbilityRequire)
                {
                    data.secondAbilityLevel = 1;
                    if (data.hero.ActiveAbilities.Count > 1)
                        _stash.SetAbilityLevel(data.hero, data.hero.ActiveAbilities[1], 1);
                }

                if (data.thirdAbilityLevel == 0 && data.level >= config.ThirdAbilityRequire)
                {
                    data.thirdAbilityLevel = 1;
                    if (data.hero.ActiveAbilities.Count > 2) 
                        _stash.SetAbilityLevel(data.hero, data.hero.ActiveAbilities[2], 1);
                }
            }

            _stash.SetExperience(data.hero, data.experience);
        }
    }
}