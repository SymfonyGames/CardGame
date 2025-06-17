using DG.Tweening;
using Level;
using Managers;
using Misc;
using Plugins.AudioManager.audio_Manager;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class CardCreature : Card, IHealth, IShield, IHitable, IDrop
{
    #region Inspector References

    [SerializeField] DamageText damageText;
    [SerializeField] CardDeathVFX cardDeathVFX;

    [Header("Image main")]
    [SerializeField] Image cardFrontFrameImage;
    [SerializeField] Image cardBackBodyImage;

    [Header("Image Glow")]
    [SerializeField] Image cardFrontGlowImage;
    [SerializeField] Image cardBackGlowImage;

    [Header("Label Link")]
    [SerializeField] TextMeshProUGUI nameLabel;
    [SerializeField] TextMeshProUGUI coinRewardLabel;
    [SerializeField] TextMeshProUGUI healthLabel;
    [SerializeField] TextMeshProUGUI shieldLabel;
    [SerializeField] TextMeshProUGUI goldRewardLabel;

    [Header("Displayed Components")]
    [SerializeField] RectTransform healthComponent;
    [SerializeField] RectTransform shieldComponent;

    [Header("Front/Back")]
    [SerializeField] RectTransform CardFront;
    [SerializeField] RectTransform CardBack;

    [Header("VFX")]
    [SerializeField] Transform attackVFXparent;
    [SerializeField] Image dangerImage;
    [SerializeField] Sprite easyDanger;
    [SerializeField] Sprite normalDanger;
    [SerializeField] Sprite hardDanger;

    #endregion

    #region Init

    public GameObject aggresive;

    public override void Init(CardDataCreature data, LevelThemeData theme, ConfigData config)
    {
        //  _expDrop = config.GetExpDrop(data.Health);
        _expDrop = data.ExpDrop;
        RotateChance = data.ChanceRotation;
        Data = data;
        InitName(data.name);
        InitImages(data, theme);
        InitStats(data);
        InitComponents();
        InitVFX(data);
        UpdateLabels();
        UpdateDanger(data.Dangerous);
        aggresive.SetActive(data.IsAggressive);
    }

    void UpdateDanger(CreatureDangerous danger)
    {
        dangerImage.sprite = danger switch
        {
            CreatureDangerous.Easy => easyDanger,
            CreatureDangerous.Normal => normalDanger,
            CreatureDangerous.Hard => hardDanger,
            _ => dangerImage.sprite
        };
    }

    void InitName(string nameText)
    {
        name = nameText;
        if (nameLabel) nameLabel.text = nameText;
    }

    void InitImages(CardDataCreature data, LevelThemeData theme)
    {
        art.sprite = data.Art;
        if (!theme) return;
        // cardFrontFrameImage.sprite = theme.CardFrontFrame;
        cardFrontGlowImage.sprite = theme.CardFrontGlow;
        cardBackBodyImage.sprite = theme.CardBackBody;
        cardBackGlowImage.sprite = theme.CardBackGlow;
    }

    public float MaxHP;

    void InitStats(CardDataCreature data)
    {
        _hp = 1;
        _dmg = MaxHP = Random.Range(data.DmgMin, data.DmgMax + 1) + data.Shield;
        // _hp = MaxHP = 100;
        //  _shield = data.Shield;
        _curSilver = data.SilverDrop;
    }

    void InitComponents()
    {
        if (coinRewardLabel) coinRewardLabel.text = Data.GoldDrop.ToString();
        if (healthComponent) healthComponent.gameObject.SetActive(_dmg > 0);
        if (healthLabel) healthLabel.gameObject.SetActive(_dmg > 0);
        if (shieldComponent) shieldComponent.gameObject.SetActive(_shield > 0);
        if (shieldLabel) shieldLabel.gameObject.SetActive(_shield > 0);
        if (!cardFrontFrameImage.sprite) cardFrontFrameImage.enabled = false;
        if (!cardFrontGlowImage.sprite) cardFrontGlowImage.enabled = false;
        if (!cardBackBodyImage.sprite) cardBackBodyImage.enabled = false;
        if (!cardBackGlowImage.sprite) cardBackGlowImage.enabled = false;
    }

    public void PlayVFX(GameObject vfx)
    {
        Instantiate(vfx, art.transform);
    }

    void InitVFX(CardDataCreature data)
    {
        if (data.AttackVFX && attackVFXparent)
        {
            attackVFX = Instantiate(data.AttackVFX, attackVFXparent);
            attackVFX.gameObject.SetActive(false);
        }
    }

    #endregion

    #region Interfaces

    #region IHitable

    public void ShowDamageText(int damage, float delay = 0)
    {
        damageText.ShowDelayed(damage, Data.Dangerous, delay);
    }

    public float AttackPower => Random.Range(Data.DmgMin,Data.DmgMax);

    public void Hit(float damage)
    {
        var totalDamage = damage;

        if (Shield > 0)
        {
            totalDamage -= Shield;
            DecreaseShield(damage);
        }

        if (totalDamage > 0)
        {
            DecreaseHealth(totalDamage);
        }

        ShowDamageText((int) damage);
        UpdateLabels();
    }

    #endregion

    #region IHealth

    public float Health => _curHp;

    public void AddHealth(float amount)
    {
        _dmg += amount;
        UpdateLabels();
    }

    public void DecreaseHealth(float amount)
    {
        _hp = 0;
        
       // _dmg -= amount;
       // if (_dmg < 0) _dmg = 0;
        if (IsDead) Dead();
        UpdateLabels();
    }

    #endregion

    #region IShield

    public float Shield => _shield;

    public void AddShield(float amount)
    {
        _shield += amount;
        UpdateLabels();
    }

    public void DecreaseShield(float amount)
    {
        _shield -= amount;
        if (_shield < 0) _shield = 0;
        UpdateLabels();
    }

    #endregion

    #endregion

    [Header("DEBUG")]
    public float _shield;
    [FormerlySerializedAs("_hp")] [Header("DEBUG")]
    public float _dmg;
    public float _hp;

    public override bool IsAlive => _hp > 0;
    public override bool IsDead => _hp <= 0;

    public CardDataCreature Data { get; private set; }


    ParticleSystem attackVFX;

    public override void Interact(Card card, int iteration)
    {
        if (card is CardHero hero)
        {
            hero.Hit(AttackPower);
        }
       
    }

    float _curHp;
    int _curGold;
    int _curSilver;
    float _expDrop;

    public override void UpdateLabels(int iteration = 0)
    {
        if (healthLabel && _dmg > 0)
        {
            // curHP= _health -( iteration);
            _curHp = _dmg * (1 + iteration);
            healthLabel.text = $"{(int) _curHp}";
        }

        if (shieldLabel) shieldLabel.text = $"{_shield}";
        if (goldRewardLabel)
        {
            _curGold = Data.GoldDrop * (1 + iteration * 2);
            goldRewardLabel.text = $"{_curGold}";
        }

        if (iteration > 0)
        {
            var time = 0.3f;
            var size = 2f;
            var sizeFinal = 1.3f;

            var t = healthLabel.transform.parent;
            t.DOScale(size, time / 2).OnComplete(() => t.DOScale(sizeFinal, time / 2));
            var g = goldRewardLabel.transform.parent;
            g.DOScale(size, time / 2).OnComplete(() => g.DOScale(sizeFinal, time / 2));
        }
        else
        {
            healthLabel.transform.localScale = Vector3.one;
            goldRewardLabel.transform.localScale = Vector3.one;
        }

        hpBar.SetValue((float) _dmg / MaxHP);
    }

    public CustomSlider hpBar;

    void Dead()
    {
        EventManager.Instance.ExperienceDrop(transform.position, _expDrop);
        CoinReward();
        PlaySound(Data.DeathSound, 0);
        cardDeathVFX.Show(0.25f);
        Destroy(gameObject, 0.25f);
    }

    void CoinReward()
    {
        if (Data.GoldDrop > 0)
        {
            // EventManager.Instance.CoinsCreate(Cell.transform.localPosition + transform.localPosition, _data.CoinReward);
            EventManager.Instance.CreateGoldVFX(transform.position, _curGold);
        }

        if (Data.SilverDrop > 0)
        {
            // EventManager.Instance.CoinsCreate(Cell.transform.localPosition + transform.localPosition, _data.CoinReward);
            EventManager.Instance.CreateSilverVFX(transform.position, _curSilver);
        }
    }

    public void DropArtefact(bool isNullPossible, CardDataArtifact baseDrop)
    {
        //  EventManager.Instance.CreateAtCell(isNullPossible ? Data.GetRandomDrop() : baseDrop, Cell);
        EventManager.Instance.CreateAtCell(baseDrop, Cell);
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
    void PlaySound(SoundData sound, float delay) => AudioManager.Instance.PlaySound(sound, delay);
    protected override void PlayAttackSound() => PlaySound(Data.AttackSound);

    public override void PlayAttackVFX()
    {
        if (attackVFX) attackVFX.gameObject.SetActive(true);
    }

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
        cardFrontGlowImage.gameObject.SetActive(false);
        cardBackGlowImage.gameObject.SetActive(false);
    }
}