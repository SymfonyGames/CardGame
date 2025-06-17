using Managers;
using UnityEngine;

namespace TESTS
{
    public class TEST_HERO : MonoBehaviour
    {
        [Header("Actions")]
        public bool healMe;
        public bool addShield;
        public bool seeAttackVFX;
        [Header("HP")]
        public bool setHp;
        public int newHp=1000;

    
        private CardHero _hero;

        private void Start()
        {
            EventManager.Instance.OnPlayerCreated += PlayerCreated;
        }

        private void PlayerCreated(Card obj)
        {
            if (obj is CardHero isHero)
            {
                _hero = isHero;
            }
        }

#if UNITY_EDITOR
        void Update()
        {
            if (healMe)
            {
                healMe = false;
                EventManager.Instance.PlayerHeal(1);
            }

            if (addShield)
            {
                addShield = false;
                EventManager.Instance.PlayerShield(1);
            }

            if (setHp)
            {
                setHp = false;
                _hero.CheatSetupHP(newHp);
            }

            if (seeAttackVFX)
            {
                seeAttackVFX = false;
                EventManager.Instance.PlayerAttackVFX();
            }
        }
#endif
    }
}