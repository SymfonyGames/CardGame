using Managers;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardTouch : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] Card card;

    void Awake()
    {
        if (!card)
            card = GetComponent<Card>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
       // card.Glow();
         EventManager.Instance.CardTouch(card);
    }
}