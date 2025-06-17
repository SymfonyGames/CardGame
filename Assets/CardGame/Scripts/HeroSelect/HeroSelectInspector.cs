#if UNITY_EDITOR

using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace HeroSelect
{
    [CustomEditor(typeof(HeroSelect))]
    public class HeroSelectInspector : Editor
    {
        HeroSelect script;
        void OnEnable() => script = (HeroSelect)target;

    
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            if (GUI.changed) SetSceneDirty();
        }
  
    
        void SetSceneDirty()
        {
            EditorUtility.SetDirty(script);
            EditorUtility.SetDirty(script.gameObject);
            EditorSceneManager.MarkSceneDirty(script.gameObject.scene);
            PrefabUtility.RecordPrefabInstancePropertyModifications(script.gameObject);
        }
    
//     public static void UpdatePrefab()
//     {
// #if UNITY_EDITOR
//  
//         var prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
//         if (prefabStage != null)
//         {
//             EditorSceneManager.MarkSceneDirty(prefabStage.scene);
//         }
// #endif
//     }

    }
}

#endif