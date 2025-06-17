using UnityEngine;

namespace TESTS
{
    public class test_int : MonoBehaviour
    {
        public int score;

        public int tapScore;

        public float n;

        public float k;

// Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            score = (int) (tapScore * (1 + n * k));
        }
    }
}
