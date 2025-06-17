using System;
using UnityEngine;
using UnityEngine.UI;

namespace MenuTabs
{
    [ExecuteInEditMode]
    public class TabUI : MonoBehaviour
    {
        [SerializeField] Button button;
        [SerializeField] TabStateAnim anim;
        public event Action OnClick = delegate { };

        public TabStateAnim Anim => anim;

        void Start()
            => button.onClick.AddListener(Click);

        void OnDisable()
            => button.onClick.RemoveListener(Click);

        void Click() 
            => OnClick();

        public void SelectedAnim()
        {
            if (anim) anim.Active();
        }

        public void UnSelectedAnim()
        {
            if (anim) anim.Inactive();
        }
    }
}