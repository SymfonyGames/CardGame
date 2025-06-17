using System;
using UnityEngine;

//#if UNITY_EDITOR
//using UnityEditor.Experimental.SceneManagement;
//using UnityEditor.SceneManagement;
//#endif

namespace HeroSelect
{
 //   [ExecuteInEditMode]
    public class HeroSelect : MonoBehaviour
    {
        [Header("Select")] 
        [SerializeField] CardDataHero hero;
        //public bool save;
        public CardDataHero Hero => hero;
        public event Action<CardDataHero> OnHeroSelected = delegate { };
    
        public void HeroSelected()
        {
            Debug.Log("HERO SELECTED");
            if (hero) 
                OnHeroSelected(hero);
        }

//#if UNITY_EDITOR
//    public void Update()
//    {
//        if (save)
//        {
//            var prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
//            EditorSceneManager.MarkSceneDirty(prefabStage ? prefabStage.scene : gameObject.scene);
//        }
//    }
//#endif

    }
}