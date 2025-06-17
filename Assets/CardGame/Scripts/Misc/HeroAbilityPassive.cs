using UnityEngine;

namespace Misc
{
    public class HeroAbilityPassive: MonoBehaviour
    {
        CardDataHeroAbilityPassive _ability;
        protected int Lvl = 1;
        protected  CardHero Hero;

        public void Init(CardHero hero,CardDataHeroAbilityPassive ability,   int lvl)
        {
            Hero = hero;
            _ability = ability;
  
            Lvl = lvl;
            OnInit();
        }

        protected virtual void OnInit()
        {
        }
    }
}