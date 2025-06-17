using DG.Tweening;
using UnityEngine;

namespace TESTS
{
    [ExecuteInEditMode]
    public class TEST_MAP_CHIP_ANIMATION : MonoBehaviour
    {
        [Header("Test")]
        public bool move;
        [Header("Settings")]
        private float baseSize;
        public float maxSize;
        public float animTime;
        [Header("Setup")]
        public Transform pointer;
        public Transform moveTo;
    
        private void Awake()
        {
            baseSize = pointer.localScale.x;
        }

        void Update()
        {
            if (move)
            {
                move = false;
                pointer.DOMove(moveTo.position, animTime);
                pointer.DOScale(baseSize * maxSize, animTime / 2f)
                    .SetEase(Ease.InQuad)
                    .OnComplete(() =>
                        pointer.DOScale(baseSize, animTime / 2f)
                            .SetEase(Ease.OutQuad));
            }
        }
    }
}