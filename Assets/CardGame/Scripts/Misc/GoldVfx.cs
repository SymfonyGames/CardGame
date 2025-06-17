 
using UnityEngine;

namespace Misc
{
    public class GoldVfx : MonoBehaviour
    {
        [SerializeField] ParticleSystem particle;

        public ParticleSystem Particle => particle;
        GoldPool _pool;
        public void Init(GoldPool pool) => _pool = pool;

        void OnDisable() => _pool.ReturnToPool(this);
    }
}