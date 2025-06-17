using System;
using System.Collections.Generic;
using System.Linq;
using Level;
using Managers;
using Misc;
using Player;
using Plugins.AudioManager.audio_Manager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardArtefact : Card
{
    #region Inspetor References

    [Header("Image main")]
    [SerializeField] Image cardFrontFrameImage;
    [SerializeField] Image cardBackBodyImage;

    [Header("Image Glow")] [SerializeField]
    Image cardFrontGlowImage;

    [SerializeField] Image cardBackGlowImage;
    [SerializeField] GameObject values;

    [Header("Label Link")]
    [SerializeField] TextMeshProUGUI nameLabel;
    [SerializeField] TextMeshProUGUI healthLabel;
    [SerializeField] TextMeshProUGUI shieldLabel;
    [SerializeField] TextMeshProUGUI coinLabel;
    [SerializeField] TextMeshProUGUI manaLabel;
    [SerializeField] TextMeshProUGUI gemLabel;

    [Header("Displayed Components")]
    [SerializeField] RectTransform healthComponent;
    [SerializeField] RectTransform shieldComponent;
    [SerializeField] RectTransform coinComponent;
    [SerializeField] RectTransform manaComponent;
    [SerializeField] RectTransform gemComponent;
    [SerializeField] Image frame;
    [SerializeField] Color goldColor;
    [SerializeField] Color manaColor;
    [SerializeField] Color shieldColor;
    [SerializeField] Color hpColor;

    [Header("Fron/Back")]
    [SerializeField] RectTransform CardFront;
    [SerializeField] RectTransform CardBack;
    [SerializeField] FlipBackAtClick flipBackAtClick;

    [Header("VFX")] [SerializeField] Transform vfxParent;

    #endregion

    public List<Sprite> abilitiesSprites = new();
    public List<Sprite> sprites_2 = new();
    public List<Sprite> sprites_3 = new();

    public List<CardDataHero> heroes = new();


    #region Init

    public override void Init(CardDataArtifact data, LevelThemeData theme, bool portalMode = false)
    {
        RotateChance = data.ChanceRotation;

        InitName(data.name);
        InitImages(data, theme);
        InitData(data);
        InitComponents();
        if (portalMode) InitVFX(data);

        InitPortalMode(portalMode);
        UpdateLabels();
    }

    public override void Init(CardDataArtifact data, LevelThemeData theme)
    {
        RotateChance = data.ChanceRotation;

        InitName(data.name);
        InitImages(data, theme);
        InitData(data);
        InitComponents();
        InitGlowColors(data);

        InitPortalMode(false);
        UpdateLabels();
    }


    void InitVFX(CardDataArtifact data)
    {
        if (data.PortalHealthVFX && vfxParent)
        {
            healVFX = Instantiate(data.PortalHealthVFX, vfxParent);
            healVFX.SetActive(false);
        }

        if (data.PortalShieldVFX && vfxParent)
        {
            shieldVFX = Instantiate(data.PortalShieldVFX, vfxParent);
            shieldVFX.SetActive(false);
        }

        if (data.PortalCoinVFX && vfxParent)
        {
            coinVFX = Instantiate(data.PortalCoinVFX, vfxParent);
            coinVFX.SetActive(false);
        }

        if (data.PortalManaVFX && vfxParent)
        {
            manaVFX = Instantiate(data.PortalManaVFX, vfxParent);
            manaVFX.SetActive(false);
        }
    }


    void InitName(string nameText)
    {
        name = nameText;
        if (nameLabel) nameLabel.text = nameText;
    }

    void InitImages(CardDataArtifact data, LevelThemeData theme)
    {
        art.sprite = data.Art;
        if (!theme) return;
        // cardFrontFrameImage.sprite = theme.CardFrontFrame;
        cardFrontGlowImage.sprite = theme.CardFrontGlow;
        cardBackBodyImage.sprite = theme.CardBackBody;
        cardBackGlowImage.sprite = theme.CardBackGlow;
    }

    List<PotionData> _potions = new();

    void InitData(CardDataArtifact data)
    {
        _data = data;
        _HP = data.HP;
        _SHIELD = data.Shield;
        _GOLD = data.Gold;
        _GEM = data.Gem;
       _MANA = data.Mana;
        _potions = data.Potions.ToList();
        if (_potions.Count > 0)
        {
            art.material = null;
        }
    }

    int _GEM;
    public Image abilityIcon;
    public GameObject abilityIconCont;
    public void DisableFrame() => frame.enabled = false;
    public void HideValues()
    {
        values.SetActive(false);
    }

    void InitComponents()
    {
        abilityIconCont.SetActive(false);
        frame.enabled = _HP > 0 || _SHIELD > 0 || _MANA > 0 || _GOLD > 0 || _data.Silver>0;
        // if (_HP > 0) frame.color = hpColor;
        // if (_SHIELD > 0) frame.color = shieldColor;
        // if (_MANA > 0) frame.color = manaColor;
        // if (_GOLD > 0) frame.color = goldColor;
        //   if (_GEM > 0) frame.color = goldColor;

        if (_potions.Count > 0)
        {
            abilityIconCont.SetActive(true);
            abilityIcon.enabled = true;
            frame.enabled = true;

            var hero = PlayerStash_heroes.Instance.PlayerData.selectedHero;

            if (_potions.Where(p => p.type == PotionType.Barby).ToList().Count > 0)
            {
                frame.color = hpColor;
                abilityIcon.sprite = abilitiesSprites[0];
            }

            if (_potions.Where(p => p.type == PotionType.Ken).ToList().Count > 0)
            {
                frame.color = manaColor;
                abilityIcon.sprite = abilitiesSprites[1];
                if (heroes.Count != 0 && heroes.Contains(hero))
                {
                    var id = heroes.IndexOf(hero);

                    abilityIcon.sprite = sprites_2[id];
                }
            }

            if (_potions.Where(p => p.type == PotionType.Jessica).ToList().Count > 0)
            {
                frame.color = shieldColor;
                abilityIcon.sprite = abilitiesSprites[2];
                if (heroes.Count != 0 && heroes.Contains(hero))
                {
                    var id = heroes.IndexOf(hero);

                    abilityIcon.sprite = sprites_3[id];
                }
            }
        }

        if (healthComponent) healthComponent.gameObject.SetActive(_HP > 0);
        if (shieldComponent) shieldComponent.gameObject.SetActive(_SHIELD > 0);
        if (coinComponent) coinComponent.gameObject.SetActive(_GOLD > 0);
        if (manaComponent) manaComponent.gameObject.SetActive(_MANA > 0);
        if (gemComponent) gemComponent.gameObject.SetActive(_MANA > 0);

        if (!cardFrontFrameImage.sprite) cardFrontFrameImage.enabled = false;
        if (!cardFrontGlowImage.sprite) cardFrontGlowImage.enabled = false;
        if (!cardBackBodyImage.sprite) cardBackBodyImage.enabled = false;
        if (!cardBackGlowImage.sprite) cardBackGlowImage.enabled = false;
    }

    void InitGlowColors(CardDataArtifact data)
    {
        if (cardFrontGlowImage) cardFrontGlowImage.color = data.GlowColor;
        if (cardBackGlowImage) cardBackGlowImage.color = data.GlowColor;
    }

    void InitPortalMode(bool portalMode)
    {
        // EventManager.Instance.OnPortalClose += PortalClosed;
        _imInPortalMode = portalMode;
        flipBackAtClick.isEnabled = portalMode;
        EventManager.Instance.OnPortalPauseActions += DisableFlipAtClick;
        EventManager.Instance.OnPortalContinueActions += EnableFlipAtClick;
    }

    #endregion

    void OnDisable()
    {
        if (!_imInPortalMode) return;
        EventManager.Instance.OnPortalPauseActions -= DisableFlipAtClick;
        EventManager.Instance.OnPortalContinueActions -= EnableFlipAtClick;
    }

    void DisableFlipAtClick() => flipBackAtClick.isEnabled = false;
    void EnableFlipAtClick() => flipBackAtClick.isEnabled = true;

    GameObject healVFX;
    GameObject shieldVFX;
    GameObject coinVFX;
    GameObject manaVFX;
    bool _imInPortalMode = false;
    int _SHIELD;
    [HideInInspector] public int _HP;
    int _GOLD;

    int _MANA;
    CardDataArtifact _data;

    public override bool IsAlive => false;

    public override bool IsDead => true;


    public override void Interact(Card card, int iteration = 0)
    {
        TryHeal(card, iteration);
        TryShield(card, iteration);
        TryGold();
        TryPotions();
        TryGem();
        if (_data.Exp > 0) EventManager.Instance.ExperienceDrop(transform.position, _data.Exp);
       TryMana(card);
        PlaySound(_data.InteractSound);
        if (!_imInPortalMode) Dead();
    }

    void TryGem()
    {
        if (_GEM > 0)
            PlayerStash_resources.Instance.AddGem(_GEM);
    }

    void TryPotions()
    {
        foreach (var data in _potions)
            EventManager.Instance.UsePotion(data);
    }

    void TryHeal(Card card, int iteration = 0)
    {
        if (_HP <= 0) return;
        if (card is IHealth health)
        {
            health.AddHealth(curHP);
            HealVFX();
        }
    }


    void TryShield(Card card, int iteration = 0)
    {
        if (_SHIELD <= 0) return;
        if (card is IShield shield)
        {
            if (card is CardHero)
                EventManager.Instance.PlayerShield(_SHIELD * (1 + iteration));
            else
                shield.AddShield(_SHIELD * (1 + iteration));

            ShieldVFX();
        }
    }

    void TryGold(int iteration = 0)
    {
        if (_GOLD > 0)
        {
            EventManager.Instance.CreateGoldVFX(transform.position, _GOLD * (1 + iteration));
            CoinVFX();
        }

        var silver = _data.Silver;
        if (silver > 0)
        {
            EventManager.Instance.CreateSilverVFX(transform.position, silver * (1 + iteration));
           // CoinVFX();
        }
    }

    void TryMana(Card card)
    {
        if (_MANA <= 0) return;
        if (card is IMana mana)
        {
            mana.AddMana(_MANA);
            ManaVFX();
        }
    }

    void Dead()
    {
        Destroy(gameObject, 0.25f);
    }

    int curHP;

    public override void UpdateLabels(int iteration = 0)
    {
        if (healthLabel && _HP > 0)
        {
            curHP = _HP * (1 + iteration);
            healthLabel.text = $"+{curHP}";
        }

        if (shieldLabel && _SHIELD > 0) shieldLabel.text = $"+{_SHIELD * (1 + iteration)}";
        if (manaLabel && _MANA > 0) manaLabel.text = $"+{_MANA * (1 + iteration)}";
        if (coinLabel && _GOLD > 0) coinLabel.text = $"+{_GOLD * (1 + iteration)}";
        if (gemLabel && _GEM > 0) gemLabel.text = $"+{_GEM * (1 + iteration)}";
    }

    protected override void CardBackState()
    {
        CardFront.gameObject.SetActive(false);
        CardBack.gameObject.SetActive(true);
    }

    protected override void CardFrontState()
    {
        CardFront.gameObject.SetActive(true);
        CardBack.gameObject.SetActive(false);
    }

    void PlaySound(SoundData sound) => AudioManager.Instance.PlaySound(sound);

    public override void EnableGlow(Color clr)
    {
        cardFrontGlowImage.color = clr;
        cardBackGlowImage.color = clr;
        cardFrontGlowImage.enabled = true;
        cardBackGlowImage.enabled = true;
        cardFrontGlowImage.gameObject.SetActive(true);
        cardBackGlowImage.gameObject.SetActive(true);
    }

    public override void DisableGlow()
    {
        cardFrontGlowImage.enabled = false;
        cardBackGlowImage.enabled = false;
    }

    void HealVFX()
    {
        if (healVFX) healVFX.SetActive(true);
    }

    void ShieldVFX()
    {
        if (shieldVFX) shieldVFX.SetActive(true);
    }

    void CoinVFX()
    {
        if (coinVFX) coinVFX.SetActive(true);
    }

    void ManaVFX()
    {
        if (manaVFX) manaVFX.SetActive(true);
    }
}