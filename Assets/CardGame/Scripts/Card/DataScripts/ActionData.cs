using BossGame.Actions;
using NaughtyAttributes;
using UnityEngine;

public class ActionData : ScriptableObject
{
    [SerializeField] [ShowAssetPreview] Sprite art;
    [SerializeField] ActionCard prefab;
    [SerializeField]   float chargeBonus;
    public ActionCard Prefab => prefab;

    public Sprite Art => art;

    public float ChargeBonus => chargeBonus;
}