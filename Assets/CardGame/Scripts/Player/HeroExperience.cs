using Managers;
using Misc;
using Plugins.AudioManager.audio_Manager;
using TMPro;
using UnityEngine;

namespace Player
{
    public class HeroExperience : MonoBehaviour
    {
        public ConfigData config;
        int _level;
        public TextMeshProUGUI expText;
        public TextMeshProUGUI lvlText;
        public CustomSlider slider;
        public SoundData lvlUpSound;
        void Start()
        {
            _level = 1;
            slider.FillInstant(0);
            lvlText.text = "Lvl. " + _level;
            expText.text = $"{(int) _exp}/{Require}";
            EventManager.Instance.OnExperienceDrop += Add;
        }

        float _exp;

        void OnDisable()
        {
            EventManager.Instance.OnExperienceDrop -= Add;
        }

        float Require => config.heroExperienceTable[_level];

        void Add(Vector3 from, float value)
        {
            if (_level >= config.HeroLevelMax) return;

            _exp += value;
            if (_exp >= Require)
            {
                _exp -= Require;
                slider.FillInstant(0);
                LevelUp();
            }
            else
            {
                slider.SetValue(_exp/Require);
            }
        
            expText.text = $"{(int) _exp}/{Require}";
 
      
        }

        void LevelUp()
        {
            _level++;
            lvlText.text = "Lvl. " + _level;
            EventManager.Instance.HeroLevelUp(_level);
            AudioManager.Instance.PlaySound(lvlUpSound);
        }
    }
}