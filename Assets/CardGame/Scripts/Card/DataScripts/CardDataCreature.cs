using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "New Creature", menuName = "Card/Creature")]
public class CardDataCreature : CardDataUnit
{
    [Space(20)]
    [SerializeField] CreatureDangerous dangerous;
    [SerializeField] bool isAggressive;
    [Header("Drop")]
    [SerializeField] List<ScriptableObject> drops = new();
    public IReadOnlyList<ScriptableObject> Drops => drops;
    public CreatureDangerous Dangerous => dangerous;
    public bool IsAggressive => isAggressive;

    public ScriptableObject GetRandomDrop()
        => drops.Count == 0
            ? null
            : drops[Random.Range(0, drops.Count)];

}