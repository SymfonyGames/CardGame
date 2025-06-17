using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BossGame
{
    public class HitpointUI : MonoBehaviour
    {
        [SerializeField] Slider slider;
        [SerializeField] TextMeshProUGUI txt;

        public void Refresh(float value, int current)
        {
            txt.text = current.ToString();
            var from = slider.value;
            var to = value;
            //   slider.value = value;
            DOVirtual.Float(from, to, 0.3f, (float f) => slider.value = f);
        }
    }
}