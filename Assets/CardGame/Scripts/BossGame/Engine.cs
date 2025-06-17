using System;
using System.Collections;
using BossGame.Actions;
using Managers;
using UnityEngine;

// ReSharper disable InconsistentNaming

namespace BossGame
{
    public class Engine : MonoBehaviour
    {
        GamePlayer p1;
        GamePlayer p2;
        Interactor _interactor;

        void Start()
        {
            EventManager.Instance.OnTakeNewActions += NextCards;
        }

        void NextCards() => Invoke(nameof(TakeCards), 1f);

        public void Init(GamePlayer player1, GamePlayer player2, Interactor interactor)
        {
            _interactor = interactor;
            p1 = player1;
            p2 = player2;
            p1.Table.OnPut += CheckReady;
            //   p2.Table.OnPut += CheckReady;
        }

        void OnDisable()
        {
            p1.Table.OnPut -= CheckReady;
            //     p2.Table.OnPut -= CheckReady;
        }

        void CheckReady(ActionCard actionCard)
        {
        //    Invoke(nameof(Fight), 0.2f);
            StartCoroutine(Interact(actionCard));
            // if (p1.Table.NotFull) return;
            // if (p2.Table.NotFull) return;
            // Fight();
        }

        IEnumerator Interact(ActionCard card)
        {
            yield return new WaitForSeconds(0.2f);
            _interactor.Interact(card);
        }
        public void StartGame()
        {
            TakeCards();
        }

        public void TakeCards()
        {
            p1.TakeCards();
            p2.TakeCards();
        }

        public void Fight()
        {
            _interactor.Interact();
        }
    }
}