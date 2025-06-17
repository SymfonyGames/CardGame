using System.Collections.Generic;
using UnityEngine;

namespace Misc
{
    public class CoinsPool : MonoBehaviour
    {
        [SerializeField] Coin coinPrefab;
        readonly Queue<Coin> _myQueue = new Queue<Coin>();

        #region Editor
#if (UNITY_EDITOR)
        int poolLenght = 0;
        string defaultName;
        void Start() => defaultName = name;
#endif
    

        #endregion

    
        public Coin GetCoin()
        {
            if (_myQueue.Count <= 0) return CreateCoin();

            var coin = _myQueue.Dequeue();
            coin.gameObject.SetActive(true);
            return coin;
        }
    
        Coin CreateCoin()
        {
            var coin = Instantiate(coinPrefab, transform);
            coin.Init(this);

            #region Editor

#if (UNITY_EDITOR)
            poolLenght++;
            gameObject.name = defaultName + " (" + poolLenght + ")";
#endif

            #endregion

            return coin;
        }
        public void ReturnToPool(Coin coin)
        {
            _myQueue.Enqueue(coin);
            coin.gameObject.SetActive(false);
        }
    }
}
