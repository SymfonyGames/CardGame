using System;
using UnityEngine;
using UnityEngine.UI;

namespace Maps
{
    public class MapPointer : MonoBehaviour
    {
        [SerializeField] int pointId;
        [Header("Setup")]
        [SerializeField] Image image;
        [SerializeField] Button button;
        [SerializeField] RectTransform myRectTransform;
        public float OffsetByX { get; private set; }
        public bool IsLock { get; private set; }
        public int Id => pointId;

        public event Action<MapPointer> OnClick;

        void Awake()
        {
            button.onClick.AddListener(CLick);
        }

        void CLick()
        {
            Debug.Log("Pointer click");
            OnClick?.Invoke(this);
        }

        public void SetSprite(Sprite sprite)
        {
            image.sprite = sprite;
        }

        public void SetOffset(float baseOffset)
        {
            OffsetByX = baseOffset - myRectTransform.localPosition.x;
        }

        public void SetLock(bool isLock)
        {
            IsLock = isLock;
        }

        void OnDrawGizmos()
        {
            if (!Application.isPlaying) return;
            if (!gameObject.activeSelf) return;
            
            Gizmos.color = IsLock ? Color.gray : Color.green;
            Gizmos.DrawSphere(transform.position,0.1f);
        }
    }
}