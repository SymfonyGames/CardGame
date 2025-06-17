using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Level;
using Misc;
using Plugins.AudioManager.audio_Manager;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Managers
{
    public class MoveSystem : MonoBehaviour
    {
        #region Singleton

        //-------------------------------------------------------------
        public static MoveSystem Instance;

        void Awake()
        {
            if (Instance == null) Instance = this;
            else gameObject.SetActive(false);
        }

        void OnDisable() => Instance = null;
        //-------------------------------------------------------------

        #endregion

        [Header("Settings")]
        [SerializeField] bool randomizeTime = true;
        [SerializeField] float moveTime = 0.3f;
        [SerializeField] float randomMult = 0.4f;
        [SerializeField] float cardAlpha = 0.85f;
        [SerializeField] Ease moveType;
        [Header("Setup")] LevelTheme levelTheme;
        [SerializeField] Cell[] cells;
        [FormerlySerializedAs("playerAttackCells")] [SerializeField] Cell[] botLine;
        [FormerlySerializedAs("aggresiveCels")] [SerializeField] Cell[] midLine;
        [FormerlySerializedAs("rootCells")] [SerializeField] Cell[] topLine;
        CardHero player;
        public List<Cell> GetRootCells() => topLine.ToList();

        void Start()
        {
            levelTheme = LevelTheme.Instance;
            EventManager.Instance.OnPlayerCreated += SetPlayer;
        }


        void SetPlayer(CardHero card)
        {
            player = card;
        }


        public bool IsNullDropPossible(Cell cell)
        {
            var line = GetLine(cell);
            return IsNeighbors(cell, line);
        }

        public void MoveAll()
        {
            PlaySound();
            SendEvent();

            foreach (var cell in cells)
            {
                if (!cell.Card || cell.NextCell.Card) continue;
                Move(cell, cell.NextCell);
            }
        }

        public void MoveAllAtStart()
        {
            foreach (var cell in cells)
            {
                if (!cell.Card || cell.NextCell.Card) continue;
                MoveAtStart(cell, cell.NextCell);
            }
        }

        public void MoveRootToAggressive()
        {
            for (var i = 0; i < topLine.Length; i++)
            {
                var root = topLine[i];
                var aggressive = midLine[i];
                if (!root.Card || !aggressive) continue;
                MoveAtStart(root, aggressive);
            }
        }

        public void MoveRootToAttacked()
        {
            for (var i = 0; i < topLine.Length; i++)
            {
                var root = topLine[i];
                var attacked = botLine[i];
                if (!root.Card || !attacked) continue;
                MoveAtStart(root, attacked);
            }
        }


        void Move(Cell from, Cell to)
        {
            var card = from.Card;
            from.Card = null;
            to.Card = card;
            card.Cell = to;
            var animTime = RandomTime;

            if (botLine.Contains(to))
            {
                card.ArtNormal();
                card.Hide(1);

                var pc = player.Cell;
                if (cells.Contains(pc))
                {
                    var pi = cells.ToList().IndexOf(pc);
                    var ci = botLine.ToList().IndexOf(to);

                    if (Mathf.Abs(pi - ci) > player.walkDistance)
                    {
                        card.ArtGray();
                        card.Hide(0.75f);
                    }
                }
            }

            if (midLine.Contains(to))
            {
             //   card.ArtGray();
                card.Hide(cardAlpha);
            }

            card.transform.SetParent(to.transform);
            card.transform.DOKill();
            card.transform.DOLocalMove(Vector3.zero, animTime).SetEase(moveType);
            card.transform.DOLocalRotate(Vector3.zero, animTime).SetDelay(0.1f);
            card.transform.DOScale(to.transform.localScale.x, animTime);
        }


        void MoveAtStart(Cell from, Cell to)
        {
            var card = from.Card;
            from.Card = null;
            to.Card = card;
            card.Cell = to;
            card.transform.SetParent(to.transform);
            card.transform.DOLocalRotate(Vector3.zero, RandomTime).SetDelay(0.1f);
            card.transform.DOScale(to.transform.localScale.x, 0.5f);

            if (botLine.Contains(to))
                card.ArtNormal();
            //   if (midLine.Contains(to))
            //      card.ArtNormal();
        }

        public void CleanBottomLine()
        {
            foreach (var cell in botLine)
            {
                if (!cell.Card) continue;
                if (cell.Card is CardHero) continue;
                StartCoroutine(Hide(cell.Card));
                //  Destroy(cell.Card.gameObject);
                // cell.Card.ArtGray();
                // cell.Card.Hide();
                // cell.Card.transform.DOMove(cell.Card.Position + Vector3.down, 0.5f);
                //     Destroy(cell.Card.gameObject);
                cell.Card = null;
            }
        }

        IEnumerator Hide(Card card)
        {
            card.ArtGray();
            //card.Hide(0.5f);
            card.transform.DOMove(card.Position + Vector3.down * 8f, RandomTime * 1.8f);
            yield return new WaitForSeconds(3f);
            //     card.Hide();
            //    yield return new WaitForSeconds(0.2f);
            Destroy(card.gameObject);
        }

        public void PlaySound()
        {
            if (levelTheme && levelTheme.Data && levelTheme.Data.Theme)
                AudioManager.Instance.PlaySound(levelTheme.Data.Theme.MoveSound);
        }

        void SendEvent()
        {
            if (EventManager.Instance)
                EventManager.Instance.CardsMoveDown(moveTime);
        }


        float RandomTime => randomizeTime
            ? Random.Range(moveTime * (1 - randomMult), moveTime * (1 + randomMult))
            : moveTime;

        public Cell[] BotLine => botLine;

        public Cell[] Cells => cells;

        public Cell[] TopLine => topLine;

        public Cell[] MidLine => midLine;

        Cell[] GetLine(Cell cell)
        {
            if (botLine.Any(c => c == cell))
                return botLine;

            if (midLine.Any(c => c == cell))
                return midLine;

            return topLine.Any(c => c == cell)
                ? topLine
                : null;
        }

        bool IsNeighbors(Cell cell, Cell[] line)
        {
            for (var i = 0; i < line.Length; i++)
            {
                if (line[i] != cell) continue;

                if (i == 0 || i == 2)
                    return line[1].Card;

                return line[0].Card || line[2].Card;
            }

            return false;
        }
    }
}