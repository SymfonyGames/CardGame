using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BossGame.Actions;
using DG.Tweening;
using Managers;
using UnityEngine;

namespace BossGame
{
    public class Cards : MonoBehaviour
    {
        [SerializeField] bool mergeCards = true;
        [SerializeField] int mergeCount = 2;

        [SerializeField] Ease mergeMove;
        [SerializeField] float mergeTime = 0.3f;
        Deck _deck;
        [SerializeField] List<ActionCard> inHand = new();
        [SerializeField] Field inHandField;
        GamePlayer _owner;
        GamePlayer _enemy;
        public bool Contains(ActionCard card) => inHand.Contains(card);
        public List<ActionCard> InHand => inHand;

        public void Init(GamePlayer owner, GamePlayer enemy, Deck deck)
        {
            _enemy = enemy;
            _owner = owner;
            _deck = deck;
        }

        public void Add(ActionCard card, ActionCell toCell = null)
        {
            inHand.Add(card);
            if (inHandField)
            {
                var cell = toCell ? inHandField.Put(card, toCell) : inHandField.Put(card);
                card.SetCell(cell);
            }
        }

        public void Remove(ActionCard card)
        {
            inHand.Remove(card);
            if (inHandField)
                inHandField.Remove(card);
        }

        public List<ActionCard> Get(int count)
        {
            for (var i = inHand.Count; i < count; i++)
            {
          
                var data = _deck.Actions.Random;
                var card = Instantiate(data.Prefab, inHandField.Container);
                card.SetData(data);
                card.SetArt(data.Art);
                card.SetPlayers(_owner, _enemy);

                Add(card);
            }

            if (mergeCards) Invoke(nameof(Merge), 0.4f);
            return inHand;
        }

        [SerializeField] List<ActionCard> merge;

        void Merge()
        {
            merge = new List<ActionCard>();
            foreach (var card in inHand)
            {
                if (merge.Any(c => c.Data == card.Data))
                {
                    continue;
                }

                var same = inHand.Count(c => c.Data == card.Data);
                if (same < mergeCount) continue;

                var sameList = inHand.Where(c => c.Data == card.Data).ToList();
                if (sameList.Count > mergeCount)
                    for (int i = mergeCount; i < sameList.Count; i++)
                        sameList.Remove(sameList[i]);

                if (card.Data is not AttackActionData && card.Data is not DefenseActionData) continue;
                if (card.Data is AttackActionData && _owner.SpecialAttacks.None) continue;
                if (card.Data is DefenseActionData && _owner.SpecialDefense.None) continue;

                merge.AddRange(sameList);

                StartCoroutine(DoMerge(sameList));
            }
        }

        IEnumerator DoMerge(List<ActionCard> cards)
        {
            var card = cards[0];
            var data = card.Data;
            var cell = card.Cell;
            var pos = card.Position;
            foreach (var c in cards)
            {
                c.transform.DOMove(pos, mergeTime).SetEase(mergeMove).OnComplete(() => Disable(c));
            }

            yield return new WaitForSeconds(mergeTime);

            var merged = data switch
            {
                AttackActionData => _owner.SpecialAttacks.Random,
                DefenseActionData => _owner.SpecialDefense.Random,
                _ => null
            };

            if (!merged)
            {
                Debug.LogError("NO CARD");
                yield break;
            }

            var newCard = Instantiate(merged.Prefab, cell.transform);
            newCard.SetData(merged);
            newCard.SetArt(merged.Art);
            newCard.SetPlayers(_owner, _enemy);

            Add(newCard, cell);
            
            EventManager.Instance.TakeNewActions();
        }

        void Disable(ActionCard c)
        {
            Remove(c);
            c.Disable();
        }
    }
}