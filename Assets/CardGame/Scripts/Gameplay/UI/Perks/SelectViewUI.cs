using System;
using System.Collections.Generic;
using System.Linq;
using Gameplay.UI.Views;
using TMPro;
using UnityEngine;

namespace Gameplay.UI.Perks
{
    public class SelectViewUI : ViewUI
    {
        [Header("Owned")]
        [SerializeField] TextMeshProUGUI label;
        [Header("Select slots")]
        [SerializeField] List<SelectSlotUI> slots = new();

        [Header("Create")]
        [SerializeField] SelectSlotUI selectSlotPrefab;
        [SerializeField] Transform container;
        readonly List<SelectSlotUI> _subscribed = new();
        public event Action<int> OnSlotSelected = delegate { };

        public void SetLabel(string labelName)
        {
            label.text = labelName;
        }
        protected override void OnShowUI() { }

        void OnDisable()
        {
            foreach (var slot in _subscribed)
                slot.OnClick -= Select;
        }

        public void Refresh(List<HeroAbilityData> perks)
        {
            CheckSlotsCount(perks.Count);
            for (var i = 0; i < perks.Count; i++)
                slots[i].Set(perks[i]);
        }

        void Select(SelectSlotUI slot)
        {
            OnSlotSelected(slots.IndexOf(slot));
            Hide();
        }

        void CheckSlotsCount(int require)
        {
            var created = slots.Count;

            if (created < require)
                CreateSlots(require);

            if (created > require)
                DisableSlots(created - require);

            foreach (var slot in slots.Where(slot => !_subscribed.Contains(slot)))
            {
                slot.OnClick += Select;
                _subscribed.Add(slot);
            }
        }


        void CreateSlots(int amount)
        {
            for (var i = 0; i < amount; i++)
            {
                if (slots.Count > i) continue;

                var slot = Instantiate(selectSlotPrefab, container);
                slot.transform.localScale = Vector3.one;
                slots.Add(slot);
            }
        }

        void DisableSlots(int amount)
        {
            var last = slots.Count - 1;
            for (int i = last; i > last - amount; i--)
                slots[i].Disable();
        }
    }
}