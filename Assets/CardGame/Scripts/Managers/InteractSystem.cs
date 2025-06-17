using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Misc;
using UnityEngine;

namespace Managers
{
    public class InteractSystem : MonoBehaviour
    {
        #region Singleton

        //-------------------------------------------------------------
        public static InteractSystem Instance;

        void Awake()
        {
            if (Instance == null) Instance = this;
            else gameObject.SetActive(false);
        }

        void OnDisable() => Instance = null;
        //-------------------------------------------------------------

        #endregion

        [Header("Setup")]
        [SerializeField] MoveSystem moveSystem;
        [SerializeField] GeneratorData data;

        [HideInInspector]
        public bool enablePlayerActions = true;
        bool _pauseDuringPortal;
        bool _autoSpawn = true;
        bool _isBusy;
        public CardHero _player;
        GeneratorSystem _generator;
        public void SetAutoSpawn(bool isAuto) => _autoSpawn = isAuto;


        void Start()
        {
            _generator = GeneratorSystem.Instance;
            EventManager.Instance.OnPortalOpen += PortalOpened;
            EventManager.Instance.OnPortalClose += PortalClosed;
            EventManager.Instance.OnCardTouch += !useTouchCombo ? TryInteract : TryInteractCombo;
            EventManager.Instance.OnPlayerCreated += SetPlayer;
            EventManager.Instance.OnDisablePlayer += DisablePlayer;
            EventManager.Instance.OnEnablePlayer += EnablePlayer;
        }

        void EnablePlayer()
        {
            isBusy = false;
        }

        void DisablePlayer()
        {
            isBusy = true;
        }


        void SetPlayer(CardHero card)
        {
            _player = card;
        }

        public bool useTouchCombo;
        public List<Card> touched;
        public int touchComboTimes = 3;

        void TryInteract(Card card)
        {
            if (card is CardHero) return;
            if (!_player.Movement.InRange(card)) return;
            Interact(_player.Cell, card.Cell);
        }

        void TryInteractCombo(Card card)
        {
            if (card is CardHero) return;
            //   if (card is CardCreature)
            //   {
            if (touched.Contains(card))
            {
                touched.Remove(card);
                card.DisableGlow();

                foreach (var cell in moveSystem.Cells)
                {
                    if (cell.Empty) continue;
                    if (touched.Contains(cell.Card)) continue;
                    //  cell.Card.UpdateLabels(touched.Count);
                }

                _player.HidePossibleDamage();

                return;
            }

            touched.Add(card);
            card.Glow();

            var possible = 0f;

            if (touched.Count > 0)
            {
                if (touched[0] is CardCreature c)
                    possible += c.Health;
                if (touched[0] is CardArtefact a)
                    possible -= a._HP;
            }

            if (touched.Count > 1)
            {
                if (touched[1] is CardCreature c1)
                    possible += c1.Health * 2;
                if (touched[1] is CardArtefact a1)
                    possible -= a1._HP;
            }

            if (possible > 0) _player.SetPossibleDamage((int) possible);
            // int pos = touched[0] is CardCreature ? 1 : 0;

            foreach (var cell in moveSystem.Cells)
            {
                if (cell.Empty) continue;
                if (touched.Contains(cell.Card)) continue;
                //  cell.Card.UpdateLabels(touched.Count);
            }


            if (touched.Count >= touchComboTimes)
            {
                StartCoroutine(Combo());
            }


            return;
            // }

            if (touched.Count > 0) return;
            Interact(_player.Cell, card.Cell);
        }

