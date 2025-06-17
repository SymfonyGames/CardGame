using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Misc
{
    public class GoldPool : MonoBehaviour
    {
        [SerializeField] GoldVfx coinPrefab;
        readonly Queue<GoldVfx> _myQueue = new Queue<GoldVfx>();

        #region Editor
#if (UNITY_EDITOR)
        int poolLenght = 0;
        string defaultName;
        void Start() => defaultName = name;
#endif
    

        #endregion

    
        public GoldVfx Get()
        {
            if (_myQueue.Count <= 0) return CreateCoin();

            var coin = _myQueue.Dequeue();
            coin.gameObject.SetActive(true);
            return coin;
        }
    
        GoldVfx CreateCoin()
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
        public void ReturnToPool(GoldVfx coin)
        {
            _myQueue.Enqueue(coin);
            coin.gameObject.SetActive(false);
        }
    }
}