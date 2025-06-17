using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Misc
{
    public class ImageFillOnEnable : MonoBehaviour
    {
        [SerializeField] Image image;
        [Header("Settings")]
        [SerializeField] bool enableFill=true;
        [SerializeField] float fillTime=1;


        void OnEnable()
        {
            if (enableFill) StartCoroutine(FillImage());
        }

        IEnumerator FillImage()
        {
            float speed = 1 / fillTime;
            float a = 0;
            while (a < 1)
            {
                a += speed * Time.deltaTime;
                image.fillAmount = a;
                yield return null;
            }
            image.fillAmount = 1;
        }
    
    }
}