        IEnumerator Combo()
        {
            _player.HidePossibleDamage();
            var lastCell = touched[^1].Cell;
            int iteration = 0;
            foreach (var card in touched)
            {
                _player.TouchAttackAnimation(card.Position);
                var target = card;
                var a = Random.Range(15, 25);
                var b = Random.value > 0.5f ? 1 : -1f;
                var rot = new Vector3(0, 0, a * b);
                var rotTime = 0.20f;
                //  if (targetCard is CardCreature c) c.ShowDamageText(player.AttackPower, 0.35f);

                target.transform.DOShakePosition(0.3f, new Vector2(10, 10));
                target.transform.DOLocalRotate(rot, rotTime / 2).SetEase(Ease.InQuad).SetDelay(0.35f)
                    .OnComplete(() =>
                        target.transform.DOLocalRotate(Vector3.zero, rotTime / 2));

                yield return new WaitForSeconds(0.35f);

                target.Interact(_player, iteration);
                _player.Interact(target, iteration);

                iteration++;
            }

            PlayerCaptureNewCell(lastCell, _player);

            moveSystem.CleanBottomLine();
            yield return null;

            moveSystem.MoveAll();
            if (_generator.IsGeneratePossible && _autoSpawn)
                _generator.SpawnRandomCards();

            yield return null;
            if (moveSystem.MidLine.Contains(_player.Cell))
            {
                moveSystem.CleanBottomLine();
                yield return null;

                moveSystem.MoveAll();
                if (_generator.IsGeneratePossible && _autoSpawn)
                    _generator.SpawnRandomCards();

                _player.ArtNormal();
                _player.Hide(1);
            }

            yield return null;
            if (moveSystem.BotLine.Contains(_player.Cell))
            {
                moveSystem.CleanBottomLine();
                yield return null;

                moveSystem.MoveAll();
                if (_generator.IsGeneratePossible && _autoSpawn)
                    _generator.SpawnRandomCards();

                _player.ArtNormal();
                _player.Hide(1);
            }

            touched.Clear();
            foreach (var cell in moveSystem.Cells)
            {
                if (cell.Empty) continue;
                if (touched.Contains(cell.Card)) continue;
                cell.Card.UpdateLabels(touched.Count);
            }
        }

        void PortalClosed() => _pauseDuringPortal = false;
        void PortalOpened(CardDataPortal obj) => _pauseDuringPortal = true;


        public void Interact(Cell playerCell, Cell targetCell, bool bySwipe = false)
        {
            if (_isBusy) return;
            if (isBusy) return;
            StartCoroutine(InteractCoroutine(playerCell, targetCell, bySwipe));
            touched.Clear();
        }

        void DisableNeighboors(Cell active)
        {
            foreach (var cell in moveSystem.BotLine)
            {
                if (cell == active) continue;
                if (cell.Card)
                    cell.Card.ArtGray();
            }
        }


