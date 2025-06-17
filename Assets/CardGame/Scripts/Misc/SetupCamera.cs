using UnityEngine;

namespace Misc
{
    public class SetupCamera : MonoBehaviour
    {
        [SerializeField] CameraType cameraType;
        Canvas canvas;
        void Awake() => canvas = GetComponent<Canvas>();

        void Start()
        {
            if (!canvas) return;
            
            var cam = CameraController.Instance.GetCamera(cameraType);
            canvas.worldCamera = cam;
        }


    }
}
