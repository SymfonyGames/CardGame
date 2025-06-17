using System.Collections.Generic;
using System.Linq;
using Level;
using Misc;
using UnityEngine;


public class GeneratorBossMini : Generator
{
    [SerializeField] AnimationCurve curvesChance;
    [SerializeField] float chanceFactor = 4f;
    [Header("Spawn at:")]
    [SerializeField] [Range(0, 1)] List<float> spawnBossAtProgress = new() {0.5f};
    [Header("Create: mini")]
    [SerializeField] List<CardDataBoss> miniBoss = new();

    Card bossPrefab;
    GeneratorData _generatorData;
    public IReadOnlyList<CardDataBoss> Items => miniBoss;
    public IReadOnlyList<float> SpawnsAtProgress => spawnBossAtProgress;

    public override void Init(GeneratorData generatorData, ConfigData configData)
    {
        _generatorData = generatorData;
        bossPrefab = generatorData.BossPrefab;
        InitChances();
    }

    void InitChances()
    {
        foreach (var item in miniBoss)
        {
            SpawnChances.Add(item.ChanceGeneration);
            RotateChances.Add(item.ChanceRotation);
        }
    }

    public override Card Spawn(LevelTheme theme)
    {
        var card = Instantiate(bossPrefab);
        //Debug.LogError("mini boss created: ", card);
        card.Init(GetRandomCard(), theme.Data.Theme);
        card.Set(_generatorData);
        if (card is CardBoss b) b.IsMiniBoss = true;
        return card;
    }

    public CardDataBoss GetRandomCard()
    {
        var r = Random.Range(0, 100) * 0.01f;
        var sum = 0f;

        for (var i = 0; i < miniBoss.Count; i++)
        {
            var chance = GetChance(i);
            sum += chance;
            if (r <= sum)
            {
                return miniBoss[i];
            }
        }

        Debug.LogError("Null returned");
        return null;
    }

    public float GetChance(int curveId)
    {
        if (miniBoss.Count == 1) return 1;

        var factor = 1 / chanceFactor;

        var point = (float) curveId / (miniBoss.Count - 1);
        var value = curvesChance.Evaluate(point);

        var factorValue = value + factor;
        var factorTotal = TotalChance + factor * miniBoss.Count;

        return factorValue / factorTotal;
    }

    float TotalChance => miniBoss
        .Select((t, i) => i / (float) (miniBoss.Count - 1))
        .Sum(curvesChance.Evaluate);
}