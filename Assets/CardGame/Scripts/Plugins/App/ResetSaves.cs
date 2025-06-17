using UnityEngine;

namespace Plugins.App
{
   public class ResetSaves : MonoBehaviour
   {
      public void Reset()
      {
         PlayerPrefs.DeleteAll();
         Debug.LogWarning("Player prefs was CLEARED totally");
      }
   }
}
