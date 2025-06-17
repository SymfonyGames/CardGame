using UnityEngine;

namespace Level
{
   [CreateAssetMenu(fileName = "New Level Data", menuName = "Level/Data")]
   public class LevelData : ScriptableObject
   {
   
      [Header("Name")]
      [SerializeField] string levelName;
   
      [Header("Theme")]
      [SerializeField] LevelThemeData theme;
   
      public string LevelName => levelName;
      public LevelThemeData Theme => theme;
   }
}
