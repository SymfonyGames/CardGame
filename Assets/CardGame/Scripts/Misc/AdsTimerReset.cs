using Managers;
using UnityEngine;

namespace Misc
{
    public class AdsTimerReset : MonoBehaviour
    {
 
        void Start()
        {
            AdsManager.Instance.ResetAdsTimer();
        }

   
    }
}
