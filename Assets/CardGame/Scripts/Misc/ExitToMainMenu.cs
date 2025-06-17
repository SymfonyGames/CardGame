using Plugins.AudioManager.audio_Manager;
using Plugins.LevelLoader;
using UnityEngine;

namespace Misc
{
    public class ExitToMainMenu : MonoBehaviour
    {
        public void Exit()
        {
            AudioManager.Instance.StopMusic();
            Loader.Instance.LoadMainMenu();
        }
    }
}