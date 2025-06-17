using System;
using System.Collections.Generic;
using System.Linq;
using Gameplay.UI.Views;
using UnityEngine;

namespace Gameplay.UI.Perks
{
    public class AbilitySelectUI : ViewUI
    {
        [Header("Owned")]
        // [SerializeField] PerkPanelUI activePerks;
        //  [SerializeField] PerkPanelUI passivePerks;
        [Header("Select slots")]
        [SerializeField] List<SelectSlotUI> slots = new();

        [Header("Create")]
        [SerializeField] SelectSlotUI selectSlotPrefab;
        [SerializeField] Transform container;

        readonly List<SelectSlotUI> _subscribed = new();

        protected override void OnShowUI()
        {
            //    var player = Scene.Instance.Player;
            //    activePerks.Set(player.ActivePerks);
            //    passivePerks.Set(player.PassivePerks);
        }

        void OnDisable()
        {
            foreach (var slot in _subscribed)
                slot.OnClick -= Select;
        }

        public void Refresh(IReadOnlyList<HeroAbilityData> abis)
        {
            CheckSlotsCount(abis.Count);
            for (var i = 0; i < abis.Count; i++)
                slots[i].Set(abis[i]);
        }

        public event Action<SelectSlotUI> OnSelected = delegate(SelectSlotUI ui) { };

        void Select(SelectSlotUI slot)
        {
            OnSelected(slot);
            //GameplayEvents.Instance.PerkSelected(slot.Perk);
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
            {
                slots[i].Disable();
            }
        }
    }
}