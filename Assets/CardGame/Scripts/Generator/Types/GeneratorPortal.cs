using System.Collections.Generic;
using System.Linq;
using Level;
using Misc;
using NaughtyAttributes;
using Player;
using UnityEngine;

public class GeneratorPortal : Generator
{
    [SerializeField] AnimationCurve curvesChance;
    [SerializeField] float chanceFactor = 4f;
    [Header("Create:")] 
    [SerializeField] [Expandable] List<CardDataPortal> portals = new();
  
    public IReadOnlyList<CardDataPortal> Items => portals;
    Card portalPrefab;
    GeneratorData _generatorData;


    public override void Init(GeneratorData generatorData, ConfigData configData)
    {
        
        _generatorData = generatorData;
        portalPrefab = generatorData.PortalPrefab;
        InitChances();
    }
    void InitChances()
    {
        foreach (var item in portals)
        {
            SpawnChances.Add(item.ChanceGeneration);
        } 
    }
    
    
    public override Card Spawn(  LevelTheme theme)
    {
        var card = Instantiate(portalPrefab);
        card.Init(GetRandomCard(), theme.Data.Theme);
        card.Set(_generatorData );
        return card;
    }
    
    public CardDataPortal GetRandomCard()
    {
        var r = Random.Range(0, 100) * 0.01f;
        var sum = 0f;

        for (var i = 0; i < portals.Count; i++)
        {
            var chance = GetChance(i);
            sum += chance;
            if (r <= sum)
            {
                return portals[i];
            }
        }

        Debug.LogError("Null returned");
        return null;
    }

    public float GetChance(int curveId)
    {
        if (portals.Count == 1) return 1;
        
        var factor = 1 / chanceFactor;

        var point = (float) curveId / (portals.Count - 1);
        var value = curvesChance.Evaluate(point);

        var factorValue = value + factor;
        var factorTotal = TotalChance + factor * portals.Count;

        return factorValue / factorTotal;
    }

    float TotalChance => portals
        .Select((t, i) => i / (float) (portals.Count - 1))
        .Sum(curvesChance.Evaluate);
}
