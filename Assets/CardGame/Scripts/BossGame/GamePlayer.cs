using System;
using UnityEngine;
using Random = UnityEngine.Random;


namespace BossGame
{
    public class GamePlayer : MonoBehaviour
    {
        [SerializeField] int cardsInHand = 5;
        [SerializeField] float maxCharge = 3;
        [SerializeField] Cards cards;
        [SerializeField] Charge charge;
        [SerializeField] Deck deck;
        [SerializeField] Field hands;
        [SerializeField] Field table;
        [SerializeField] Hitpoints hitpoints;
        [SerializeField] CardsMover cardsMove;
        [SerializeField] PlayerUI ui;

        [Header("DEBUG")]
        [SerializeField] PlayerStats stats;
        [SerializeField] ActionStats specialAttacks;
        [SerializeField] ActionStats specialDefense;
        public event Action OnCardsTaken = delegate { };
        public Deck Deck => deck;
        public Cards Cards => cards;
        public int CardsInHand => cardsInHand;
        public Field Hands => hands;
        public CardsMover CardsMove => cardsMove;
        public Field Table => table;
        public ActionStats SpecialAttacks => specialAttacks;
        public ActionStats SpecialDefense => specialDefense;

        public PlayerUI UI => ui;
        public float Damage
        {
            get
            {
                var dmg = Random.Range(stats.dmgMin, stats.dmgMax);
                return dmg;
            }
        }



        public float TakeDamage(float dmg)
        {
            var result = dmg * (1 - stats.armor * 0.01f);
            hitpoints.Damage(result);
            RefreshHp();
            return result;
        }

        void RefreshHp()
        {
            ui.Hitpoints.Refresh(hitpoints.Value, (int) hitpoints.HP);
        }

        public void Init(
            PlayerStats playerStats, 
            ActionStats actions, ActionStats specialAttack, ActionStats specialDefenses,
            GamePlayer enemy, Interactor interactor, Sprite art)
        {
            specialDefense = specialDefenses;
            specialAttacks = specialAttack;
            stats = playerStats;

            hitpoints.Init(playerStats.hp);


            deck.Init(actions);
            cards.Init(this, enemy, deck);
            cardsMove.Init(cards, table, interactor);

            hands.Init(ui.Hands);
            table.Init(ui.Table);

            ui.RefreshStats((int) playerStats.dmgMin, (int) playerStats.dmgMax, (int) playerStats.armor);
            ui.SetArt(art);
            RefreshHp();
            
            charge.Init(this, maxCharge);
        }

        public void TakeCards()
        {
            Cards.Get(cardsInHand);
            OnCardsTaken();
        }
    }
}