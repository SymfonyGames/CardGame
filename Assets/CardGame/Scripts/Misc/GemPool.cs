using System.Collections.Generic;
using UnityEngine;

namespace Misc
{
    public class GemPool : MonoBehaviour
    {
        [SerializeField] Gem prefab;
        readonly Queue<Gem> _myQueue = new Queue<Gem>();

        #region Editor
#if (UNITY_EDITOR)
        int poolLenght = 0;
        string defaultName;
        void Start() => defaultName = name;
#endif


        #endregion


        public Gem GetCoin()
        {
            if (_myQueue.Count <= 0) return Create();

            var gem = _myQueue.Dequeue();
            gem.gameObject.SetActive(true);
            return gem;
        }

        Gem Create()
        {
            var gem = Instantiate(prefab, transform);
            gem.Init(this);

            #region Editor

#if (UNITY_EDITOR)
            poolLenght++;
            gameObject.name = defaultName + " (" + poolLenght + ")";
#endif

            #endregion

            return gem;
        }
        public void ReturnToPool(Gem gem)
        {
            _myQueue.Enqueue(gem);
            gem.gameObject.SetActive(false);
        }
    }
}
