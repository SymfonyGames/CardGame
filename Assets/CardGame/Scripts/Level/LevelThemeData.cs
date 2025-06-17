using Plugins.AudioManager.audio_Manager;
using UnityEngine;

namespace Level
{
    [CreateAssetMenu(fileName = "New Level Theme", menuName = "Level/Theme")]
    public class LevelThemeData : ScriptableObject 
    {
        [Header("Background")] 
        [SerializeField] Sprite background;
        [SerializeField] GameObject envoirementVFXPrefab;
        [SerializeField] Color filterColor;
    
        [Header("Card front")]
        [SerializeField] Sprite cardFrontFrame;
        [SerializeField] Sprite cardFrontGlow;
    
        [Header("Card back")]
        [SerializeField] Sprite cardBackBody;
        [SerializeField] Sprite cardBackGlow;

        [Header("Sounds")] 
        [SerializeField] SoundData moveSound;
        [SerializeField] private SoundData backgroundMusic;


        public Sprite Background => background;
        public Sprite CardFrontFrame => cardFrontFrame;
        public Sprite CardFrontGlow => cardFrontGlow;
        public Sprite CardBackGlow => cardBackGlow;
        public Sprite CardBackBody => cardBackBody;
        public GameObject EnvoirementVfXprefab => envoirementVFXPrefab;
        public SoundData MoveSound => moveSound;

        public SoundData BackgroundMusic => backgroundMusic;

        public Color FilterColor => filterColor;
    }
}