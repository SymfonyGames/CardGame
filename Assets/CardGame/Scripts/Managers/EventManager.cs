using System;
using BossGame;
using BossGame.Actions;
using Misc;
using UnityEngine;

namespace Managers
{
    public class EventManager : MonoBehaviour
    {
        
        #region Singleton

        //-------------------------------------------------------------
        public static EventManager Instance;

        void Awake()
        {
            if (Instance == null) Instance = this;
            else gameObject.SetActive(false);
        }

        void OnDisable() => Instance = null;
        //-------------------------------------------------------------

        #endregion

        #region Camera

        // Enable focus
        public event Action OnEnableFocus = delegate { };

        public void EnableFocusCam() => OnEnableFocus();

        // Disable focus
        public event Action OnDisableFocus = delegate { };
        public void CameraFocusDisable() => OnDisableFocus();

        #endregion

        #region Player


     
        public event Action<CardHero> OnPlayerCreated = delegate { };
        public void PlayerCreated(CardHero card) => OnPlayerCreated(card);
        

        public event Action<PotionData> OnUsePotion = delegate { };
        public void UsePotion(PotionData potion) => OnUsePotion(potion);
        
        
        public event Action OnPlayerDeath = delegate { };

        public void PlayerDeath() => OnPlayerDeath();


        public event Action OnPlayerRevive = delegate { };

        public void PlayerRevive() => OnPlayerRevive();


        public event Action<Card> OnPlayerInteractWith = delegate { };

        public void PlayerInteractWith(Card card) => OnPlayerInteractWith(card);

        public event Action<float> OnPlayerHeal = delegate { };

        public void PlayerHeal(float amount) => OnPlayerHeal(amount);


        public event Action<int> OnPlayerShield = delegate { };

        public void PlayerShield(int amount) => OnPlayerShield(amount);


        public event Action OnPlayerAttackVFX = delegate { };
        public void PlayerAttackVFX() => OnPlayerAttackVFX();


        public event Action OnDisablePlayer = delegate { };
        public void DisablePlayer() => OnDisablePlayer();


        public event Action OnEnablePlayer = delegate { };
        public void EnablePlayer() => OnEnablePlayer();


        public event Action<float> OnPlayerHitpointsChanged = delegate { };
        public void PlayerHitpointsChanged(float currentHp) => OnPlayerHitpointsChanged(currentHp);

        #endregion

        #region Bank

        public event Action<int> OnGoldReceived = delegate { };

        public void GoldReceived(int amount) => OnGoldReceived(amount);
        
        
        public event Action<  int, Vector3> OnGoldVFX = delegate { };
        public void PlayGoldVFX(  int amount, Vector3 fromPos) => OnGoldVFX(  amount, fromPos);

        public event Action<int> OnGemReceived = delegate { };
        public void GemReceived(int amount) => OnGemReceived(amount);


        public event Action OnResourcesChanged = delegate { };
        public void ResourcesChanged() => OnResourcesChanged();


        public event Action<Vector3, int, int> OnCreateGoldVFX = delegate { };
        public void CreateGoldVFX(Vector3 from, int amount, int maxObjects = -1) =>
            OnCreateGoldVFX(from, amount, maxObjects);
        public event Action<Vector3, int, int> OnCreateSilverVFX = delegate { };
        public void CreateSilverVFX(Vector3 from, int amount, int maxObjects = -1) =>
            OnCreateSilverVFX(from, amount, maxObjects);
        public event Action<Vector3,float> OnExperienceDrop = delegate { };
        public void ExperienceDrop(Vector3 from, float exp ) =>
            OnExperienceDrop(from, exp);

        public event Action<int> OnHeroLevelUp = delegate { };
        public void HeroLevelUp(int lvl ) =>
            OnHeroLevelUp(lvl);
        
        
        public event Action<Vector3, int, int> OnGemCreate = delegate { };
        public void GemCreate(Vector3 from, int amount, int maxObjects = -1) => OnGemCreate(from, amount, maxObjects);

        #endregion

        #region Shop

        public event Action OnShopOpen = delegate { };

        public void ShopOpen() => OnShopOpen();


        public event Action OnShopClose = delegate { };
        public void ShopClose() => OnShopClose();

        #endregion

        #region Portal

        public event Action<CardDataPortal> OnPortalOpen = delegate { };
        public void PortalOpen(CardDataPortal portal) => OnPortalOpen(portal);


        public event Action OnPortalClose = delegate { };
        public void PortalClose() => OnPortalClose();


        public event Action<CardDataArtifact, Cell, bool> OnPortalCreateArtefactAtCell = delegate { };

        public void CreatePortalCard(CardDataArtifact artefact, Cell cell, bool flip) =>
            OnPortalCreateArtefactAtCell(artefact, cell, flip);


        public event Action OnPortalPauseActions = delegate { };

        public void PortalPauseActions() => OnPortalPauseActions();


        public event Action OnPortalContinueActions = delegate { };
        public void PortalContinueActions() => OnPortalContinueActions();

        #endregion

        #region Cards

        public event Action<Card> OnCardTouch = delegate { };
        public void CardTouch(Card card) => OnCardTouch(card);
        
        public event Action<GamePlayer,float> OnChargePlayer = delegate { };
        public void ChargePlayer(GamePlayer player, float value) => OnChargePlayer(player, value);
        
        public event Action<ActionCard> OnActionTouch = delegate { };
        public void ActionTouch(ActionCard card) => OnActionTouch(card);

        public event Action<Card> OnFlipBack = delegate { };
        public void FlipBack(Card card) => OnFlipBack(card);

        public event Action<Card> OnFlipBackFinish = delegate { };
        public void FlipBackFinish(Card card) => OnFlipBackFinish(card);

        public event Action<float> OnCardsMoveDown = delegate { };
        public void CardsMoveDown(float moveTime) => OnCardsMoveDown(moveTime);


        public event Action<CardDataArtifact, Cell> OnCreateArtefactAtCell = delegate { };

        public void CreateArtefactAtCell(CardDataArtifact artefact, Cell cell) =>
            OnCreateArtefactAtCell(artefact, cell);

        public event Action<ScriptableObject, Cell> OnCreateAtCell = delegate { };
        public void CreateAtCell(ScriptableObject artefact, Cell cell) => OnCreateAtCell(artefact, cell);

        #endregion

        #region Level WinLose

        // Level WIN
        public event Action OnLevelWin = delegate { };
        public void LevelWin() => OnLevelWin();

        // Level LOSE
        public event Action OnLevelLose = delegate { };
        public void LevelLose() => OnLevelLose();

        #endregion
        
        
        public event Action<Card> OnInteract = delegate { };
        
        public void Interact(Card card ) => OnInteract(card );
        
        public event Action OnTakeNewActions = delegate { };
        public void TakeNewActions( ) => OnTakeNewActions( );
        
        public event Action OnEnemyDropCard = delegate { };
        public void EnemyDropCard( ) => OnEnemyDropCard( );
    }
}