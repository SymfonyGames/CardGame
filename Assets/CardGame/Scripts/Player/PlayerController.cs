using System;
using Managers;
using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        private Card _playerCard;

        public Card PlayerCard => _playerCard;

        void Start()
        {
            EventManager.Instance.OnPlayerCreated += Created;
            EventManager.Instance.OnPlayerRevive += Revive;
        }

        void OnDisable()
        {
            EventManager.Instance.OnPlayerCreated -= Created;
            EventManager.Instance.OnPlayerRevive -= Revive;
        }

        private void Created(Card card)
        {
            _playerCard = card;
        }

        private void Revive()
        {
            PlayerCard.Revive();
            PlayerCard.MoveBack();
            EventManager.Instance.CameraFocusDisable();
        }
    }
}