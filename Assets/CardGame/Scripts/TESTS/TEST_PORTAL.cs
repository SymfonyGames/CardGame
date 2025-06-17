using Managers;
using UnityEngine;

namespace TESTS
{
    public class TEST_PORTAL : MonoBehaviour
    {
        public bool testOpenPortal;
        public CardDataPortal portal;
        void Update()
        {
            if (testOpenPortal)
            {
                testOpenPortal = false;
                EventManager.Instance.PortalOpen(portal);
            }
        }

    }
}
