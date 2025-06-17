using System.Collections.Generic;
using Level;
using UnityEngine;

namespace TESTS
{
    [ExecuteInEditMode]
    public class TEST_SAVE_SO : MonoBehaviour
    {
        [SerializeField] private List<LevelSequenceData> data;
        public LevelSequenceData addData;
        [SerializeField] private List<LevelSequenceData> loaded;
        string saveKey = "savekeyyky";
        public bool save;
        public bool load;
        public bool add;
        private TestSave saveData;

        public class TestSave
        {
            public List<LevelSequenceData> _type = new List<LevelSequenceData>();
        }


        void Update()
        {
            if (add)
            {
                add = false;
                var equal = data.Find(e=>e==addData);
                if (equal)
                {
                    Debug.LogWarning("Ops");
                }
                else
                {
                    data.Add(addData);
                }
            }

            if (save)
            {
                save = false;
                saveData = new TestSave();
                saveData._type = data;
                PlayerPrefs.SetString(saveKey, JsonUtility.ToJson(saveData));
            }

            if (load)
            {
                load = false;

                loaded = PlayerPrefs.HasKey(saveKey)
                    ? JsonUtility.FromJson<TestSave>(PlayerPrefs.GetString(saveKey))._type
                    : new List<LevelSequenceData>();
            }
        }
    }
}