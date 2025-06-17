using System.Collections.Generic;
using Level;
using Plugins.AudioManager.audio_Manager;
using UnityEngine;

namespace Maps
{
    [CreateAssetMenu(fileName = "Map Levels", menuName = "Level/MapLevels")]
    public class MapData : ScriptableObject
    {
        [Header("Sound")]
        [SerializeField] private SoundData openSound;
        [SerializeField] private SoundData closeSound;
        [Header("Map icons")]
        [SerializeField] private Sprite mapLevelIconLocked;
        [SerializeField] private Sprite mapLevelIconDefault;
        [SerializeField] private Sprite mapLevelIconComplete;
        [SerializeField] private Sprite mapLevelIconLastUnlock;
        [Header("Hero chip animation")]
        [SerializeField] float chipMaxSize;
        [SerializeField] float chipMoveDuration;
        [Header("Map level sequences")]
        [SerializeField] private ThemeData startingTheme;
        [SerializeField] List<MapSequenceData> levelSequences;


        public Sprite MapLevelIconDefault => mapLevelIconDefault;
        public Sprite MapLevelIconLocked => mapLevelIconLocked;
        public Sprite MapLevelIconComplete => mapLevelIconComplete;
        public Sprite MapLevelIconLastUnlock => mapLevelIconLastUnlock;
        public IReadOnlyList<MapSequenceData> Sequences => levelSequences;
        public float ChipMaxSize => chipMaxSize;
        public float ChipMoveDuration => chipMoveDuration;

        public ThemeData StartingTheme => startingTheme;

        public SoundData OpenSound => openSound;

        public SoundData CloseSound => closeSound;
    }
}