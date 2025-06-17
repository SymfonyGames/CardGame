using DataSave;
using HeroSelect;
using Level;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Misc
{
    public class MainMenu : MonoBehaviour
    {
        public Button playButton;
        public HeroSelectSystem heroSelect;
        public LevelData tutorialLevel;


        void Start()
        {
            if (playButton)
                playButton.onClick.AddListener(PlayButton);
        }

        public void PlayButton()
        {
            if (PlayerPrefs.GetInt(SaveKeys.TUTORIAL_COMPLETE) != 1)
                SceneManager.LoadScene(tutorialLevel.name);
            else
                heroSelect.Enable();
        }
    }
}