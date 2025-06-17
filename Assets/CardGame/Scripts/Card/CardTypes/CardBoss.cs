using System.Collections.Generic;
using DG.Tweening;
using Level;
using Managers;
using Misc;
using Plugins.AudioManager.audio_Manager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardBoss : Card, IHealth, IShield, IHitable
{
    #region Inspector References

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
    [SerializeField] Text shieldLabel;

    [Header("Displayed Components")]
    [SerializeField] RectTransform healthComponent;
    [SerializeField] RectTransform shieldComponent;

    [Header("Fron/Back")]
    [SerializeField] RectTransform CardFront;
    [SerializeField] RectTransform CardBack;

    [Header("VFX")]
    [SerializeField] Transform attackVFXparent;

    #endregion

    public CustomSlider hpBar;

    #region Init

    float _expDrop;

    public override void Init(CardDataBoss data, LevelThemeData theme)
    {
        _expDrop = data.ExpDrop;
        Data = data;
        InitName(data.Name);
        InitImages(data, theme);
        InitStats(data);
        InitComponents();
        InitVFX(data);
        UpdateLabels();
    }

    void InitName(string nameText)
    {
        name = nameText;
        if (nameLabel) nameLabel.text = nameText;
    }

    public Image atkCdImg;
    public TextMeshProUGUI atkCdTxt;
    int atkCd;

    void InitImages(CardDataBoss data, LevelThemeData theme)
    {
        art.sprite = data.Art;
        if (!theme) return;
        // cardFrontFrameImage.sprite = theme.CardFrontFrame;
        cardFrontGlowImage.sprite = theme.CardFrontGlow;
        cardBackBodyImage.sprite = theme.CardBackBody;
        cardBackGlowImage.sprite = theme.CardBackGlow;
    }

    public float MaxHP;

    void InitStats(CardDataBoss data)
    {
        atkCd = data.AttackInterval;
        lives = data.Lives;
       // lives = 3;
        _health = MaxHP = Random.Range(data.HpMin, data.HpMax + 1);
        //   _hp = MaxHP = Random.Range(data.HealthMin, data.HealthMax + 1) + data.Shield;
        _shield = data.Shield;
    }

    void InitComponents()
    {
        if (coinRewardLabel) coinRewardLabel.text = Data.GoldDrop.ToString();
        if (healthComponent) healthComponent.gameObject.SetActive(_health > 0);
        if (shieldComponent) shieldComponent.gameObject.SetActive(_shield > 0);
        if (!cardFrontFrameImage.sprite) cardFrontFrameImage.enabled = false;
        if (!cardFrontGlowImage.sprite) cardFrontGlowImage.enabled = false;
        if (!cardBackBodyImage.sprite) cardBackBodyImage.enabled = false;
        if (!cardBackGlowImage.sprite) cardBackGlowImage.enabled = false;
        livesObj.SetActive(lives > 1);
    }

    void InitVFX(CardDataBoss data)
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

    public float AttackPower => Random.Range(Data.DmgMin, Data.DmgMax);

    public void Hit(float damage)
    {
    //    lives--;
  //   if(lives>1)  
     //    livesObjects[lives].SetActive(false);
        // if (lives <= 0)
        // {
        //     damage = 99999;
        // }
        
        float totalDamage = damage;

        if (Shield > 0)
        {
            totalDamage -= Shield;
            DecreaseShield(damage);
        }

        if (totalDamage > 0)
        {
            DecreaseHealth(totalDamage);
        }
        
        ShowDamageText((int) totalDamage, CreatureDangerous.Easy, 0);
    }
    [SerializeField] DamageText damageText;
    public void ShowDamageText(int damage, CreatureDangerous danger, float delay)
    {
        if (damage == 0) return;
        damageText.ShowDelayed(damage, danger, delay);
    }
    #endregion

    #region IHealth

    public float Health => _health;

    public void AddHealth(float amount)
    {
        _health += amount;
        UpdateLabels();
    }

    float takenDmg;

    public void DecreaseHealth(float amount)
    {
        
        takenDmg += amount;
        _health -= amount;
     //   _health =0;
        if (_health < 0) _health = 0;
        CheckIsAlive();
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

    public bool IsMiniBoss { get; set; }
    float _shield, _health;

    public override bool IsAlive => _health > 0;

    public override bool IsDead => _health <= 0;

    public CardDataBoss Data { get; private set; }


    ParticleSystem attackVFX;
    public GameObject cdObject;

    public override void Interact(Card card, int iteration)
    {
        
        if (card is CardHero h)
            h.Hit(AttackPower);
        
        return;
        atkCd--;
        UpdateLabels();


        if (atkCd > 0)
        {
            cdObject.SetActive(true);
            DOVirtual.Float(atkCdImg.fillAmount, (float) atkCd / Data.AttackInterval, 1f, f => atkCdImg.fillAmount = f);
        }
        else
        {
            if (cdObject.activeSelf)
                DOVirtual.Float(atkCdImg.fillAmount, 0, 0.3f, f => atkCdImg.fillAmount = f)
                    .OnComplete(() => cdObject.SetActive(false));
        }

        if (atkCd >= 0) return;

        if (card is CardHero hero)
        {
            //   this.AgressiveAttackAnimation(card.Position);

            atkCd = Data.AttackInterval;
            cdObject.SetActive(true);
            atkCdImg.fillAmount = 1;

            var sequence = DOTween.Sequence();
            var moveDur = 0.3f;

            transform.DOScale(1f, moveDur).SetDelay(0.7f);

            sequence.Append(transform.DOScale(1.3f, 0.2f).SetDelay(0.4f).OnComplete(PlayAttackVFX));
            //  sequence.Append(transform.DOMove(card.Cell.Position, moveDur).SetEase(Ease.OutQuad).OnComplete(()=>DoDmg(card)));
            sequence.Append(transform.DOScale(1, 0.2f).SetEase(Ease.OutQuad).OnComplete(() => DoDmg(card)));

            //  sequence.Append(transform.DOMove(card.Cell.Position, moveDur));
            sequence.Append(transform.DOShakePosition(0.3f, new Vector2(10, 10)));
            //  sequence.Append(transform.DOMove(Position, moveDur));
        }
    }

    void DoDmg(Card card)
    {
        if (card is CardHero hero)
            hero.Hit(AttackPower);

        card.PlayHitVFX(Data.AttackHitVFX);
    }

    int lives;
    public GameObject livesObj;
    public List<GameObject> livesObjects=new();
    public TextMeshProUGUI livesTxt;
    public TextMeshProUGUI atkDmgTxt;

    void UpdateLabels()
    {
        
        livesTxt.text = lives.ToString();
        //  if (healthLabel) healthLabel.text = $"{Data.HealthMin - takenDmg}-{Data.HealthMax - takenDmg}";
        if (healthLabel) healthLabel.text = $"{(int) _health}/{(int) MaxHP}";
        //     if (healthLabel) healthLabel.text = $"{_health}";
        if (shieldLabel) shieldLabel.text = $"{_shield}";
        if (atkDmgTxt)
            atkDmgTxt.text = Data.DmgMin == Data.DmgMax ? Data.DmgMin.ToString() : $"{Data.DmgMin}-{Data.DmgMax}";
        hpBar.SetValue((float) _health / MaxHP);

        atkCdTxt.text = atkCd.ToString();
        //  atkCdImg.fillAmount = (float) atkCd / Data.AttackInterval;
    }

    void CheckIsAlive()
    {
        if (IsDead)
        {
         //   Invoke(nameof(Dead), 1.5f);
          
            Dead();
        }
    }

    void Dead()
    {
        EventManager.Instance.ExperienceDrop(transform.position, _expDrop);
        CoinReward();
        PlaySound(Data.DeathSound, 0.25f);
        Destroy(gameObject, 0.25f);
    }

    void CoinReward()
    {
        if (Data.GoldDrop > 0)
        {
            EventManager.Instance.CreateGoldVFX(transform.position,
                Data.GoldDrop);
        }
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