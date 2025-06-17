using System.Collections.Generic;
using Plugins.AudioManager.audio_Manager;
using UnityEngine;

namespace MenuTabs
{
    public class TabsPanelController : MonoBehaviour
    {
        [Header("ParentTab")]
        [SerializeField] Tab parent;
        [Header("Tabs")]
        [SerializeField] Tab openByDefault;
        [SerializeField] List<Tab> childTabs = new();
        Tab _lastOpenedTabUI;
        public List<Tab> ChildTabs => childTabs;
        public SoundData ClickSound { get; set; }

        void Start()
        {
            SubscribeParent();
            SubscribeChild();
            OpenDefault();
        }

        void SubscribeParent()
        {
            if (!parent) return;
            parent.OnTabEnabled += Enable;
            parent.OnTabDisabled += Disable;
        }

        void SubscribeChild()
        {
            foreach (var tab in childTabs)
                tab.OnClick += Click;
        }

        void OnDisable()
        {
            if (parent)
            {
                parent.OnTabEnabled -= Enable;
                parent.OnTabDisabled -= Disable;
            }

            foreach (var tab in childTabs)
                tab.OnClick -= Click;
        }

        void Enable()
        {
            OpenDefault();
        }

        void Disable()
        {
            CloseLastTab();
        }


        void Click(Tab tab)
        {
            if (AudioManager.Instance)
                AudioManager.Instance.PlaySound(ClickSound);
            RefreshTabs(tab);
        }

        void RefreshTabs(Tab selected)
        {
            foreach (var tab in childTabs)
            {
                if (selected == tab)
                {
                    _lastOpenedTabUI = tab;
                    tab.Enable();
                }
                else
                {
                    tab.Disable();
                }
            }
        }

        void OpenDefault()
        {
            if (openByDefault)
            {
                RefreshTabs(openByDefault);
            }
            else
            {
                if (childTabs[0])
                    RefreshTabs(childTabs[0]);
                else
                    Debug.LogError("myTabs.Count = 0");
            }
        }

        void CloseLastTab()
        {
            if (_lastOpenedTabUI)
                _lastOpenedTabUI.Disable();
        }
    }
}