using DG.Tweening;
using UnityEngine;

namespace Misc
{
    public class ArrowUIanimation : MonoBehaviour
    {
        [SerializeField] RectTransform moveObject;
        [SerializeField] Vector3 offset;
        [SerializeField] float animTime;
    
    
 
        void Start()
        {
            moveObject.DOLocalMove(moveObject.transform.localPosition + offset, animTime)
                .SetLoops(-1,LoopType.Restart)
                .SetEase(Ease.InOutQuint);
        }

     
    }
}
