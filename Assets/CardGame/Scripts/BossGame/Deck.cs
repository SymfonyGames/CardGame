using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;

namespace BossGame
{
    public class Deck : MonoBehaviour
    {
        [Header("DEBUG")]
        [SerializeField]  [ReadOnly] ActionStats _actionStats;
        public ActionStats Actions => _actionStats;

        public void Init(ActionStats actionStats   )
        {
            _actionStats = actionStats;
 
        }

      
    }
}