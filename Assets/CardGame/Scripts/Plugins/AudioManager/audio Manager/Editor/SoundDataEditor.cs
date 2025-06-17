using Plugins.AudioManager.audio_Manager;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SoundData), true)]
public class SoundDataEditor : Editor
{

    [SerializeField] private AudioSource _previewer;

    public void OnEnable()
    {
        _previewer = EditorUtility.CreateGameObjectWithHideFlags("TestSound", HideFlags.HideAndDontSave, typeof(AudioSource)).GetComponent<AudioSource>();
    }

    public void OnDisable()
    {
        DestroyImmediate(_previewer.gameObject);
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        // EditorGUI.BeginDisabledGroup(serializedObject.isEditingMultipleObjects);
        GUILayout.Space(20);
        if (GUILayout.Button("TestSound"))
        {
            ((SoundData)target).EditorTest(_previewer);
        }
        // EditorGUI.EndDisabledGroup();
    }
}
