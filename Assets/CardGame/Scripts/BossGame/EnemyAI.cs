using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BossGame.Actions;
using Managers;
using UnityEngine;

namespace BossGame
{
    public class EnemyAI : MonoBehaviour
    {
        [SerializeField] GamePlayer player;

        public void Init()
        {
            EventManager.Instance.OnEnemyDropCard+=Drop;
          //  player.OnCardsTaken += ChooseCards;
          Drop();
        }

        void Drop()
        {
          Invoke(nameof(ChooseCards),2f);
        }

        void ChooseCards()
        {
            StartCoroutine(Choose());
        }

        IEnumerator Choose()
        {
            yield return null;

            var taken = new List<ActionCard>();
            var hands = player.Cards.InHand.ToList();

            while (taken.Count < player.Table.Size && hands.Count > 0)
            {
                var r = Random.Range(0, hands.Count);
                var c = hands[r];

                taken.Add(c);
                hands.Remove(c);

                yield return null;
            }

            foreach (var card in taken)
            {
                player.CardsMove.MoveByFree(card);
            }
        }
    }
}