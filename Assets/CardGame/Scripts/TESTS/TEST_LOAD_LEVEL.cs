using Level;
using Plugins.LevelLoader;
using UnityEngine;

namespace TESTS
{
    public class TEST_LOAD_LEVEL : MonoBehaviour
    {
        [Header("Load by name")]
        public string levelName;
        public bool test;
        [Header("Load by sequence")]
        public LevelSequenceData sequence;
        public int lvlId;
        public bool test2;
    
    
        // Update is called once per frame
        void Update()
        {
            if (test)
            {
                test = false;
                Loader.Instance.LoadLevel(levelName);
            }
        
            if (test2)
            {
                test2 = false;
                Loader.Instance.LoadLevel(sequence,lvlId);
            }
        }
    }
}
