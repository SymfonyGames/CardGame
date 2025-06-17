using UnityEngine;

namespace MenuTabs
{
    public abstract class TabStateAnim : MonoBehaviour
    {
        public abstract void Active();
        public abstract void Inactive();
    }
}