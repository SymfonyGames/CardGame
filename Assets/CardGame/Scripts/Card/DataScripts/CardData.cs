using NaughtyAttributes;
using Plugins.AudioManager.audio_Manager;
using UnityEngine;





public class CardData : ScriptableObject
{
    [SerializeField] string cardName;
    [SerializeField] string description;
    [SerializeField] SoundData interactSound;
    [SerializeField] [ShowAssetPreview] Sprite artwork;
    [Space(30)]
    [Range(0f, 1f)] [SerializeField] float chanceGeneration;
    [Range(0f, 1f)] [SerializeField] float chanceRotation;
    
    public float ChanceGeneration => chanceGeneration;
    public float ChanceRotation => chanceRotation;
    public Sprite Art => artwork;
    public SoundData InteractSound => interactSound;
    public string Name => cardName;
    public string Description => description;
}