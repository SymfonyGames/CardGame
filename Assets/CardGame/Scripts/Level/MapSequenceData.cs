using System.Collections.Generic;

namespace Level
{
    [System.Serializable]
    public class MapSequenceData
    {
        public ThemeData theme;
        public LevelSequenceData sequence;
        public List<int> unlockedLvlIds = new List<int>();
    }

    [System.Serializable]
    public class MapSequenceDataSave : MapSequenceData
    {
        public List<int> completeLevelIDs = new List<int>();
    }
}