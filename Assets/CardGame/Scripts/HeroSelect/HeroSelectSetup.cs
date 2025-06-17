using TMPro;
using UnityEngine;
using UnityEngine.UI;

//#if UNITY_EDITOR
//using UnityEditor.Experimental.SceneManagement;
//#endif

namespace HeroSelect
{
   // [ExecuteInEditMode]
    [RequireComponent(typeof(HeroSelect))]
    public class HeroSelectSetup : MonoBehaviour
    {
        [Header("Artwork")]
        [SerializeField] Image artWorkImage;

        [Header("Label Link")]
        [SerializeField] TextMeshProUGUI nameLabel;
        [SerializeField] TextMeshProUGUI healthLabel;
        [SerializeField] TextMeshProUGUI levelLabel;
        [SerializeField] Text shieldLabel;

        [Header("Displayed Components")]
        [SerializeField] RectTransform healthComponent;
        [SerializeField] RectTransform shieldComponent;

        [SerializeField] HeroSelect _heroSelect;
        CardDataHero _hero;

        void Start()
        {
            if (!_heroSelect)
                _heroSelect = GetComponent<HeroSelect>();
            _hero = _heroSelect.Hero;
            RefreshContainers();
        }

        void RefreshContainers()
        {
            if (_hero)
            {
                artWorkImage.sprite = _hero.Art;
                nameLabel.text = _hero.Name;

                healthComponent.gameObject.SetActive(_hero.Health > 0);
                healthLabel.text = _hero.Health.ToString();

                shieldComponent.gameObject.SetActive(_hero.Shield > 0);
                shieldLabel.text = _hero.Shield.ToString();
            }
            else
            {
                artWorkImage.sprite = null;
                nameLabel.text = "";

                healthComponent.gameObject.SetActive(false);
                shieldComponent.gameObject.SetActive(false);
            }
        }

//    #region UNITY EDITOR

//#if UNITY_EDITOR
//    const string DEFAULT_NAME = "hero";

//    void Update()
//    {
//        if (!heroSelect) heroSelect = GetComponent<HeroSelect>();

//        hero = heroSelect.Hero;
//        var prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
//        if (!prefabStage) RenameGameobject();

//        RefreshContainers();
//    }

//    void RenameGameobject()
//    {
//        string resultName = hero ? DEFAULT_NAME + " (" + hero.name + ")" : DEFAULT_NAME;
//        if (name != resultName) name = resultName;
//    }
//#endif

//    #endregion
    }
}