        IEnumerator InteractCoroutine(Cell playerCell, Cell targetCell, bool bySwipe)
        {
            _isBusy = true;

            var player = playerCell.Card;
            var target = targetCell.Card;

            target.Glow();
            DisableNeighboors(targetCell);
            DisableActions();
            EventManager.Instance.PlayerInteractWith(target);


            #region Flip card?

            if (target.CurrentState == Card.State.Close)
            {
                player.MoveBack();
                yield return null;
                target.FlipBack();
                yield return null;
                while (target.CurrentState == Card.State.Busy)
                    yield return null;
            }

            #endregion

            #region Attack animation

            var isInteracted = false;

            if (target is CardCreature or CardBoss)
            {
                if (bySwipe) player.SwipeAttackAnim(target.Position);
                else player.TouchAttackAnimation(target.Position);
                //player.PlayAttackVFX();
                var a = Random.Range(15, 25);
                var b = Random.value > 0.5f ? 1 : -1f;
                var rot = new Vector3(0, 0, a * b);
                var rotTime = 0.20f;
                //  if (targetCard is CardCreature c) c.ShowDamageText(player.AttackPower, 0.35f);

                target.transform.DOShakePosition(0.3f, new Vector2(10, 10));
                target.transform.DOLocalRotate(rot, rotTime / 2).SetEase(Ease.InQuad).SetDelay(0.35f)
                    .OnComplete(() =>
                        target.transform.DOLocalRotate(Vector3.zero, rotTime / 2));

                yield return new WaitForSeconds(0.35f);
                isInteracted = true;
                target.Interact(player);
                player.Interact(target);

                // if (target.IsDead)
                //     EventManager.Instance.CreateArtefactAtCell(data.DefaultDrop, targetCell);
                //
                // player.transform.DOLocalMove(Vector3.zero, 0.2f);
                // yield return new WaitForSeconds(0.2f);
                // _isBusy = false;
                // EnableActions();
                // yield break;


                // if (target.IsAlive)
                // {
                //     _isBusy = false;
                //     EnableActions();
                //     yield break;
                // }
            }
            
/*
            if (target is CardBoss)
            {
                if (bySwipe) player.SwipeAttackAnim(target.Position);
                else player.TouchAttackAnimation(target.Position);
                //player.PlayAttackVFX();
                var a = Random.Range(15, 25);
                var b = Random.value > 0.5f ? 1 : -1f;
                var rot = new Vector3(0, 0, a * b);
                var rotTime = 0.20f;
                //  if (targetCard is CardCreature c) c.ShowDamageText(player.AttackPower, 0.35f);

                target.transform.DOShakePosition(0.3f, new Vector2(10, 10));
                target.transform.DOLocalRotate(rot, rotTime / 2).SetEase(Ease.InQuad).SetDelay(0.35f)
                    .OnComplete(() =>
                        target.transform.DOLocalRotate(Vector3.zero, rotTime / 2));

                yield return new WaitForSeconds(0.35f);
                isInteracted = true;
                target.Interact(player);
                player.Interact(target);

                // if (target.IsDead)
                //     EventManager.Instance.CreateArtefactAtCell(data.DefaultDrop, targetCell);
                //
                 player.transform.DOLocalMove(Vector3.zero, 0.2f);
                  yield return new WaitForSeconds(0.2f);
                // _isBusy = false;
                // EnableActions();
                // yield break;


                if (target.IsAlive)
                {
                    _isBusy = false;
                    EnableActions();
                    yield break;
                }
                else
                {
                    yield return new WaitForSeconds(1f);
                }
            }
          */

            if (target is CardArtefact or CardPortal)
            {
                if (bySwipe) player.SwipeAttackAnim(target.Position, false);
                else player.TouchAttackAnimation(target.Position, false);
                //  player.PlayCollectAnimation(target.Position);
                //  CollectAnimation(player,target, bySwipe);

                yield return new WaitForSeconds(0.35f);
                isInteracted = true;
                target.Interact(player);
                player.Interact(target);
            }

            EventManager.Instance.Interact(target);
            yield return null;
            //  while (playerCard.CurrentState == Card.State.Busy) yield return null;

            #endregion


            if (!isInteracted)
            {
                target.Interact(player);
                player.Interact(target);
            }


            if (player.IsAlive)
            {
                #region Player capture new cell

                PlayerCaptureNewCell(targetCell, player);
                yield return null;
                // while (playerCard.CurrentState == Card.State.Busy) yield return null;
                //   yield return new WaitForSeconds(0.1f);

                #endregion

                #region IsPortalOpened?

                if (_pauseDuringPortal)
                {
                    while (_pauseDuringPortal) yield return null;
                    yield return new WaitForSeconds(0.6f);
                }

                #endregion

                #region New cell attacked by top enemy?

                var attackingCard = targetCell.AttackingCell.Card;
                //  var isNotBoss = attackingCard is not CardBoss;
                if (attackingCard is CardCreature creature && creature.Data.IsAggressive)
                {
                    attackingCard.ArtNormal();
                    attackingCard.Hide(1);
                    attackingCard.Glow();

                    #region Flip card?

                    if (attackingCard.CurrentState == Card.State.Close)
                    {
                        attackingCard.FlipBack();
                        while (attackingCard.CurrentState == Card.State.Busy)
                            yield return null;
                    }

                    #endregion

                    yield return new WaitForSeconds(0.2f);

                    var cardSortingOrder = attackingCard.SortingOrder;
                    attackingCard.SortingOrder = 99;

                    #region Attack animation

                    var moveDur = 0.5f;

                    if (attackingCard is CardCreature c)
                    {
                        c.ShowDamageText((int) c.Health, moveDur);
                        _player.ShowDamageText((int) c.Health, c.Data.Dangerous, moveDur);
                    }

                    attackingCard.AgressiveAttackAnimation(player.Position);

                    yield return new WaitForSeconds(moveDur);
                    attackingCard.DisableGlow();

                    attackingCard.Interact(player);
                    player.Interact(attackingCard);

                    EventManager.Instance.Interact(attackingCard);
                    // while (attackingCard.CurrentState == Card.State.Busy && attackingCard.IsAlive)
                    yield return null;

                    #endregion


                    if (attackingCard.IsAlive)
                    {
                        attackingCard.DisableGlow();
                        attackingCard.MoveBack();
                        // while (attackingCard.CurrentState == Card.State.Busy)
                        //     yield return null;

                        attackingCard.SortingOrder = cardSortingOrder;
                    }
                    else
                    {
                        if (attackingCard is IDrop drop)
                        {
                            drop.DropArtefact(moveSystem.IsNullDropPossible(attackingCard.Cell),
                                data.DefaultDrop);
                        }

                        //   yield return new WaitForSeconds(0.2f);
                    }
                }

                #endregion

                #region CreateNewCards

                moveSystem.CleanBottomLine();
                yield return null;

                moveSystem.MoveAll();
                if (_generator.IsGeneratePossible && _autoSpawn)
                    _generator.SpawnRandomCards();

                #endregion
            }
            else
            {
                if (target.IsDead)
                    EventManager.Instance.CreateArtefactAtCell(data.DefaultDrop, targetCell);
            }

            if (enablePlayerActions)
                EnableActions();

            while (player.CurrentState == Card.State.Busy) yield return null;
            _isBusy = false;
            //   AdsManager.Instance.ShowInterstitial();
        }

