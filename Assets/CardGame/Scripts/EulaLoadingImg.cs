 
using UnityEngine;
using UnityEngine.UI;

public class EulaLoadingImg : MonoBehaviour
{
     public Image img;

     public void Show()
     {
       //   Time.timeScale = 1;
       img.gameObject.SetActive(true);

       //  img.DOFade(1, 0.2f);
     }
}
