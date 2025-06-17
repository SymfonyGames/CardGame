#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GeneratorList))]
public class CardDataBossEditor : UnityEditor.Editor
{
    Vector2 _scroll;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUILayout.Space();

        var script = (GeneratorList) target;
  

        if (script.Items.Count > 0)
        {
            _scroll = EditorGUILayout.BeginScrollView(_scroll, GUILayout.MaxHeight(300));
            for (var i = 0; i < script.Items.Count; i++)
            {
                EditorGUILayout.BeginHorizontal("box");
                var nam = script.Items[i] ? script.Items[i].name : "None";
                EditorGUILayout.LabelField(nam);

                var chance = (int) (script.GetChance(i) * 100f);

                EditorGUILayout.LabelField(chance + "%");
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndScrollView();
        }
  
    }
}
#endif