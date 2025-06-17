using UnityEngine;

namespace Misc
{
    public class CanvasEditorOffset : MonoBehaviour
    {
        void Awake() => transform.localPosition = Vector2.zero;
    
    }
}