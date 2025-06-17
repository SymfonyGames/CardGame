 using Level;
 using UnityEngine;

 namespace Tutorials.items
 {
     public class TutorialSetTotalLines : MonoBehaviour
     {
         public LevelProgress levelProgress;
         public int totalLines;

         private void Awake()
         {
             levelProgress.SetTotalLines(totalLines);
         }
 
     }
 }