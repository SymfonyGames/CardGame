using System;
using Managers;
using Plugins.LevelLoader;
using UnityEngine;

namespace Level
{
    public class LevelProgress : MonoBehaviour
    {
        #region Singleton

        //-------------------------------------------------------------
        public static LevelProgress Instance;

        void Awake()
        {
            if (Instance == null) Instance = this;
            else gameObject.SetActive(false);
        }

 
        //-------------------------------------------------------------

        #endregion

        [Header("References")]
        [SerializeField] LevelProgressUI progressUI;
        int _current;
        int _totalEnemyLines;
        
        bool isTotalLinesSet;
        public bool IsProgressComplete => _current >= _totalEnemyLines;
        public event Action OnProgressChanged = delegate { };
        public float Value => (float) _current / _totalEnemyLines;

        public LevelProgressUI ProgressUI => progressUI;


        void Start()
        {
            //  if (!isTotalLinesSet)
            //      SetTotalLines(Loader.Instance.GetTotalEnemyLines());

            EventManager.Instance.OnCardsMoveDown += NextLine;
        }
        void OnDisable()
        {
            EventManager.Instance.OnCardsMoveDown -= NextLine;
        }

        public void SetTotalLines(int total)
        {
            isTotalLinesSet = true;
            _totalEnemyLines = total;
            progressUI.Setup(_totalEnemyLines);
            RefreshUI();
        }

        public void NextLine(float moveTime)
        {
            _current++;
            OnProgressChanged();

            RefreshUI();
        }

        void RefreshUI()
        {
            float step = 1f / (float) _totalEnemyLines;
            float fill = (_current) * step;
            //var fillValue = (float) (_current) / (_totalEnemyLines-1);
            progressUI.Refresh(fill, _current);

            //Debug.LogError("current: " + _current);
            //Debug.LogError("step: " + step);
            //Debug.LogError("fill: " + fill);
        }
    }
}