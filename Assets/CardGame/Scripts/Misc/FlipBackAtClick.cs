using UnityEngine;
using UnityEngine.EventSystems;

namespace Misc
{
    public class FlipBackAtClick : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] Card card;
        [HideInInspector] public bool isEnabled;

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!isEnabled) return;
            isEnabled = false;
            
            card.FlipBack();
        }
    }
}