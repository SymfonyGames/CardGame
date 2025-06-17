using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Misc;
using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    public class Portal : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float createFrequency;
        [SerializeField] float delayBeforeClose = 1f;
        [SerializeField] float delayAfterFlip = 1f;
        [Header("Vfx")]
        [SerializeField] GameObject healthVFXprefab;
        [SerializeField] GameObject shieldVFXprefab;
        [Header("Setup")]
        [SerializeField] Canvas portalCanvas;
        [SerializeField] Text tapsAllowedText;
        [SerializeField] List<Cell> cells = new();
    
        List<Card> _flippedCards = new();
        int _maxAllowedCards;
        int _openedCards;
        Card _playerCard;

        void Awake()
        {
            portalCanvas.gameObject.SetActive(false);
            healthVFXprefab.SetActive(false);
            shieldVFXprefab.SetActive(false);
        }

        void Start()
        {
            EventManager.Instance.OnPortalOpen += OpenPortal;
            EventManager.Instance.OnPlayerCreated += PlayerCreated;
        }

        void PlayerCreated(Card card) => _playerCard = card;

        void OpenPortal(CardDataPortal portal)
        {
            EnableCanvas();
            EnableCells(portal);
            CreateCards(portal);
            StartPortal(portal);
            CameraFocusEnable();
        }

        void ClosePortal()
        {
            DestroyFlippedCards();
            DisableCanvas();
            SendCloseEvent();
            StopPortal();
            CameraFocusDisable();
        }
    
        void CameraFocusDisable() => EventManager.Instance.CameraFocusDisable();
        void CameraFocusEnable() => EventManager.Instance.EnableFocusCam();
    
        void StartPortal(CardDataPortal portal)
        {
            _flippedCards = new List<Card>();
            _maxAllowedCards = portal.AllowToOpenCards;
            _openedCards = 0;
            tapsAllowedText.text = _maxAllowedCards.ToString();
            FlipEventSubscribe(true);
            EnableActions();
        }

        void StopPortal()
        {
            FlipEventSubscribe(false);
        }

        void FlipEventSubscribe(bool subscribe)
        {
            if (subscribe)
            {
                EventManager.Instance.OnFlipBack += FlipBack;
                EventManager.Instance.OnPlayerHeal += PlayHealVFX;
                EventManager.Instance.OnPlayerShield += PlayShieldVFX;
            }
            else
            {
                EventManager.Instance.OnFlipBack -= FlipBack;
                EventManager.Instance.OnPlayerHeal -= PlayHealVFX;
                EventManager.Instance.OnPlayerShield -= PlayShieldVFX;
            }
        }

        void PlayShieldVFX(int shieldValue)
        {
            if (shieldVFXprefab) shieldVFXprefab.SetActive(true);
        }

        void PlayHealVFX(float healValue)
        {
            if (healthVFXprefab) healthVFXprefab.SetActive(true);
        }


        void FlipBack(Card card)
        {
            StartCoroutine(InteractDelayed(card));
            _flippedCards.Add(card.GetComponent<Card>());
            _openedCards++;
            tapsAllowedText.text = (_maxAllowedCards - _openedCards).ToString();

            DisableActions();

            if (_openedCards >= _maxAllowedCards)
            {
                Invoke(nameof(DestroyBackFacedCards), 0.1f);
                Invoke(nameof(ClosePortal), delayBeforeClose);
            }
            else
            {
                Invoke(nameof(EnableActions), delayAfterFlip);
            }
        }

        IEnumerator InteractDelayed(Card card)
        {
            yield return new WaitForSeconds(0.5f);
            card.Interact(_playerCard);
        }

        void EnableCells(CardDataPortal portal)
        {
            for (var i = 0; i < cells.Count; i++)
            {
                var act = i < portal.TotalCards;
                cells[i].gameObject.SetActive(act);
            }
        }

        void EnableCanvas() => portalCanvas.gameObject.SetActive(true);
        void DisableCanvas() => portalCanvas.gameObject.SetActive(false);

        void CreateCards(CardDataPortal portal)
        {
            if (portal.Cards.Count == 0) return;
            StartCoroutine(CreateCardsRoutine(portal));
        }

        IEnumerator CreateCardsRoutine(CardDataPortal portal)
        {
            var cardList = new List<CardDataArtifact>();
        
            var artefactId = 0;
            for (var i = 0; i < portal.TotalCards; i++)
            {
                if (i >= cells.Count) break;

                artefactId++;
                if (artefactId == portal.Cards.Count) artefactId = 0;
                var artifact = portal.Cards[artefactId];
                cardList.Add(artifact);
            }

            cardList = cardList.OrderBy(i => Guid.NewGuid()).ToList();
            
            for (var i = 0; i < cardList.Count; i++)
            {
                var artifact = cardList[i];
                EventManager.Instance.CreatePortalCard(artifact, cells[i], portal.isFlip);
                yield return new WaitForSeconds(createFrequency);
            }
        }

        void DestroyFlippedCards()
        {
            foreach (var cell in cells.Where(c => c.Card && _flippedCards.Contains(c.Card)))
            {
                Destroy(cell.Card.gameObject);
            }
        }

        void DestroyBackFacedCards()
        {
            foreach (var cell in cells.Where(c => (c.Card) && !_flippedCards.Contains(c.Card)))
            {
                Destroy(cell.Card.gameObject);
                cell.Card = null;
            }
        }

        void SendCloseEvent() => EventManager.Instance.PortalClose();
        void DisableActions() => EventManager.Instance.PortalPauseActions();
        void EnableActions() => EventManager.Instance.PortalContinueActions();
    }
}