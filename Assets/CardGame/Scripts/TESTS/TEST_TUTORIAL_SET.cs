using DataSave;
using UnityEngine;

namespace TESTS
{
    public class TEST_TUTORIAL_SET : MonoBehaviour
    {

        void Awake()
        {
            PlayerPrefs.SetInt(SaveKeys.TUTORIAL_COMPLETE, 1);
        }
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
