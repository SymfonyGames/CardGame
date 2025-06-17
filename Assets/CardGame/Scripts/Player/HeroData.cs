using UnityEngine;

namespace Player
{
    [System.Serializable]
    public class HeroData
    {
        public CardDataHero hero;
        public int id;
        [HideInInspector] public int level = 1;
        [HideInInspector] public float experience;
        public int firstAbilityLevel = 1;
        public int secondAbilityLevel;
        public int thirdAbilityLevel;
        [HideInInspector] public int selectedAbilityID;
        public bool owned;
    }
}