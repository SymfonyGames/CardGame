using BossGame.Actions;
using Managers;
using UnityEngine;

namespace BossGame
{
    // ReSharper disable once IdentifierTypo
    public class CardsMover : MonoBehaviour
    {
        Cards _cards;
        Field _table;
        Interactor _interactor;

        public void Init(Cards cards, Field table, Interactor interactor)
        {
            _interactor = interactor;
            _table = table;
            _cards = cards;
            EventManager.Instance.OnActionTouch += Move;
        }

        public void Move(ActionCard card)
        {
            if (_cards.Contains(card))
            {
                if (_table.IsFull) return;
               // _table.Put(card, _interactor.step);
                _table.Put(card);
                _cards.Remove(card);
            }
        }
        public void MoveByFree(ActionCard card)
        {
            if (_cards.Contains(card))
            {
                if (_table.IsFull) return;
                _table.Put(card );
                _cards.Remove(card);
            }
        }
    }
}