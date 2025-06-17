using System.Collections.Generic;
using System.Linq;
using BossGame;
using NaughtyAttributes;
using Player;
using Plugins.AudioManager.audio_Manager;
using Plugins.ColorAttribute;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "New Hero", menuName = "Card/Hero")]
public class CardDataHero : ScriptableObject
{
    [SerializeField] string cardName;
    [SerializeField] string description;
    [SerializeField] SoundData interactSound;
    [SerializeField] [ShowAssetPreview] Sprite artwork;
    [ContentColor(0.5f, 1f, 0.5f, 1f)]
    [Space(30)]
    [SerializeField] int health;
    [SerializeField] int mana;
    [SerializeField] float dmgMin;
    [SerializeField] float dmgMax;
    [SerializeField] HeroStats stats;
    [SerializeField] int shield;
    [Space(10)]
    [SerializeField] int healthPerLevel = 2;
    [Space(10)]
    [SerializeField] SoundData attackSound;
    [SerializeField] SoundData deathSound;
    [SerializeField] GameObject attackVFX;

    [ContentColor]
    [Space(10)]
    [SerializeField] GameObject healthPickupVFX;
    [SerializeField] GameObject shieldPickupVFX;


    [Space(10)]
    [SerializeField] List<CardDataHeroAbilityPassive> passiveAbilities = new();
    [SerializeField] List<CardDataHeroAbility> activeAbilities = new();


    [Space(10)]
    [ContentColor(1f, 1f, 0f, 1f)]
    [FormerlySerializedAs("price")]
    [SerializeField] ResourceData shopPrice;

    [FormerlySerializedAs("stats")]
    [Space(20)]
    [HideInInspector] [SerializeField] PlayerStats customGameStats;
    [Space(20)]
    [Header("ACTIONS")]
    [HideInInspector] [SerializeField] ActionStats actionStats;
    [HideInInspector] [SerializeField] List<float> chances = new();
    [Header("SPECIAL ACTIONS")]
    [HideInInspector] [SerializeField] ActionStats attacks;
    [HideInInspector] [SerializeField] ActionStats defense;
    [HideInInspector] [SerializeField] ActionStats ultimate;
    public ActionStats Attacks => attacks;
    public ActionStats Defense => defense;
    public ActionStats Ultimate => ultimate;

    public int GoldCost => shopPrice.gold;
    public int GemCost => shopPrice.gems;

    public GameObject HealthPickupVFX => healthPickupVFX;
    public GameObject ShieldPickupVFX => shieldPickupVFX;
    public Sprite Art => artwork;
    public SoundData InteractSound => interactSound;
    public string Name => cardName;
    public string Description => description;

    public int Health => health;
    public int Shield => shield;
    public SoundData AttackSound => attackSound;
    public SoundData DeathSound => deathSound;
    public GameObject AttackVFX => attackVFX;

    void OnValidate()
    {
        chances = new List<float>();
        if (actionStats.actions.Count > 0)
        {
            for (var i = 0; i < actionStats.actions.Count; i++)
            {
                var chance = (int) (GetChance(i) * 100f);
                chances.Add(chance);
            }
        }
    }

    public float GetChance(int curveId)
    {
        var factor = 1 / actionStats.factor;

        var point = (float) curveId / (actionStats.actions.Count - 1);
        var value = actionStats.curve.Evaluate(point);

        var factorValue = value + factor;
        var factorTotal = TotalChance + factor * actionStats.actions.Count;

        return factorValue / factorTotal;
    }

    float TotalChance => actionStats.actions
        .Select((t, i) => i / (float) (actionStats.actions.Count - 1))
        .Sum(actionStats.curve.Evaluate);


    public PlayerStats CustomGameStats => customGameStats;

    public ActionStats Actions => actionStats;

    public IReadOnlyList<CardDataHeroAbility> ActiveAbilities => activeAbilities;
    public IReadOnlyList<CardDataHeroAbilityPassive> PassiveAbilities => passiveAbilities;

    public int HealthPerLevel => healthPerLevel;

    public int Mana => mana;

    public HeroStats Stats => stats;

    public float DmgMin => dmgMin;

    public float DmgMax => dmgMax;
}