using Player;
using UnityEngine;

//#if UNITY_EDITOR
//using UnityEditor.Experimental.SceneManagement;
//using UnityEditor.SceneManagement;
//#endif

namespace HeroSelect
{
 //   [ExecuteInEditMode]
  //  [RequireComponent(typeof(HeroSelect))]
    public class HeroSelectOwned : MonoBehaviour
    {
        [Header("Status:")]
        [SerializeField] bool owned;
        [SerializeField] HeroSelect heroSelect;
        public bool isOwned => PlayerStash_heroes.Instance.IsHeroOwned(heroSelect.Hero);
        private bool isFree => heroSelect.Hero.GoldCost == 0 && heroSelect.Hero.GemCost == 0;
 
        public HeroSelect HeroSelect => heroSelect;
        public void SetOwn(bool isOwn)
        {
            owned = isOwn;
        }

        void Start()
        {
            if (Application.isPlaying)
            {
                if (!heroSelect)
                    heroSelect = GetComponent<HeroSelect>();

                Invoke(nameof(CheckIfHeroFree), 0.1f);
            }
        }


        void CheckIfHeroFree()
        {
            if (isFree && !isOwned)
                OwnThisHero(heroSelect.Hero);


        }

        void OwnThisHero(CardDataHero hero) => PlayerStash_heroes.Instance.Own(hero);


//    #region UNITY_EDITOR

//#if UNITY_EDITOR
//    void Update()
//    {
//        if (PrefabStageUtility.GetCurrentPrefabStage() != null)
//        {
//            return;
//        }

//        if (!heroSelect)
//            heroSelect = GetComponent<HeroSelect>();

//        if (heroSelect.Hero == null)
//        {
//            owned = false;
//            return;
//        }

//        if (!PlayerStash_heroes.Instance)
//            return;

//        owned = isOwned;
//        CheckIfHeroFree();
//    }
//#endif

        //#endregion
    }
}