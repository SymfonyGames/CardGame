using Player;
using UnityEngine;

namespace TESTS
{
    public class TEST_PLAYER_RESOURCES : MonoBehaviour
    {
        [Header("Test")]
        public bool addResources;
        [Header("Setup")]
        public int Gold = 100;
        public int Gem = 100;

 

        private void Start()
        {
            if (addResources)
            {
                addResources = false;
                Invoke(nameof(DelayedAdd), 1f);
            }
        }

        void DelayedAdd()
        {
            Debug.Log("Trying to add player resources");
            PlayerStash_resources.Instance.AddGold((Gold));
            PlayerStash_resources.Instance.AddGem((Gem));
        }

#if UNITY_EDITOR
        private void Update()
        {
            if (addResources)
            {
                addResources = false;
                Debug.Log("Trying to add player resources");
                PlayerStash_resources.Instance.AddGold((Gold));
                PlayerStash_resources.Instance.AddGem((Gem));
            }
        }
#endif
    }
}