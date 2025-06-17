using System.Collections.Generic;
using System.Collections.ObjectModel;
using NaughtyAttributes;
using Plugins.ColorAttribute;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "New Portal", menuName = "Card/Portal")]
public class CardDataPortal : CardData
{
    [Space(20)]
    [ContentColor(0.2f, 1f, 1f, 1f)]
    [SerializeField] bool flip = true;
    [SerializeField] int totalCards;
    [SerializeField] int allowToOpenCards;
 
    [FormerlySerializedAs("artifactsPool")]
    [SerializeField] [Expandable] List<CardDataArtifact> artifacts = new();

    public ReadOnlyCollection<CardDataArtifact> Cards => artifacts.AsReadOnly();
    public int TotalCards => totalCards;
    public bool isFlip => flip;
    public int AllowToOpenCards => allowToOpenCards;
}