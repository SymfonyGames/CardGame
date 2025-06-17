using DamageNumbersPro;
using Managers;
using UnityEngine;

namespace Misc
{
    public class FloatingTextPlayer : MonoBehaviour
    {
        public bool showExp = true;
        public DamageNumber expText;
        public Vector3 expOffset;
 
        void Start()
        {
            EventManager.Instance.OnExperienceDrop += SpawnExp;
        }

        void OnDisable()
        {
            EventManager.Instance.OnExperienceDrop -= SpawnExp;
        }

        void SpawnExp(Vector3 fromPos, float value)
        {
            if (showExp && expText)
                expText.Spawn(fromPos + expOffset, value);
        }
    }
}
