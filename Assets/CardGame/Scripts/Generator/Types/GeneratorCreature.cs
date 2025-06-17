using System.Collections.Generic;
using System.Linq;
using Level;
using Misc;
using NaughtyAttributes;
using UnityEngine;

public class GeneratorCreature : Generator
{
    [SerializeField] AnimationCurve curvesChance;
    [SerializeField] float chanceFactor = 4f;
    [Header("Create:")]
    [SerializeField] [Expandable] List<CardDataCreature> creatures;
    public IReadOnlyList<CardDataCreature> Items => creatures;
    Card creaturePrefab;
    GeneratorData _generatorData;
    ConfigData _config;


    public override void Init(GeneratorData generatorData, ConfigData config)
    {
        _config = config;
        _generatorData = generatorData;
        creaturePrefab = generatorData.CreaturePrefab;
        InitChances();
    }

    void InitChances()
    {
        foreach (var item in creatures)
        {
            SpawnChances.Add(item.ChanceGeneration);
            RotateChances.Add(item.ChanceRotation);
        }
    }


    public override Card Spawn( LevelTheme theme)
    {
        var card = Instantiate(creaturePrefab);
        card.Init(GetRandomCard(), theme.Data.Theme, _config);
        card.Set(_generatorData );
        return card;
    }


    public CardDataCreature GetRandomCard()
    {
        var r = Random.Range(0, 100) * 0.01f;
        var sum = 0f;

        for (var i = 0; i < creatures.Count; i++)
        {
            var chance = GetChance(i);
            sum += chance;
            if (r <= sum)
            {
                return creatures[i];
            }
        }

        Debug.LogError("Null returned");
        return null;
    }

    public float GetChance(int curveId)
    {
        if (creatures.Count == 1) return 1;
        
        var factor = 1 / chanceFactor;

        var point = (float) curveId / (creatures.Count - 1);
        var value = curvesChance.Evaluate(point);

        var factorValue = value + factor;
        var factorTotal = TotalChance + factor * creatures.Count;

        return factorValue / factorTotal;
    }

    float TotalChance => creatures
        .Select((t, i) => i / (float) (creatures.Count - 1))
        .Sum(curvesChance.Evaluate);
}