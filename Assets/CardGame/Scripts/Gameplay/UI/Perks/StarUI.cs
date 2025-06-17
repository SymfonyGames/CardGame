using System.Collections;
 
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.UI.Perks
{
    public class StarUI : MonoBehaviour
    {
        [SerializeField] Image star;
        [SerializeField] float animSpeed=2;
        bool _fade;
        float _fadeAmount;

        public void Enable()
        {
            star.enabled = true;
        }

        public void Disable()
        {
            star.enabled = false;
        }

        void StopAnimation() => StopAllCoroutines();

      //  [Button]
        void PlayAnimation()
        {
            StartCoroutine(Animation());
        }

        IEnumerator Animation()
        {
            Enable();
            
            _fade = false;
            _fadeAmount = 0;
            Fade(_fadeAmount);
            
            while (true)
            {
                Anim();
                yield return new WaitForSecondsRealtime(0.01f);
            }
        }

        void Anim()
        {
            if (_fade)
            {
                _fadeAmount -= 0.01f*animSpeed;
                if (_fadeAmount < 0)
                    _fade = !_fade;
            }
            else
            {
                _fadeAmount += 0.01f*animSpeed;
                if (_fadeAmount > 1)
                    _fade = !_fade;
            }

            Fade(_fadeAmount);
        }

        void Fade(float alpha)
        {
            var clr = star.color;
            clr.a = alpha;
            star.color = clr;
        }
    }
}