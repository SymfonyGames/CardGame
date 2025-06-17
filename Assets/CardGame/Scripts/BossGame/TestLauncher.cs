using Level;
using NaughtyAttributes;
using Player;
using UnityEngine;

namespace BossGame
{
    public class TestLauncher : MonoBehaviour
    {
        public GameController controller;
        public PlayerData data;
        public CardHero hero;
        public CardBoss boss;
        public LevelThemeData theme;
        public CardDataBoss bossData;

        void Start()
        {
            boss.Init(bossData, theme);
            controller.InitEnemy(boss);
            
            
            hero.Init(data.selectedHero, theme);
            controller.SetPlayer(hero);
            controller.InitPlayer();
            
            controller.StartGame();
        }
    }
}