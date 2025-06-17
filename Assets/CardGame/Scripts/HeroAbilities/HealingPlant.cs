using System.Collections.Generic;
using Misc;
using UnityEngine;

namespace HeroAbilities
{
    public class HealingPlant : HeroAbility
    {
        [SerializeField] List<int> healAmount=new();

        protected override void UseAbility()
        {
            var id = data.Level - 1;
            var heal = id < healAmount.Count 
                ? healAmount[id<0? 0:id]
                : healAmount[^1];
            _hero.AddHealth(heal);
        }
    }
}