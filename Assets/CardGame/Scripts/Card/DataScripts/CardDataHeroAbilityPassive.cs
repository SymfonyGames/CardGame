 
using Misc;
using NaughtyAttributes;
using Plugins.AudioManager.audio_Manager;
using UnityEngine;

[CreateAssetMenu(fileName = "New Hero Ability Passive", menuName = "Card/Hero Ability Passive")]
public class CardDataHeroAbilityPassive : ScriptableObject
{
    [SerializeField] string cardName;
    [SerializeField] string description;
    [SerializeField] SoundData sound;
    [SerializeField] [ShowAssetPreview] Sprite icon;
    [Space(10)]
    [SerializeField] HeroAbilityPassive prefab;
 
    public HeroAbilityPassive Prefab => prefab;
 
    public Sprite Icon => icon;
    public SoundData Sound => sound;
    public string Description => description;
    public string Name => cardName;
}
