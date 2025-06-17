using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Misc;
using UnityEngine;
using UnityEngine.Serialization;

[ExecuteInEditMode]
public class GeneratorList : MonoBehaviour
{
    [SerializeField] AnimationCurve curvesChance;
    [SerializeField] float chanceFactor = 4f;
    [SerializeField] [Range(0, 1)] float emptyCellChance;
    [SerializeField] [Range(0.51f, 0.9f)] float roadChanceStart;
    [SerializeField] [Range(0, 0.5f)] float roadChanceEnd;
    [SerializeField] [Range(0, 1f)] float portalChance;
    [SerializeField] Generator portals;
    public float GetRoadChance(float progress)
    {
        var abs = roadChanceStart - roadChanceEnd;
        var val = abs * progress;
        return roadChanceStart - val;
    }
    public float EmptyCellChance => emptyCellChance;
 

    [Header("Generators")]
    [SerializeField] GeneratorHero heroCreator;
    [SerializeField] List<Generator> generators = new List<Generator>();
    public IReadOnlyList<Generator> Items => GetItems();
    [HideInInspector] public List<Generator> items = new();
    [HideInInspector] public List<float> curveChances = new();
    [Range(0, 1f)] public List<float> chances = new();

    void Awake()
    {
        items = new List<Generator>();
        curveChances = new List<float>();

        foreach (var gen in generators)
        {
            if (gen is GeneratorBoss) continue;
            if (gen is GeneratorBossMini) continue;
            items.Add(gen);
        }

        for (int i = 0; i < items.Count; i++)
        {
            curveChances.Add(GetChance(i));
        }
    }

    IReadOnlyList<Generator> GetItems()
    {
        var newList = new List<Generator>();
        foreach (var gen in generators)
        {
            if (gen is GeneratorBoss) continue;
            if (gen is GeneratorBossMini) continue;
            newList.Add(gen);
        }

        return newList;
    }

    public GeneratorHero HeroCreator => heroCreator;
    public ReadOnlyCollection<Generator> Generators => generators.AsReadOnly();

    public bool IsFinalBoss => generators.OfType<GeneratorBoss>().Any();
    public bool IsMiniBoss => generators.OfType<GeneratorBossMini>().Any();

    public GeneratorBoss BossGenerator => generators.OfType<GeneratorBoss>().FirstOrDefault();
    public GeneratorBossMini MiniBossGenerator => generators.OfType<GeneratorBossMini>().FirstOrDefault();

    public void InitGenerators(GeneratorData generatorData, ConfigData config)
    {
        //    FindGeneratorsInChild();
        heroCreator.Init(generatorData);
        foreach (var item in generators)
        {
            if (item.gameObject.activeSelf)
                item.Init(generatorData, config);
        }
    }

    void FindGeneratorsInChild()
    {
        var childGenerators = GetComponentsInChildren<Generator>();
        generators = new List<Generator>();
        foreach (var item in childGenerators)
        {
            generators.Add(item);
        }
    }

    public float GetChance(int curveId)
    {
        var factor = 1 / chanceFactor;

        var point = (float) curveId / (Items.Count - 1);
        var value = curvesChance.Evaluate(point);

        var factorValue = value + factor;
        var factorTotal = TotalChance + factor * Items.Count;

        return factorValue / factorTotal;
    }

    float TotalChance => Items
        .Select((t, i) => i / (float) (Items.Count - 1))
        .Sum(curvesChance.Evaluate);

    public float PortalChance
    {
        get => portalChance;
        set => portalChance = value;
    }

    public Generator Portals
    {
        get => portals;
        set => portals = value;
    }
}