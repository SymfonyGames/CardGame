using System.Collections.Generic;
using System.Linq;
using Level;
using Misc;
using UnityEngine;

public class GeneratorBoss : Generator
{
    [SerializeField] AnimationCurve curvesChance;
    [SerializeField] float chanceFactor = 4f;
    [Header("Create: big")]
    [SerializeField] List<CardDataBoss> bigBosses = new();
    Card bossPrefab;
    GeneratorData _generatorData;
    public IReadOnlyList<CardDataBoss> Items => bigBosses;
    public override void Init(GeneratorData generatorData, ConfigData configData)
    {
        _generatorData = generatorData;
        bossPrefab = generatorData.BossPrefab;
        InitChances();
    }

    void InitChances()
    {
        foreach (var item in bigBosses)
        {
            SpawnChances.Add(item.ChanceGeneration);
            RotateChances.Add(item.ChanceRotation);
        }
    }

    public override Card Spawn( LevelTheme theme)
    {
        var card = Instantiate(bossPrefab);
        card.Init(GetRandomCard(), theme.Data.Theme);
        card.Set(_generatorData );
        return card;
    }
    
    public CardDataBoss GetRandomCard()
    {
        var r = Random.Range(0, 100) * 0.01f;
        var sum = 0f;

        for (var i = 0; i < bigBosses.Count; i++)
        {
            var chance = GetChance(i);
            sum += chance;
            if (r <= sum)
            {
                return bigBosses[i];
            }
        }

        Debug.LogError("Null returned");
        return null;
    }

    public float GetChance(int curveId)
    {
        if (bigBosses.Count == 1) return 1;
        
        var factor = 1 / chanceFactor;

        var point = (float) curveId / (bigBosses.Count - 1);
        var value = curvesChance.Evaluate(point);

        var factorValue = value + factor;
        var factorTotal = TotalChance + factor * bigBosses.Count;

        return factorValue / factorTotal;
    }

    float TotalChance => bigBosses
        .Select((t, i) => i / (float) (bigBosses.Count - 1))
        .Sum(curvesChance.Evaluate);
}