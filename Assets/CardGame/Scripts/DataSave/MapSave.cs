using System.Collections.Generic;
using Level;

namespace DataSave
{
    [System.Serializable]
    public class MapSave
    {
        public ThemeData currentTheme;
        public int currentPointId;
        public List<MapSequenceDataSave> sequencesData;
    }
}