using System;
using System.Collections.Generic;
using System.Linq;
using BossGame.Actions;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BossGame
{
    public class Field : MonoBehaviour
    {
        public IReadOnlyList<ActionCell> Cells;
        [SerializeField] float moveTime = 0.3f;
        [SerializeField] float randomMult = 0.4f;
        [SerializeField] Ease moveType;
        public Transform Container { get; private set; }
        public ActionCell GetCell => Cells.FirstOrDefault(c => c.HasCard);
        public bool NoCards => Cells.All(c => c.Empty);

        public void Init(CellsUI ui)
        {
            Container = ui.transform;
            Cells = ui.Cells.Where(c => c.gameObject.activeSelf).ToList();
        }

        public bool IsFull => Cells.All(c => c.HasCard);
        public bool NotFull => Cells.Any(c => c.Empty);
        public int Size => Cells.Count;
        public event Action<ActionCard> OnPut = delegate { };


        float RandomTime => Random.Range
            (moveTime * (1 - randomMult), moveTime * (1 + randomMult));

        public void Remove(ActionCard card)
        {
            var find = Cells.FirstOrDefault(c => c.Card == card);
            if (!find) return;
            card.OnUse -= Remove;
            find.Remove(card);
        }

        public ActionCell Put(ActionCard card)
        {
            foreach (var cell in Cells)
            {
                if (cell.HasCard) continue;
                cell.Put(card);
                card.OnUse += Remove;

                OnPut(card);

                var animTime = RandomTime;
                card.transform.DOLocalMove(Vector3.zero, animTime).SetEase(moveType).SetDelay(0f);
                card.transform.DOLocalRotate(Vector3.zero, animTime).SetDelay(0.1f);

                card.transform.DOScale(1, 0);
                return cell;
            }

            return null;
        }

        public void Put(ActionCard card, int slotID)
        {
            var cell = Cells[slotID];

            cell.Put(card);
            card.OnUse += Remove;

            OnPut(card);

            var animTime = RandomTime;
            card.transform.DOLocalMove(Vector3.zero, animTime).SetEase(moveType);
            card.transform.DOLocalRotate(Vector3.zero, animTime).SetDelay(0.1f);
            // card.transform.DOScale(cell.transform.localScale.x, animTime);
            card.transform.DOScale(1, 0);
        }

        public ActionCell Put(ActionCard card, ActionCell cell)
        {
            cell.Put(card);
            card.OnUse += Remove;

            OnPut(card);

            var animTime = RandomTime;
            card.transform.DOLocalMove(Vector3.zero, animTime).SetEase(moveType);
            card.transform.DOLocalRotate(Vector3.zero, animTime).SetDelay(0.1f);
            // card.transform.DOScale(cell.transform.localScale.x, animTime);
            card.transform.DOScale(1, 0);
            return cell;
        }
    }
}