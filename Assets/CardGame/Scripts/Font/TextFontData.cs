using TMPro;
using UnityEngine;

namespace Font
{
    [CreateAssetMenu(fileName = "App text font", menuName = "Game/AppFont")]
    public class TextFontData : ScriptableObject
    {
        public UnityEngine.Font appTextFont;
        public TMP_FontAsset appTextFontTMP;
    }
}
