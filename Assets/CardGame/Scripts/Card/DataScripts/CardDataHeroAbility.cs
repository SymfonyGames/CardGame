using System.Collections.Generic;
using Misc;
using NaughtyAttributes;
using Plugins.AudioManager.audio_Manager;
using UnityEngine;

[CreateAssetMenu(fileName = "New Hero Ability", menuName = "Card/Hero Ability")]
public class CardDataHeroAbility : ScriptableObject
{
    [SerializeField] string cardName;
    [SerializeField] string description;
    [SerializeField] SoundData sound;
    [SerializeField] [ShowAssetPreview] Sprite icon;
    [Space(10)]
    [SerializeField] HeroAbility prefab;
    [SerializeField] List<int> manaCost = new();
    [SerializeField] List<int> cooldown = new();

    public HeroAbility Prefab => prefab;
    public int ManaCost(int lvl) => lvl - 1 < manaCost.Count ? manaCost[lvl - 1 > 0 ? lvl - 1 : 0] : manaCost[^1];
    public int Cooldown(int lvl) => lvl - 1 < cooldown.Count ? cooldown[lvl - 1 > 0 ? lvl - 1 : 0] : cooldown[^1];
    public Sprite Icon => icon;
    public SoundData Sound => sound;
    public string Description => description;
    public string Name => cardName;
}