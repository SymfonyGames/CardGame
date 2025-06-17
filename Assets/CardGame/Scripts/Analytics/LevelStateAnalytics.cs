/*using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelStateAnalytics : MonoBehaviour
{
    [SerializeField] private LevelProgress _levelProgress;
    [SerializeField] private EventManager _eventManager;
    private float _levelStartTime;
    private string _sceneName = string.Empty;

    private string LevelName
    {
        get
        {
            if (_sceneName.Length > 0) return _sceneName;
            _sceneName = SceneManager.GetActiveScene().name;
            _sceneName = _sceneName.ToLower();
            _sceneName = _sceneName.Replace(' ', '_');
            return _sceneName;
        }
    }


    private void Start()
    {
        //_levelProgress.OnProgressChanged += OnLevelProgressChanged;
        _levelStartTime = Time.realtimeSinceStartup;
        OnLevelStarted();
    }

    private void OnEnable()
    {
        _eventManager.OnLevelLose += OnLevelFailed;
    }

    private void OnDisable()
    {
        _eventManager.OnLevelLose -= OnLevelFailed;
    }

    private void OnLevelStarted()
    {
        var parameters = new Dictionary<string, object>();
        parameters.Add("name", LevelName);
        AppMetrica.Instance.ReportEvent("level_started", parameters);
#if UNITY_EDITOR
        DebugLogEvent("level_started", parameters);
#endif
    }

    private void OnLevelProgressChanged()
    {
        if (!_levelProgress.IsProgressComplete) return;
        var parameters = new Dictionary<string, object>();
        parameters.Add("name", LevelName);
        parameters.Add("state", "win");
        parameters.Add("duration", Time.realtimeSinceStartup - _levelStartTime);
        AppMetrica.Instance.ReportEvent("level_complete", parameters);
#if UNITY_EDITOR
        DebugLogEvent("level_complete", parameters);
#endif
    }

    private void OnLevelFailed()
    {
        var parameters = new Dictionary<string, object>();
        parameters.Add("name", LevelName);
        parameters.Add("state", "failed");
        parameters.Add("progress", _levelProgress.Progress);
        parameters.Add("duration", Time.realtimeSinceStartup - _levelStartTime);
        AppMetrica.Instance.ReportEvent("level_complete", parameters);
#if UNITY_EDITOR
        DebugLogEvent("level_complete", parameters);
#endif
    }

    private void DebugLogEvent(string eventName, Dictionary<string, object> parameters)
    {
        var sb = new StringBuilder();
        sb.Append($"Event: {eventName}");
        sb.Append('\n');
        foreach(var key in parameters.Keys)
        {
            sb.Append(key);
            sb.Append(": ");
            sb.Append(parameters[key]);
            sb.Append('\n');
        }
        Debug.Log(sb.ToString());
    }
}
*/