        public void CollectAnimation(Card card, Card target, bool bySwipe)
        {
            if (bySwipe) card.SwipeAttackAnim(target.Position);
            else card.TouchAttackAnimation(target.Position);
            return;
            //Debug.LogError("AAAAAAAAAAAAAAA");
            card.SetBusy();

            var offset = new Vector3(0, -2.5f, 0);
            //var offsetSmall = this is CardHero ? new Vector3(0, -0.5f, 0) : new Vector3(0, 0.4f, 0);
            card.Container.DOShakePosition(0.3f, new Vector2(10, 10));

            var position = card.Position + offset;
            //Debug.LogError(card.Position);
            //Debug.LogError(position);

            var sequence = DOTween.Sequence();
            var moveDur = 0.3f;
            var t = card.transform;
            //transform.DOScale(1f, moveDur).SetDelay(moveDur);

            //sequence.Append(t.DOScale(1.3f, 0.2f));
            sequence.Append(t.DOMove(position, moveDur).SetEase(Ease.InQuad)).SetDelay(0.0f);
            //.OnComplete(PlayAttackVFX));

            // sequence.Append(transform.DOMove(position, moveDur));
            sequence.Append(t.DOShakePosition(0.3f, new Vector2(10, 10)));
            sequence.OnComplete(card.SetFree);
        }

        void PlayerCaptureNewCell(Cell targetCell, Card playerCard)
        {
            playerCard.transform.SetParent(targetCell.transform);
            playerCard.Cell.Card = null;
            playerCard.Cell = targetCell;
            targetCell.Card = playerCard;
            //  playerCard.MoveBack();
        }


        void DisableActions()
        {
            EventManager.Instance.DisablePlayer();
        }

        bool isBusy;

        void EnableActions()
        {
            EventManager.Instance.EnablePlayer();
        }
    }
}