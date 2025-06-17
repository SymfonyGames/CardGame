using Managers;
using Player;
using TMPro;
using UnityEngine;

namespace TESTS
{
    public class TEST_HERO_STATUS : MonoBehaviour
    {

        private CardHero _hero;
        public TextMeshProUGUI txt;
        public bool actionsEnabled;
        public PlayerMove mover;
        private void Start()
        {
            EventManager.Instance.OnPlayerCreated += PlayerCreated;
            EventManager.Instance.OnDisablePlayer += DisableACtions;
            EventManager.Instance.OnEnablePlayer += EnableACtions;
        }

        private void EnableACtions()
        {
            Debug.LogError("ACTIONS ENABLED");
            txt.text = "STATUS: ENABLED";
        }

        private void DisableACtions()
        {
            Debug.LogError("ACTIONS DISABLED");
            txt.text = "STATUS: DISABLED";
        }

        private void PlayerCreated(Card obj)
        {
            if (obj is CardHero isHero)
            {
                _hero = isHero;
                mover = _hero.GetComponent<PlayerMove>();
            }
        }
        void Update()
        {
            if (mover)
                actionsEnabled = !mover.IsBusy;
        }


    }
}
