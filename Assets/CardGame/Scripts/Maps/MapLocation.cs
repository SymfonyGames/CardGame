using System;
using System.Linq;
using Level;
using UnityEngine;

namespace Maps
{
    [ExecuteInEditMode]
    public class MapLocation : MonoBehaviour
    {
        [SerializeField] private ThemeData theme;
        public ThemeData Theme => theme;
        public MapPointer[] Pointers { get; private set; }
        
        public event Action<MapLocation,MapPointer> OnPointerClick;

        public void Init()
        {
            var pointers = GetComponentsInChildren<MapPointer>();
            Pointers = pointers.OrderBy(p => p.Id).ToArray();

            var myRect = GetComponent<RectTransform>();
            foreach (var pointer in Pointers)
            {
                pointer.SetOffset(myRect.rect.width / 2 - myRect.localPosition.x);
                pointer.OnClick += PointerClick;
            }
        }

        private void PointerClick(MapPointer pointer)
        {
            OnPointerClick?.Invoke(this,pointer);
        }
    }
}