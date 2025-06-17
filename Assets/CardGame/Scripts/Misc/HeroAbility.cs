using System;
using System.Collections.Generic;
using Gameplay.UI.Perks;
using Level;
using Managers;
using Plugins.AudioManager.audio_Manager;
using UnityEngine;
using UnityEngine.Serialization;

namespace Misc
{
    public abstract class HeroAbility : MonoBehaviour
    {
        [SerializeField] PotionType potionType;
        [SerializeField] HeroAbilityUI ui;
        [SerializeField] SoundData sound;

        public List<CardDataHero> heroes = new();
        protected int Lvl = 1;
        int cdTimer;
        int _potionAmount;
        protected CardHero _hero;
        CardDataHeroAbility ability => data.Ability;
        [SerializeField] protected HeroAbilityData data;
        public PotionType PotionType => potionType;
        public event Action OnUse = delegate { };
        int ManaCost => ability.ManaCost(data.Level);
        int CoolDown => ability.Cooldown(data.Level);

        public void Set(CardHero hero, HeroAbilityData ability)
        {
            data = ability;
            _hero = hero;
            _hero.OnManaChanged += RefreshMana;
            ui.Init(hero.Data, ability);
            RefreshUI();

            EventManager.Instance.OnCardsMoveDown -= NextLine;
            EventManager.Instance.OnCardsMoveDown += NextLine;
        }

        void RefreshMana()
        {
            ui.RefreshMP((int) _hero.Mana, ManaCost);
        }

        public void Init(CardDataHeroAbility ability, HeroAbilityUI abilityUI, int lvl)
        {
            //    this.ability = ability;

            //    ui = abilityUI;
            // Lvl = lvl;
        }

        void OnDisable()
        {
            EventManager.Instance.OnCardsMoveDown -= NextLine;
        }

        void NextLine(float moveTime)
        {
            if (cdTimer <= 0) return;
            cdTimer--;
            RefreshUI();
        }


        void OnEnable()
        {
            ui.ClickButton.onClick.AddListener(Click);
        }

        void Start()
        {
            Invoke(nameof(RefreshUI), 1f);
            //RefreshUI();
        }

        void Click()
        {
            if(data.Level <1) return;
            if (_hero.Mana < ManaCost) return;
            if (cdTimer > 0) return;
            Use();
            OnUse();
        }

        public void AddPotion(PotionData potion)
        {
            return;
            if (_potionAmount >= ManaCost)
                _potionAmount = ManaCost;
            _potionAmount += potion.Value;
            RefreshUI();
        }


        protected abstract void UseAbility();

        void Use()
        {
            cdTimer = CoolDown;
            _hero.DecreaseMana(ManaCost);
            //_potionAmount -= ManaCost;
            AudioManager.Instance.PlaySound(sound);
            RefreshUI();
            UseAbility();
        }

        void RefreshUI()
        {
            if (!_hero) return;

            if (_hero.Mana < ManaCost)
                ui.Disable();
            else
                ui.Enable();

            ui.SetCooldown(cdTimer, (float) cdTimer / CoolDown);
            ui.RefreshMP((int) _hero.Mana, ManaCost);
            ui.SetLevel(data.Level);
            return;
            
            
            
            
            ui.SetProgress((float) _potionAmount / ManaCost, _potionAmount, ManaCost);
        }
    }
}