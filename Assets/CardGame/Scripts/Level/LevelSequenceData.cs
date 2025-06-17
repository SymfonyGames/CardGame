using System.Collections.Generic;
using Plugins.ColorAttribute;
using UnityEngine;

namespace Level
{
    [System.Serializable]
    public class LevelElement
    {
        [ContentColor(1f, 1, 1f, 1f)] public LevelData level;
        [ContentColor(1f, 1f, 0f, 1f)] public int goldReward;
        [ContentColor(0f, 0.8f, 0.8f, 1f)] public int gemReward;
        [ContentColor(1f, 1, 1f, 1f)] public int enemyLinesTotal = 10;
    }

    [CreateAssetMenu(fileName = "Level Sequence", menuName = "Level/Sequence")]
    public class LevelSequenceData : ScriptableObject
    {
        [ContentColor(1f, 1, 1f, 1f)]
        [SerializeField] private List<LevelElement> sequence;

        public string GetLevelName(int id) => id >= sequence.Count ? string.Empty : sequence[id].level.LevelName;
        public int GetTotalEnemyLines(int id) => id >= sequence.Count ? 1000 : sequence[id].enemyLinesTotal;
        public int GemReward(int id) => id >= sequence.Count ? 0 : sequence[id].gemReward;
        public int GoldReward(int id) => id >= sequence.Count ? 0 : sequence[id].goldReward;
        public int Count => sequence.Count;
    }
}