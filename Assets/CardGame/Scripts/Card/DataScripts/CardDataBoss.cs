using System.Collections.Generic;
using System.Linq;
using BossGame;
using UnityEngine;

[CreateAssetMenu(fileName = "New Hero", menuName = "Card/Boss")]
public class CardDataBoss : CardDataUnit
{
    [SerializeField] float hpMin;
    [SerializeField] float hpMax;
    [SerializeField] int attackInterval=3;
    [Space(20)]
    [HideInInspector] [SerializeField] PlayerStats stats;
    [Space(20)]
    [Header("ACTIONS")]
    [HideInInspector]  [SerializeField] ActionStats actionStats;
    [HideInInspector]  [SerializeField] List<float> chances = new();
    [Header("SPECIAL ACTIONS")]
    [HideInInspector]  [SerializeField] ActionStats attacks;
    [HideInInspector]  [SerializeField] ActionStats defense;
    [HideInInspector]  [SerializeField] ActionStats ultimate;
    public ActionStats Attacks => attacks;
    public ActionStats Defense => defense;
    public ActionStats Ultimate => ultimate;
    [SerializeField] int lives = 1;
    public float GetChance(int curveId)
    {
        var factor = 1 / actionStats.factor;

        var point = (float) curveId / (actionStats.actions.Count - 1);
        var value = actionStats.curve.Evaluate(point);

        var factorValue = value + factor;
        var factorTotal = TotalChance + factor * actionStats.actions.Count;

        return factorValue / factorTotal;
    }

    float TotalChance => actionStats.actions
        .Select((t, i) => i / (float) (actionStats.actions.Count - 1))
        .Sum(actionStats.curve.Evaluate);


    public PlayerStats Stats => stats;

    public ActionStats Actions => actionStats;

    public int Lives => lives;

    public float HpMin => hpMin;

    public float HpMax => hpMax;

    public int AttackInterval => attackInterval;

    void OnValidate()
    {
        chances = new List<float>();
        if (actionStats.actions.Count > 0)
        {
            for (var i = 0; i < actionStats.actions.Count; i++)
            {
                var chance = (int) (GetChance(i) * 100f);
                chances.Add(chance);
            }
        }
    }
}