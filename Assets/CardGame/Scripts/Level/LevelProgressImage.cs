using UnityEngine;
using UnityEngine.UI;

namespace Level
{
    public class LevelProgressImage : MonoBehaviour
    {
        [SerializeField] private Image image;

        public void Setup(Sprite sprite)
        {
            image.sprite = sprite;
        }


    }
}
