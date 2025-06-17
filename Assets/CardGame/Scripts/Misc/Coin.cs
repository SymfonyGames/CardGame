using UnityEngine;

namespace Misc
{
    public class Coin : MonoBehaviour
    {
        [SerializeField] Transform coinTransform;
        CoinsPool _pool;
        public Transform CoinTransform => coinTransform;

        public void Init(CoinsPool pool)
        {
            _pool = pool;
        }

        public void ReturnToPool() => _pool.ReturnToPool(this);
    }
}