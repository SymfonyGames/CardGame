using System;
using System.Collections.Generic;
using Level;
using Managers;
using Misc;
using Player;
using Plugins.AudioManager.audio_Manager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class CardHero : Card, IHealth, IShield, IHitable, IMana
{
    #region Inspetor References

    [Space(20)]
    [SerializeField] DamageText damageText;
    [SerializeField] PlayerMove movement;

    [Header("Image main")]
    [SerializeField] Image cardFrontFrameImage;
    [SerializeField] Image cardBackBodyImage;

    [Header("Image Glow")]
    [SerializeField] Image cardFrontGlowImage;
    [SerializeField] Image cardBackGlowImage;

    [Header("Label Link")]
    [SerializeField] TextMeshProUGUI nameLabel;
    [SerializeField] TextMeshProUGUI healthLabel;
    [SerializeField] TextMeshProUGUI manaLabel;
    [SerializeField] TextMeshProUGUI possibleDamage;
    [SerializeField] Text shieldLabel;

    [Header("Displayed Components")]
    [SerializeField] RectTransform healthComponent;
    [SerializeField] RectTransform shieldComponent;

    [Header("Fron/Back")]
    [SerializeField] RectTransform CardFront;
    [SerializeField] RectTransform CardBack;



    public List<GameObject> levelUpVfx = new();
    public TextMeshProUGUI lvlUpVfxTxt;

    #endregion

    public bool HealAtLevelUp = true;
    public bool RestoreManaAtLevelUp;

    #region Init

    public override void Init(CardDataHero data, LevelThemeData theme)
    {
        Data = data;
        InitName(data.name);
        InitImages(data, theme);
        InitStats(data);
        InitComponents();
        InitVFX(data);
        UpdateLabels();
        movement.Init(data);
        lvlText.text = "Lvl." + 1;
        foreach (var vfx in levelUpVfx)
            vfx.SetActive(false);
    }

    public TextMeshProUGUI lvlText;

    void LevelUp(int lvl)
    {
        lvlText.text = "Lvl." + lvl;
        lvlUpVfxTxt.text = lvl.ToString();

        foreach (var vfx in levelUpVfx)
            vfx.SetActive(true);

        MaxHp += Data.Stats.HpBonusPerLvl;
        MaxMana += Data.Stats.MpBonusPerLvl;

        if (HealAtLevelUp)
        {
            Invoke(nameof(HealFull), 1f);
        }
    }

    void HealFull()
    {
        _hp = MaxHp;
        _mana = MaxMana;
        UpdateLabels();
    }

    void InitName(string nameText)
    {
        name = nameText;
        if (nameLabel) nameLabel.text = nameText;
    }

    void InitImages(CardDataHero data, LevelThemeData theme)
    {
        art.sprite = data.Art;
        if (!theme) return;
        //  cardFrontFrameImage.sprite = theme.CardFrontFrame;
        cardFrontGlowImage.sprite = theme.CardFrontGlow;
        cardBackBodyImage.sprite = theme.CardBackBody;
        cardBackGlowImage.sprite = theme.CardBackGlow;
    }

    public float MaxHp { get; private set; }


    void InitStats(CardDataHero data)
    {
        _hp = MaxHp = data.Health + data.Shield;
        _shield = 0;
        _mana = MaxMana = data.Mana;
    }

    void InitComponents()
    {
        if (healthComponent) healthComponent.gameObject.SetActive(_hp > 0);
        if (shieldComponent) shieldComponent.gameObject.SetActive(_shield > 0);
        if (!cardFrontFrameImage.sprite) cardFrontFrameImage.enabled = false;
        if (!cardFrontGlowImage.sprite) cardFrontGlowImage.enabled = false;
        if (!cardBackBodyImage.sprite) cardBackBodyImage.enabled = false;
        if (!cardBackGlowImage.sprite) cardBackGlowImage.enabled = false;
    }

    void InitVFX(CardDataHero data)
    {
        if (data.AttackVFX && VFXparent)
        {
            _attackVFX = Instantiate(data.AttackVFX, VFXparent);
            _attackVFX.SetActive(false);
        }

        if (data.HealthPickupVFX && VFXparent)
        {
            _healVFX = Instantiate(data.HealthPickupVFX, VFXparent);
            _healVFX.SetActive(false);
        }

        if (data.ShieldPickupVFX && VFXparent)
        {
            _shieldVFX = Instantiate(data.ShieldPickupVFX, VFXparent);
            _shieldVFX.SetActive(false);
        }
    }

    #endregion

    #region Interfaces

    #region IHitable

    public float AttackPower => Random.Range(Data.DmgMin, Data.DmgMax);
    public bool IsImmune { get; private set; }

    public void SetImmune(bool isImmune)
    {
        IsImmune = isImmune;
    }

    public void PlayVFX(GameObject vfx)
    {
        Instantiate(vfx, art.transform);
    }

    public GameObject shieldVFX;

    public void SetShieldVFX(bool isShow)
    {
        shieldVFX.SetActive(isShow);
    }

    public event Action<float> OnDamage = delegate { };

    public void Hit(float damage)
    {
        var totalDamage = damage;

        if (IsImmune)
            totalDamage = 0;

        OnDamage(totalDamage);

        if (Shield > 0)
        {
            totalDamage -= damage;
            DecreaseShield(damage);
        }

        if (totalDamage > 0)
        {
            DecreaseHealth(totalDamage);
        }


        //   var danger = c.Data.Dangerous;
        ShowDamageText((int) totalDamage, CreatureDangerous.Easy, 0);
    }

    #endregion

    #region IHealth

    public float Health => _hp;
    public TextMeshProUGUI atkDmgText;

    public void AddHealth(float amount)
    {
        if (_hp >= MaxHp) return;

        var missingHp = MaxHp - _hp;
        if (missingHp < amount)
            amount = missingHp;

        _hp += amount;

        PlayHealVFX();
        if (amount > 0) damageText.ShowHeal((int) amount, 0.3f);
        UpdateLabels();

        EventManager.Instance.PlayerHitpointsChanged(_hp);
    }

    public void DecreaseHealth(float amount)
    {
        _hp -= amount;
        if (_hp < 0) _hp = 0;
        if (_hp <= 0) Dead();
        UpdateLabels();
        EventManager.Instance.PlayerHitpointsChanged(_hp);
    }

    #endregion

    #region IShield

    public float Shield => _shield;

    public PlayerMove Movement => movement;

    public void AddShield(float amount)
    {
        // if (_shield > 0) return;
        _shield += amount;
        PlayShieldVFX();
        UpdateLabels();
    }

    public void DecreaseShield(float amount)
    {
        _shield -= amount;
        if (_shield < 0) _shield = 0;
        UpdateLabels();
    }

    #endregion

    public float Mana => _mana;
    public float MaxMana { get; private set; }


    public void AddMana(float amount)
    {
        _mana += amount;
        if (_mana > MaxMana) _mana = MaxMana;
        OnManaChanged();
        UpdateLabels();
    }

    public void DecreaseMana(float amount)
    {
        _mana -= amount;
        if (_mana < 0) _mana = 0;
        OnManaChanged();
        UpdateLabels();
    }

    #endregion

    float _shield, _hp, _mana;
    GameObject _attackVFX;
    GameObject _healVFX;
    GameObject _shieldVFX;
    public event Action OnManaChanged = delegate { };
    public int walkDistance = 1;

    void Start()
    {
        EventManager.Instance.OnPlayerHeal += HealMe;
        EventManager.Instance.OnPlayerShield += ShieldMe;
        EventManager.Instance.OnPlayerAttackVFX += AttackVFX;
        EventManager.Instance.OnHeroLevelUp += LevelUp;
    }

    void OnDisable()
    {
        EventManager.Instance.OnPlayerHeal -= HealMe;
        EventManager.Instance.OnPlayerShield -= ShieldMe;
        EventManager.Instance.OnPlayerAttackVFX -= AttackVFX;
        EventManager.Instance.OnHeroLevelUp -= LevelUp;
    }

    public void ShowDamageText(int damage, CreatureDangerous danger, float delay)
    {
        if (damage == 0) return;
        damageText.ShowDelayed(damage, danger, delay);
    }

    void AttackVFX()
    {
        PlayAttackVFX();
    }

    void HealMe(float amount)
    {
        AddHealth(amount);
    }

    void ShieldMe(int amount)
    {
        AddShield(amount);
    }

    public override bool IsAlive => _hp > 0;

    public override bool IsDead => _hp <= 0;

    public CardDataHero Data { get; private set; }

    public void HidePossibleDamage()
    {
        possibleDamage.enabled = false;
        if (healthLabel) healthLabel.text = $"{_hp + _shield}";
    }

    public void SetPossibleDamage(int dmg)
    {
        possibleDamage.enabled = true;
        possibleDamage.text = "-" + dmg;
        // if (healthLabel)
        // {
        //     healthLabel.text = $"{_hp + _shield - dmg}";
        // }
    }

    public override void Interact(Card card, int iteration = 0)
    {
        if (card is IHitable enemy)
        {
            var dealDamage = AttackPower;
            //    var takenDamage = enemy.AttackPower;

            //  DamageMyself(takenDamage, card);
            enemy.Hit(dealDamage);


            //  var crit = Random.Range(0, 1f) < 0.2f ? 2.5f:1f;
            //   var dealDamage = Random.Range(14f,16f)*crit;
            //   var takenDamage = 1;

            //  DamageMyself(takenDamage, card);
            //  enemy.Hit(dealDamage);
            // if (card is CardBoss)
            //     DamageMyself(takenDamage, card);
            // else
            //     HealMe(takenDamage);
        }
        else
        {
            Regen();
        }
    }

    public void Regen()
    {
        HealMe(Data.Stats.HpRegen);
        AddMana(Data.Stats.MpRegen);
    }

    public override void Revive()
    {
        cardModel.SetActive(true);
        _hp = Data.Health;
        _shield = Data.Shield;
        UpdateLabels();
    }

    void DamageMyself(float amount, Card card)
    {
        Hit(amount);
        if (card is CardCreature c)
        {
            var danger = c.Data.Dangerous;
            ShowDamageText((int) amount, danger, 0);
        }

        if (card is CardBoss b)
        {
            //  var danger = b.Data.Dangerous;
            ShowDamageText((int) amount, CreatureDangerous.Hard, 0);
        }
    }

    void Dead()
    {
        PlaySound(Data.DeathSound);
        Invoke(nameof(DelayDeath), 0.25f);
    }

    public GameObject cardModel;

    void DelayDeath()
    {
        cardModel.SetActive(false);
        EventManager.Instance.PlayerDeath();
    }

    public bool showSimpleHPText;
    public bool showSimpleManaText;

    void UpdateLabels()
    {
        if (healthLabel)
        {
            healthLabel.text = showSimpleHPText ? $"{_hp + _shield}" : $"{_hp + _shield}/{MaxHp}";
        }

        if (shieldLabel) shieldLabel.text = $"{_shield}";
        if (manaLabel)
        {
            manaLabel.text = showSimpleManaText ? $"{_mana}" : $"{_mana}/{MaxMana}";
            //  manaLabel.text = $"{_mana}/{MaxMana}";
        }

        if (atkDmgText)
        {
            atkDmgText.text = Data.DmgMin == Data.DmgMax ? $" {Data.DmgMax}" : $"{Data.DmgMin}-{Data.DmgMax}";
        }


        shieldComponent.gameObject.SetActive(_shield > 0);

        hpBar.SetValue((float) _hp / MaxHp);
        manaBar.SetValue((float) _mana / MaxMana);
    }

    public CustomSlider hpBar;
    public CustomSlider manaBar;
    public CustomSlider expBar;

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
    protected override void PlayAttackSound() => PlaySound(Data.AttackSound);

    public override void PlayAttackVFX()
    {
        if (_attackVFX) _attackVFX.SetActive(true);
    }

    void PlayHealVFX()
    {
        if (_healVFX) _healVFX.SetActive(true);
    }

    void PlayShieldVFX()
    {
        if (_shieldVFX) _shieldVFX.SetActive(true);
    }

    public override void EnableGlow(Color clr)
    {
        cardFrontGlowImage.color = clr;
        cardBackGlowImage.color = clr;
        cardFrontGlowImage.enabled = true;
        cardBackGlowImage.enabled = true;
    }

    public override void DisableGlow()
    {
        cardFrontGlowImage.enabled = false;
        cardBackGlowImage.enabled = false;
    }

    public void ChangeArtwork(Sprite sprite)
    {
        art.sprite = sprite;
    }

    public void CheatSetupHP(int amount)
    {
        _hp = amount;
        UpdateLabels();
    }
}