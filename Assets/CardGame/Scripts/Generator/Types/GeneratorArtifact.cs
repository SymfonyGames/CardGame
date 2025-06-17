using System.Collections.Generic;
using System.Linq;
using Level;
using Misc;
using UnityEngine;

public class GeneratorArtifact : Generator
{
    [SerializeField] AnimationCurve curvesChance;
    [SerializeField] float chanceFactor = 4f;
    [Header("Create:")]
    [SerializeField] List<CardDataArtifact> artifacts = new();
    public IReadOnlyList<CardDataArtifact> Items => artifacts;
    Card artifactPrefab;
    GeneratorData _generatorData;


    public override void Init(GeneratorData generatorData, ConfigData configData)
    {
        _generatorData = generatorData;
        artifactPrefab = generatorData.ArtefactPrefab;
        InitChances();
    }

    void InitChances()
    {
        foreach (var item in artifacts)
        {
            SpawnChances.Add(item.ChanceGeneration);
            RotateChances.Add(item.ChanceRotation);
        }
    }


    public override Card Spawn(LevelTheme theme)
    {
        var card = Instantiate(artifactPrefab);
        var so = GetRandom();
        card.Init(so, theme.Data.Theme);
        card.Set(_generatorData);
        
      //  if (so.Potions.Count > 0) 
       //     card.forceEmptyMaterial = true;
        
        return card;
    }


    public CardDataArtifact GetRandom()
    {
        var r = Random.Range(0, 100) * 0.01f;
        var sum = 0f;

        for (var i = 0; i < artifacts.Count; i++)
        {
            var chance = GetChance(i);
            sum += chance;
            if (r <= sum)
            {
                return artifacts[i];
            }
        }

        Debug.LogError("Null returned");
        return null;
    }

    public float GetChance(int curveId)
    {
        if (artifacts.Count == 1) return 1;

        var factor = 1 / chanceFactor;

        var point = (float) curveId / (artifacts.Count - 1);
        var value = curvesChance.Evaluate(point);

        var factorValue = value + factor;
        var factorTotal = TotalChance + factor * artifacts.Count;

        return factorValue / factorTotal;
    }

    float TotalChance => artifacts
        .Select((t, i) => i / (float) (artifacts.Count - 1))
        .Sum(curvesChance.Evaluate);
}