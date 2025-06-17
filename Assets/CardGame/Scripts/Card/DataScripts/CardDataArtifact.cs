using System.Collections.Generic;
using NaughtyAttributes;
using Plugins.ColorAttribute;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "New Artifact", menuName = "Card/Artifact")]
public class CardDataArtifact : CardData
{
    [Space(30)]

    [ContentColor(0.2f, 1f, 1f, 1f)]
    [SerializeField] Color glowColor;

    [Space(10)]
    [SerializeField] GameObject portalHealthVFX;
    [SerializeField] GameObject portalShieldVFX;
    [SerializeField] GameObject portalCoinVFX;
    [SerializeField] GameObject portalManaFX;

    [Space(10)]
    [FormerlySerializedAs("bonusHealthMin")] [SerializeField] int hpMin;
    [FormerlySerializedAs("bonusHealthMax")] [SerializeField] int hpMax;
    [Space(10)]
    [FormerlySerializedAs("bonusShieldMin")] [SerializeField] int shieldMin;
    [FormerlySerializedAs("bonusShieldMax")] [SerializeField] int shieldMax;
    [Space(10)]
    [FormerlySerializedAs("bonusCoinsMin")] [SerializeField] int manaMin;
    [FormerlySerializedAs("bonusCoinsMax")] [SerializeField] int manaMax;
    [Space(10)]
    [FormerlySerializedAs("bonusCoinsMin")] [SerializeField] int goldMin;
    [FormerlySerializedAs("bonusCoinsMax")] [SerializeField] int goldMax;
    
        [Space(10)]
        [FormerlySerializedAs("bonusCoinsMin")] [SerializeField] int silverMin;
        [FormerlySerializedAs("bonusCoinsMax")] [SerializeField] int silverMax;
    [Space(10)]
    [FormerlySerializedAs("bonusGemMin")] [SerializeField] int gemMin;
    [FormerlySerializedAs("bonusGemMax")] [SerializeField] int gemMax;
    [Space(10)]
    [FormerlySerializedAs("bonusGemMin")] [SerializeField] int expMin;
    [FormerlySerializedAs("bonusGemMax")] [SerializeField] int expMax;

    [Header("Potions")]
    [SerializeField] List<PotionData> potions = new();


    public int HP => Random.Range(hpMin, hpMax + 1);
    public int Mana => Random.Range(manaMin, manaMax + 1);
    public int Exp => Random.Range(expMin, expMax + 1);
    public int Shield => Random.Range(shieldMin, shieldMax + 1);
    public int Gold => Random.Range(goldMin, goldMax + 1);
    public int Silver => Random.Range(silverMin, silverMax + 1);
    public int Gem => Random.Range(gemMin, gemMax + 1);
    public Color GlowColor => glowColor;
    public GameObject PortalHealthVFX => portalHealthVFX;
    public GameObject PortalShieldVFX => portalShieldVFX;
    public GameObject PortalCoinVFX => portalCoinVFX;
    public GameObject PortalManaVFX => portalManaFX;
    public IReadOnlyList<PotionData> Potions => potions;
}