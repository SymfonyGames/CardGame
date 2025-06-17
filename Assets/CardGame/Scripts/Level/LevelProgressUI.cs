using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Level
{
    public class LevelProgressUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI totalLines;
        [SerializeField] TextMeshProUGUI currentLine;
        [SerializeField] LevelProgressImage prefab;
        [SerializeField] RectTransform sliderTransform;
        [SerializeField] Slider slider;
        [SerializeField] Transform miniBossIconContainer;
        List<LevelProgressImage> _icons = new List<LevelProgressImage>();


        public void Setup(int totalEnemyLines)
        {
            totalLines.text = totalEnemyLines.ToString();
        }

        public void Refresh(float fillValue, int current)
        {
            slider.DOValue(fillValue, 1f);
            currentLine.text = current.ToString();
        }

        public void AddIcon(Sprite sprite, float progress)
        {
            var img = Instantiate(prefab, miniBossIconContainer);
            img.Setup(sprite);

            var height = sliderTransform.rect.height;
            var imgPosY = height * progress;
            img.transform.localPosition = new Vector3(0, imgPosY, 0);
        }
    }
}