using Managers;
using Player;
using UnityEngine;

namespace BossGame
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] PlayerData data;
        [SerializeField] GamePlayer player_1;
        [SerializeField] GamePlayer player_2;
        [SerializeField] Engine engine;
        [SerializeField] EnemyAI enemyAI;
        [SerializeField] Interactor interactor;

        CardHero _hero;

        void Start()
        {
            EventManager.Instance.OnPlayerCreated += SetPlayer;
        }

        public void StartGame()
        {
            interactor.Init(player_1, player_2);
            enemyAI.Init();
            engine.Init(player_1, player_2, interactor);
            engine.StartGame();
        }

        void OnDisable()
        {
            EventManager.Instance.OnPlayerCreated -= SetPlayer;
        }

        public void SetPlayer(CardHero hero) => _hero = hero;

        public void InitPlayer()
        {
            var h = data.selectedHero;

            Debug.LogError(" CHANGE HERO HP x10 MULT");
            var stats = new PlayerStats
            {
                hp = _hero.Health * 10,
                armor = h.CustomGameStats.armor,
                dmgMin = h.CustomGameStats.dmgMin,
                dmgMax = h.CustomGameStats.dmgMax
            };

            player_1.Init(stats, h.Actions, h.Attacks, h.Defense, player_2, interactor, h.Art);
        }

        public void InitEnemy(CardBoss boss)
        {
            var h = boss.Data;
            var stats = new PlayerStats
            {
                hp = h.Stats.hp,
                armor = h.Stats.armor,
                dmgMin = h.Stats.dmgMin,
                dmgMax = h.Stats.dmgMax
            };

            player_2.Init(stats, h.Actions, h.Attacks, h.Defense, player_1, interactor, h.Art);
        }
    }
}