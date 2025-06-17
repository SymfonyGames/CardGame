using UnityEngine;

namespace Font
{
    [ExecuteInEditMode]
    public class TextFontManager : MonoBehaviour
    {

        #region Singleton
        //-------------------------------------------------------------
        public static TextFontManager Instance;
        void Awake()
        {
            if (Instance == null) Instance = this;
            else
            {
                Debug.LogWarning("Two instance of singleton here", gameObject);
                //gameObject.SetActive(false);
            }
        }
        //-------------------------------------------------------------
        #endregion
 
        public TextFontData textFontData;

    }
}
