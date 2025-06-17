using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Generator Data", menuName = "Game/Generator Data")]
public class GeneratorData : ScriptableObject
{
    [SerializeField] Material grayMaterial;
    [SerializeField] Material heroMaterial;
    [SerializeField] Material artifactMaterial;
    [SerializeField] Material creatureMaterial;
    [SerializeField] [ShowAssetPreview()] Sprite miniBossIcon;
    [SerializeField] [Range(0.5f, 1.5f)] float bossSize = 1f;
    [SerializeField] [Range(0.5f, 1.5f)] float miniBossSize = 1f;
    [Header("Prefabs")]
    [SerializeField] CardHero heroPrefab;
    [SerializeField] Card creaturePrefab;
    [SerializeField] Card artefactPrefab;
    [SerializeField] Card spellPrefab;
    [SerializeField] Card portalPrefab;
    [SerializeField] Card bossPrefab;

    [Space(10)]
    [FormerlySerializedAs("monsterEggDrop")]
    [SerializeField] CardDataArtifact defaultDrop;
    [SerializeField] CardDataArtifact roadCard;

    public CardHero HeroPrefab => heroPrefab;
    public Card CreaturePrefab => creaturePrefab;
    public Card ArtefactPrefab => artefactPrefab;
    public Card SpellPrefab => spellPrefab;
    public Card PortalPrefab => portalPrefab;
    public Card BossPrefab => bossPrefab;
    public Sprite MiniBossIcon => miniBossIcon;
    public float BossSize => bossSize;
    public float MiniBossSize => miniBossSize;
    public CardDataArtifact DefaultDrop => defaultDrop;

    public Material GrayMaterial => grayMaterial;
    public Material HeroMaterial => heroMaterial;
    public Material ArtifactMaterial => artifactMaterial;

    public Material CreatureMaterial => creatureMaterial;

    public CardDataArtifact RoadCard => roadCard;
}