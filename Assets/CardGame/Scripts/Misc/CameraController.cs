using Managers;
using UnityEngine;

namespace Misc
{
    public class CameraController : MonoBehaviour
    {

        #region Singleton

        //-------------------------------------------------------------
        public static CameraController Instance;

        void Awake()
        {
            if (Instance == null) Instance = this;
            else gameObject.SetActive(false);
        }


        //-------------------------------------------------------------

        #endregion

        [SerializeField] Camera main;
        [SerializeField] Camera focus;
        [SerializeField] Camera particles;
        [SerializeField] Camera background;
        [SerializeField] Camera backgroundVFX;


        void Start()
        {
            EventManager.Instance.OnEnableFocus += EnableFocusCamera;
            EventManager.Instance.OnDisableFocus += DisableFocusCamera;
        }

        void OnDisable()
        {
            EventManager.Instance.OnEnableFocus -= EnableFocusCamera;
            EventManager.Instance.OnDisableFocus -= DisableFocusCamera;
        }

        void EnableFocusCamera() 
            => focus.gameObject.SetActive(true);
        void DisableFocusCamera() 
            => focus.gameObject.SetActive(false);

        public Camera GetCamera(CameraType type)
        {
            return type switch
            {
                CameraType.Main => main,
                CameraType.Focus => focus,
                CameraType.Particles => particles,
                CameraType.Background => background,
                CameraType.BackgroundVFX => backgroundVFX,
                _ => null,
            };
        }
    }
}