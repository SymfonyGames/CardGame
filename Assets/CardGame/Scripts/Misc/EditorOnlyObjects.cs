using UnityEngine;

namespace Misc
{
    public class EditorOnlyObjects : MonoBehaviour
    {
        [SerializeField] GameObject[] objects;

        void Awake()
        {
#if UNITY_EDITOR

            foreach (var obj in objects)
            {
                if (obj) obj.gameObject.SetActive(true);
            }
#else
        gameObject.SetActive(false);
#endif
        }
    }
}