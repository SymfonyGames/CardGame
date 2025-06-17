using System.Collections.Generic;
using UnityEngine;

namespace DataSave
{
    public class DataStorage : MonoBehaviour
    {
        const string MAP_SAVE_KEY = "MAP_SAVE_KEY";
 
        class MapsList
        {
            public List<MapSave> maps;
        }

 
        public List<MapSave> LoadMaps()
        {
            return PlayerPrefs.HasKey(MAP_SAVE_KEY)
                ? JsonUtility.FromJson<MapsList>(PlayerPrefs.GetString(MAP_SAVE_KEY)).maps
                : new List<MapSave>();
        }

 
        public void SaveMaps(List<MapSave> maps)
        {
            var dataSave = new MapsList {maps = maps};
            PlayerPrefs.SetString(MAP_SAVE_KEY, JsonUtility.ToJson(dataSave));
        }
    }
}