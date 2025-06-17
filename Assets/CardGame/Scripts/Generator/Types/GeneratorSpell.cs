using System.Collections.Generic;
using System.Linq;
using Level;
using Misc;
using UnityEngine;

public class GeneratorSpell : Generator
{
    [SerializeField] AnimationCurve curvesChance;
    [SerializeField] float chanceFactor = 4f;
    [Header("Create:")] 
    [SerializeField] List<CardDataSpell> spells = new();
    public IReadOnlyList<CardDataSpell> Items => spells;
    Card _prefab;
    GeneratorData _generatorData;


    public override void Init(GeneratorData generatorData, ConfigData configData)
    {
        _generatorData = generatorData;
        _prefab = generatorData.SpellPrefab;
        InitChances();
    }
    void InitChances()
    {
        foreach (var item in spells)
        {
            SpawnChances.Add(item.ChanceGeneration);
            RotateChances.Add(item.ChanceRotation);
        } 
    }
    
    
    public override Card Spawn( LevelTheme theme)
    {
        var card = Instantiate(_prefab);
        card.Init(GetRandom(), theme.Data.Theme);
        card.Set(_generatorData );
        return card;
    }
    
    public CardDataSpell GetRandom()
    {
        var r = Random.Range(0, 100) * 0.01f;
        var sum = 0f;

        for (var i = 0; i < spells.Count; i++)
        {
            var chance = GetChance(i);
            sum += chance;
            if (r <= sum)
            {
                return spells[i];
            }
        }

        Debug.LogError("Null returned");
        return null;
    }

    public float GetChance(int curveId)
    {
        if (spells.Count == 1) return 1;
        
        var factor = 1 / chanceFactor;

        var point = (float) curveId / (spells.Count - 1);
        var value = curvesChance.Evaluate(point);

        var factorValue = value + factor;
        var factorTotal = TotalChance + factor * spells.Count;

        return factorValue / factorTotal;
    }

    float TotalChance => spells
        .Select((t, i) => i / (float) (spells.Count - 1))
        .Sum(curvesChance.Evaluate);
}