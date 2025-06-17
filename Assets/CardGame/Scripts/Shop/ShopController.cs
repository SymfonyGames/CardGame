using System.Collections.Generic;
using Managers;
using Plugins.AudioManager.audio_Manager;
using UnityEngine;

namespace Shop
{
    public class ShopController : MonoBehaviour
    {
        [SerializeField] Canvas shopCanvas;
        [SerializeField] SoundData openSound;
        [SerializeField] SoundData closeSound;

        [SerializeField] List<GameObject> lineFocus = new List<GameObject>();
        [SerializeField] List<GameObject> panels = new List<GameObject>();

        void Start()
        {
            ShopClose();
            EventManager.Instance.OnShopOpen += ShopOpen;
            EventManager.Instance.OnShopClose += ShopClose;
            PanelSwitch(1);
        }


        public void PanelSwitch(int id)
        {
            for (int i = 0; i < panels.Count; i++)
            {
                bool act = i == id - 1;
                lineFocus[i].SetActive(act);
                panels[i].SetActive(act);
            }
        }

        public void ShopOpen()
        {
            if (shopCanvas) shopCanvas.gameObject.SetActive(true);
            PanelSwitch(1);
            PlaySound(openSound);
            CameraFocusEnable();
        }

        public void ShopClose()
        {
            if (shopCanvas) shopCanvas.gameObject.SetActive(false);
            PanelSwitch(0);
            PlaySound(closeSound);
            CameraFocusDisable();
        }

        void CameraFocusDisable() => EventManager.Instance.CameraFocusDisable();
        void CameraFocusEnable() => EventManager.Instance.EnableFocusCam();
        void PlaySound(SoundData sound) => AudioManager.Instance.PlaySound(sound);
    }
}