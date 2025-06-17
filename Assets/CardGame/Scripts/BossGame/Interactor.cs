using System;
using System.Linq;
using BossGame.Actions;
using DG.Tweening;
using Managers;
using UnityEngine;

// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming

namespace BossGame
{
    public class Interactor : MonoBehaviour
    {
        Field t1;
        Field t2;
        GamePlayer p1;
        GamePlayer p2;

        public void Init(GamePlayer player1, GamePlayer player2)
        {
            p1 = player1;
            p2 = player2;

            t1 = player1.Table;
            t2 = player2.Table;

            step = 0;
        }

        public int step = 0;
        public ActionCell cell1;
        public ActionCell cell2;

        public void Interact(ActionCard card)
        {
            step++;
            var pos = card.transform.position;

            if (card is AttackAction)
            {
                var findDefense = t2.Cells.FirstOrDefault(c => c.Card is DefenseAction);
                if (findDefense)
                {
                    var offset = new Vector3(0, 0, 0);
                    findDefense.Card.transform
                        .DOMove(pos + offset, 0.4f)
                        .OnComplete(() =>
                            DisableCards(card, findDefense.Card));
                    
                    Check();
                    return;
                }
            }

            if (card is DefenseAction)
            {
                var findAttack = t2.Cells.FirstOrDefault(c => c.Card is AttackAction);
                if (findAttack)
                {
                    var offset = new Vector3(0, 0, 0);
                    findAttack.Card.transform
                        .DOMove(pos + offset, 0.4f)
                        .OnComplete(() =>
                            DisableCards(card, findAttack.Card));

                    Check();
                    return;
                }
            }

            var card_2 = t2.GetCell.Card;
            UseCard(card, 0.2f);
            UseCard(card_2, 0.6f, true);
            Check();
    
           
        }

        void Check()
        {
            
            if (step >= 3)
            {
                step = 0;
                EventManager.Instance.TakeNewActions();
            }

            EventManager.Instance.EnemyDropCard();
        }

        void DisableCards(ActionCard c1, ActionCard c2)
        {
            c1.Disable();
            c2.Disable();
        }

        public void Interact()
        {
            cell1 = t1.Cells[step];
            cell2 = t2.Cells[step];

            var a1 = t1.Cells[step].Card;
            var a2 = t2.Cells[step].Card;

            if (IsAttackDefense(a1, a2))
            {
                Attack_Defense(a1, a2);
                step++;
                return;
            }

            UseCard(a1);
            UseCard(a2, 0.5f, true);
            step++;
            if (step >= 3)
            {
                step = 0;
                EventManager.Instance.TakeNewActions();
            }
        }

        public Ease moveType;

        void UseCard(ActionCard a, float delay = 0, bool isEnemy = false)
        {
            if (a is AttackAction)
            {
                var pos = a.transform.position;
                var step = 3;
                var offset = new Vector3(0, isEnemy ? -step * 1.5f : step, 0);
                a.transform.DOMove(pos + offset, 0.4f).SetEase(moveType).SetDelay(delay).OnComplete(a.Use);
                return;
            }

            a.Use();
        }

        bool IsAttackDefense(ActionCard a1, ActionCard a2) =>
            a1 is AttackAction && a2 is DefenseAction ||
            a1 is DefenseAction && a2 is AttackAction;

        void Attack_Defense(ActionCard c1, ActionCard c2)
        {
            var attack = c1 is AttackAction ? c1 : c2;
            var defense = c1 is DefenseAction ? c1 : c2;

            defense.transform.DOScale(1.3f, 0.4f).OnComplete(defense.Disable);
            var pos = defense.transform.position;
            var step = 1;
            var offset = new Vector3(0, attack == c1 ? step : -step, 0);
            attack.transform.DOMove(pos + offset, 0.4f).OnComplete(attack.Disable);
        }
    }
}