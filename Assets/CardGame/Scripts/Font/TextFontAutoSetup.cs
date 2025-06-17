using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Font
{
    [ExecuteInEditMode]
    public class TextFontAutoSetup : MonoBehaviour
    {
#if UNITY_EDITOR
        public bool autoFindTexts_AddFontSetupComponent;
        public Text[] allTextObejcts;
        public TextMeshProUGUI[] allTextObejctsTMP;


        void Update()
        {
            if (autoFindTexts_AddFontSetupComponent)
            {
                allTextObejcts = Resources.FindObjectsOfTypeAll<Text>();
                if (allTextObejcts.Length > 0)
                {
                    foreach (var item in allTextObejcts)
                    {
                        var fontSetup = item.GetComponent<TextFontSetup>();
                        if (!fontSetup)
                        {
                            item.gameObject.AddComponent<TextFontSetup>();
                            Debug.Log("There was no FontSetup component, I added it for you, no thanks", item.gameObject);
                        }
                    }
                }

                allTextObejctsTMP = Resources.FindObjectsOfTypeAll<TextMeshProUGUI>();
                if (allTextObejctsTMP.Length > 0)
                {
                    foreach (var item in allTextObejctsTMP)
                    {
                        var fontSetup = item.GetComponent<TextFontSetup>();
                        if (!fontSetup)
                        {
                            item.gameObject.AddComponent<TextFontSetup>();
                            Debug.Log("There was no FontSetup component, I added it for you, no thanks", item.gameObject);
                        }
                    }
                }
            }
        }

#endif
    }
}
