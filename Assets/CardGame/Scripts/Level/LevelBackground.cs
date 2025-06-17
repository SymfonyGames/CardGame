using UnityEngine;
using UnityEngine.UI;

namespace Level
{
    [ExecuteInEditMode]
    public class LevelBackground : MonoBehaviour
    {
        LevelTheme levelTheme;
        [SerializeField] Image levelBackground_1;
        [SerializeField] Image levelBackground_2;
        [SerializeField] Image colorFilterImage;
        [SerializeField] GameObject vfxCanvas;

        void Start()
        {
            levelTheme = LevelTheme.Instance;
            if (levelTheme && levelTheme.Data)
            {
                if (levelTheme.Data.Theme.Background)
                {
                    if (levelBackground_1) levelBackground_1.sprite = levelTheme.Data.Theme.Background;
                    if (levelBackground_2) levelBackground_2.sprite = levelTheme.Data.Theme.Background;
                }

                if (levelTheme.Data.Theme.Background && vfxCanvas && levelTheme.Data.Theme.EnvoirementVfXprefab)
                {
                    if (!Application.isPlaying) return;
                    Instantiate(levelTheme.Data.Theme.EnvoirementVfXprefab, vfxCanvas.transform);
                }

                colorFilterImage.color = levelTheme.Data.Theme.FilterColor;
            }
        }

#if UNITY_EDITOR
        void Update()
        {
            if (Application.isPlaying) return;
            if (levelTheme && levelTheme.Data && levelTheme.Data.Theme.Background)
            {
                if (levelBackground_1 && levelBackground_1.sprite != levelTheme.Data.Theme.Background) levelBackground_1.sprite = levelTheme.Data.Theme.Background;
                if (levelBackground_2 && levelBackground_2.sprite != levelTheme.Data.Theme.Background) levelBackground_2.sprite = levelTheme.Data.Theme.Background;
            }
        }
#endif
    }
}