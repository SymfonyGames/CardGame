using TMPro;
using UnityEngine;
using UnityEngine.UI;

//[ExecuteInEditMode]
namespace Font
{
    public class TextFontSetup : MonoBehaviour
    {
        [Header("Setup manualy for beter perfomance")]
        public Text txt;
        public TextMeshProUGUI txtTMP;


        void Start()
        {
            return;
            
            if (!txt) txt = GetComponent<Text>();
            if (!txtTMP) txtTMP = GetComponent<TextMeshProUGUI>();
            if (txt || txtTMP) TrySetupFont();
        }


        void TrySetupFont()
        {
            if (TextFontManager.Instance && TextFontManager.Instance.textFontData)
            {
                if (txt) txt.font = TextFontManager.Instance.textFontData.appTextFont;
                if (txtTMP) txtTMP.font = TextFontManager.Instance.textFontData.appTextFontTMP;
            }
        }


    }
}
