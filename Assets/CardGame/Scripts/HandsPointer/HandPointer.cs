using DG.Tweening;
using Plugins.AudioManager.audio_Manager;
using UnityEngine;

namespace HandsPointer
{
    public class HandPointer : MonoBehaviour
    {
        public bool followMouse;
        public Camera cam;
        public DOTweenAnimation clickAnim;
        public SoundData clickSound;
        void Awake()
        {
            cam = Camera.main;
        }

        public float smooth = 0.1f;
        public Vector3 offset;

        void Update()
        {
             
            if (Input.GetMouseButtonDown(0))
            {
         
                clickAnim.DORestart();
                AudioManager.Instance.PlaySound(clickSound);
            }
        }

        void FixedUpdate()
        {
            if (!followMouse) return;
            var pos = Input.mousePosition;
            var cp = cam.ScreenToWorldPoint(pos) + offset;
            var smoothpos = Vector3.Lerp(transform.position, cp, this.smooth);
            transform.position = smoothpos;
        
            //
            // Vector2 movePos;
            //
            // RectTransformUtility.ScreenPointToLocalPointInRectangle(
            //     canvas.transform as RectTransform,
            //     Input.mousePosition, canvas.worldCamera,
            //     out movePos);
            //
            // hand.position = canvas.transform.TransformPoint(movePos);
            //
            // if (Input.GetMouseButtonDown(0))
            // {
            //     anim.DORestart();
            //     anim.DOPlay();
            // }
        }

        public void Disable()
        {
        }
    }
}