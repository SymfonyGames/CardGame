using Managers;
using UnityEngine;

namespace TESTS
{
    public class TEST_COINSUI : MonoBehaviour
    {
        [Header("Test")]
        public bool playCoinAnimation;
        [Header("Setup")]
        public Transform fromTransform;


        void Update()
        {
            if (playCoinAnimation)
            {
                playCoinAnimation = false;
                EventManager.Instance.CreateGoldVFX(fromTransform.localPosition, 1);
            }
        }
    }
}