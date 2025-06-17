using Managers;
using Misc;
using UnityEngine;

namespace Tutorials.items
{
    public class TutorialBlockSpawn : MonoBehaviour
    {
        [SerializeField] private GameStart gameStart;
        [SerializeField] private InteractSystem interactSystem;
        public bool disablePlayerActions;
        private void Awake()
        {
            gameStart.enabled = false;
            interactSystem.SetAutoSpawn(false);
            if (disablePlayerActions)
                interactSystem.enablePlayerActions = false;
        }
    }
}