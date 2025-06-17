using UnityEngine;

namespace Misc
{
    public class Gem : MonoBehaviour
    {
        [SerializeField] Transform coinTransform;
        GemPool _pool;
        public Transform CoinTransform => coinTransform;

        public void Init(GemPool pool)
        {
            _pool = pool;
        }

        public void ReturnToPool() => _pool.ReturnToPool(this);
    }
}
