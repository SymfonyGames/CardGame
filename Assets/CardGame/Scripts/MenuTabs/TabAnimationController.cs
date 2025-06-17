using NaughtyAttributes;
using Plugins.AudioManager.audio_Manager;
using UnityEngine;

namespace MenuTabs
{
    public class TabAnimationController : MonoBehaviour
    {
        [SerializeField] TabsPanelController controller;
        [Header("Sound")]
        [SerializeField] SoundData clickSound;
        [Header("Settings")]
        [SerializeField] TabAnimType animType;
        [ShowIf(nameof(IsMainMenu))]
        [SerializeField] MainMenuTabAnimData anim;

        bool IsMainMenu()
        {
            if (animType == TabAnimType.MainMenu)
                return true;
            return false;
        }

        enum TabAnimType
        {
            MainMenu,
            CharacterTabs
        }

        void Awake()
        {
            controller.ClickSound = clickSound;
            foreach (var tab in controller.ChildTabs)
            {
                if (tab.UI.Anim is MainMenuTabAnim menu)
                    menu.Init(anim);
            }
        }
    }
}