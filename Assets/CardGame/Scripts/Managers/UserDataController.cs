using System.Collections.Generic;
using DataSave;
using Level;
using Plugins.LevelLoader;
using UnityEngine;

namespace Managers
{
    public class UserDataController : MonoBehaviour
    {
        [SerializeField] DataStorage dataStorage;

        List<MapSave> _mapSave;
        public IReadOnlyList<MapSave> MapSave => _mapSave;
        public bool IsInitialized { get; private set; }

        [Header("Debug")]
        public ThemeData lastTheme;
        public int lastCompleteLevelId;
        public int lastMapId;

        void Start()
        {
            Invoke(nameof(DelayedDebug), 1f);
        }

        void DelayedDebug()
        {
            lastTheme = MapSave[0].currentTheme;
            if (Loader.Instance)
                lastCompleteLevelId = Loader.Instance.CurrentLevel;
        }

        public void MapLevelComplete(int mapId, ThemeData theme, LevelSequenceData sequence, int completeLevelId)
        {
            lastMapId = mapId;
            lastTheme = theme;
            lastCompleteLevelId = completeLevelId;

            var seqData = _mapSave[mapId].sequencesData.Find(s => s.theme == theme);

            if (seqData != null)
            {
                if (!seqData.completeLevelIDs.Contains(completeLevelId))
                {
                    //Debug.LogError("Level completed: " + completeLevelId);
                    seqData.completeLevelIDs.Add(completeLevelId);
                }
            }
            else
            {
                var dataToAdd = new MapSequenceDataSave
                {
                    theme = theme,
                    sequence = sequence,
                    unlockedLvlIds = new List<int>() {completeLevelId},
                    completeLevelIDs = new List<int> {completeLevelId}
                };
                _mapSave[mapId].sequencesData.Add(dataToAdd);
            }

            SaveMapData();
        }

        public void MapLevelUnlock(int mapId, ThemeData theme, LevelSequenceData sequence, int unlockLevelId)
        {
            var seqData = _mapSave[mapId].sequencesData.Find(s => s.theme == theme);

            if (seqData != null)
            {
                if (!seqData.unlockedLvlIds.Contains(unlockLevelId))
                {
                    seqData.unlockedLvlIds.Add(unlockLevelId);
                }
            }
            else
            {
                var dataToAdd = new MapSequenceDataSave
                {
                    theme = theme,
                    sequence = sequence,
                    unlockedLvlIds = new List<int>() {unlockLevelId},
                    completeLevelIDs = new List<int> {unlockLevelId}
                };
                _mapSave[mapId].sequencesData.Add(dataToAdd);
            }

            SaveMapData();
        }

        public void SetMapData(IReadOnlyList<MapSave> mapSave)
        {
            _mapSave = mapSave as List<MapSave>;
            SaveMapData();
        }


        void Awake() => LoadData();

        void LoadData()
        {
            LoadMapData();
            IsInitialized = true;
        }

        void LoadMapData() => _mapSave = dataStorage.LoadMaps();


        public void SaveMapData() => dataStorage.SaveMaps(_mapSave);
    }
}