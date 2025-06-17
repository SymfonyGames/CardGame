using Misc;
using UnityEngine;

[System.Serializable]
public class PotionData
{
    public PotionType type;
    [SerializeField] int min;
    [SerializeField] int max;
    public int Value => Random.Range(min, max + 1);
}