using System.Reflection;
using UnityEditor;
using UnityEngine;

public  class ClearMenuEditor : EditorWindow
{
    [MenuItem("CLEAR/PlayerPrefs &z")]
    static void DeleteAllPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
       Debug.Log( "[CLEAR] " + "Player prefs");
    }

    [MenuItem("GameObject/ActiveToggle _a")]
    static void ToggleActivationSelection()
    {
        foreach(var go in Selection.gameObjects)
            go.SetActive(!go.activeSelf);
    }

    [MenuItem("CLEAR/Console &c")]
    static void ClearConsole()
    {
        var assembly = Assembly.GetAssembly(typeof(UnityEditor.Editor));
        var logEntries = assembly.GetType("UnityEditor.LogEntries");
        var clearConsoleMethod = logEntries.GetMethod("Clear");
        clearConsoleMethod?.Invoke(new object(), null);
        Debug.ClearDeveloperConsole();
    }
}