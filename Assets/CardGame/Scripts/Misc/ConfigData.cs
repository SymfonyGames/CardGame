using System;
using System.Collections.Generic;
using Player;
using UnityEngine;
using UnityEngine.Serialization;

namespace Misc
{
    [CreateAssetMenu(fileName = "Config Data", menuName = "Game/Config Data")]
    public class ConfigData : ScriptableObject
    {
        [Space(10)] [SerializeField] int hpMinHealAnimation;
        [Space(10)] [SerializeField] ResourceData reviveCost;
        [FormerlySerializedAs("heroExperience")]
        [Space(10)]
        [SerializeField] int heroLevelMax;
        [SerializeField] List<float> heroExperienceForLevel = new();
        [SerializeField] List<float> unitExperience = new();
        [Space(10)]
        [SerializeField] int firstAbilityRequire;
        [SerializeField] int secondAbilityRequire;
        [SerializeField] int thirdAbilityRequire;
        [Space(10)]
        [SerializeField] float expPerStep;
        [SerializeField] float expPerMiniBoss;
        [SerializeField] float expPerBoss;

        public ResourceData ReviveCost => reviveCost;
        public int HpMinHealAnimation => hpMinHealAnimation;
        public IReadOnlyList<float> heroExperienceTable => heroExperienceForLevel;

        public int FirstAbilityRequire => firstAbilityRequire;
        public int SecondAbilityRequire => secondAbilityRequire;
        public int ThirdAbilityRequire => thirdAbilityRequire;
        public float ExpPerStep => expPerStep;
        public float ExpPerMiniBoss => expPerMiniBoss;
        public float ExpPerBoss => expPerBoss;

        public int HeroLevelMax => heroLevelMax;
        public float heroExpCoef = 50;
        public float unitExpCoef = 50;
        public float unitExpAddCoef = 50;

        public float GetExpDrop(int cardHP)
        {
            var step = cardHP / 5f;
            var mult = (11 - step) / 10;
            
            return baseExp * (step) * mult;
            return cardHP - 1 < unitExperience.Count ? unitExperience[cardHP - 1] : unitExperience[^1];
        }

        public float TEXT_HP;
        public float TEXT_EXP;
        public float TEXT_EXPAdd;
        [SerializeField] List<float> test = new();
        [SerializeField] AnimationCurve curvesChance;
        [SerializeField] float chanceFactor = 4f;
        public int maxExp;
        public float baseExp = 25;
        public bool autoSetHeroExperience;
        void OnValidate()
        {
            var step = TEXT_HP / 10;
            var mult = (11 - step) / 10;
            
            TEXT_EXP = baseExp * (step) * mult;
            //test[0] = 25;
            for (int i = 0; i < test.Count; i++)
            {
                //test[i] = test[i - 1] + TEXT_EXPAdd * (i + 2);
                var point = (float) i / (maxExp);
                test[i] = 2500 * curvesChance.Evaluate(point);
                test[i] = Mathf.Log10(i * 5);
            }

            if (autoSetHeroExperience)
            {
                for (var i = 0; i < heroExperienceForLevel.Count; i++)
                {
                    var level = i + 1;
                    heroExperienceForLevel[i] = heroExpCoef * (level * level + level - 2);
                }
            }
           

            unitExperience[0] = unitExpCoef;
            for (int i = 1; i < unitExperience.Count; i++)
            {
                unitExperience[i] = unitExperience[i - 1] + (unitExpAddCoef * (i + 2));
            }
        }
    }
}