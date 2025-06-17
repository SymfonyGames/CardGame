using DataSave;
using Maps;
using UnityEngine;

namespace TESTS
{
    public class TEST_MAP_UI : MonoBehaviour
    {
        [Header("Test")]
        public bool refreshMap;
        public bool show;
        public bool hide;
        [Header("Setup")]
        public Map map;
        public MapUI mapsUI;
 
 
        public MapSave mapSave;
 
    
#if UNITY_EDITOR
        private void Update()
        {
 
            if (refreshMap)
            {
                refreshMap = false;
                mapsUI.RefreshPointers(mapSave.sequencesData);
                mapsUI.SetCurrentPointer(mapSave);
                mapsUI.MoveMapToCurrentPointer();
                mapsUI.MoveHeroChipInstantToCurrent();
            }
        
            if (show)
            {
                show = false;
                map.Show();
            }
        
            if (hide)
            {
                hide = false;
                map.Hide();
            }
        
        }
#endif
    }
}