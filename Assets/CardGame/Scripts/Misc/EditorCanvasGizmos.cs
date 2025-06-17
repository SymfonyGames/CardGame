using UnityEngine;

namespace Misc
{
    [ExecuteInEditMode]
    public class EditorCanvasGizmos : MonoBehaviour
    {
#if UNITY_EDITOR
        [Header("Color")]
        public Color clr=Color.green;
        [Header("Inspector")]
        public float width;
        public float height;
        RectTransform rect;
        float x, y;
        void Start() => rect = GetComponent<RectTransform>();


        void Update()
        {
            width = rect.rect.width;
            height = rect.rect.height;
            x = transform.position.x;
            y = transform.position.y;

            DrawQuadrant();
        }

        void DrawQuadrant()
        {
            float w = width / 2;
            float h = height / 2;

            Vector2 a = new Vector2(x - w, y - h);
            Vector2 b = new Vector2(x - w, y + h);
            Vector2 c = new Vector2(x + w, y +h);
            Vector2 d = new Vector2(x + w, y - h);

            Debug.DrawLine(a, b, clr);
            Debug.DrawLine(b, c, clr);
            Debug.DrawLine(c, d, clr);
            Debug.DrawLine(d, a, clr);

        }
#endif
    }
}
