using DataSave;
using HeroSelect;
using Maps;
using Player;
using UnityEngine;

namespace Misc
{
    public class SkipTutorial : MonoBehaviour
    {
        public bool skipTutorial;
        public Map map;
        public HeroSelectSystem select;
        public PlayerData data;
        public MainMenu mainMenu;

        public void Go()
        {
            if (skipTutorial && PlayerPrefs.GetInt(SaveKeys.TUTORIAL_COMPLETE) == 0)
            {
                PlayerPrefs.SetInt(SaveKeys.TUTORIAL_COMPLETE, 1);
                mainMenu.PlayButton();
                select.Enable();
                select.HeroSelected(data.selectedHero);
                map.OnPlayButton();
            }
        }
    }
}