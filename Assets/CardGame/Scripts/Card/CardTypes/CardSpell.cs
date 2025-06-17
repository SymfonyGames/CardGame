using Level;
using Plugins.AudioManager.audio_Manager;
using UnityEngine;
using UnityEngine.UI;

public class CardSpell : Card
{
    
    #region Inspetor References
    [Header("Image main")]

    [SerializeField] Image cardFrontFrameImage;
    [SerializeField] Image cardBackBodyImage;

    [Header("Image Glow")]
    [SerializeField] Image cardFrontGlowImage;
    [SerializeField] Image cardBackGlowImage;
    
    [Header("Label link")]
    [SerializeField] Text nameLabel;
    
    [Header("Fron/Back")]
    [SerializeField] RectTransform CardFront;
    [SerializeField] RectTransform CardBack;
    #endregion
    
    #region Init
    public override void Init(CardDataSpell data, LevelThemeData theme)
    {
        RotateChance = data.ChanceRotation;
        InitName(data.name);
        InitImages(data, theme);
        InitStats(data);
        InitComponents();
        UpdateLabels();
    }
    void InitName(string nameText)
    {
        name = nameText;
        if (nameLabel) nameLabel.text = nameText;
    }
    void InitImages(CardDataSpell data, LevelThemeData theme)
    {
        art.sprite = data.Art;
        if(!theme)return;
        cardFrontFrameImage.sprite = theme.CardFrontFrame;
        cardFrontGlowImage.sprite = theme.CardFrontGlow;
        cardBackBodyImage.sprite = theme.CardBackBody;
        cardBackGlowImage.sprite = theme.CardBackGlow;
    }
    void InitStats(CardDataSpell data)
    {
        _data = data;
    }
    void InitComponents()
    {
        if (!cardFrontFrameImage.sprite) cardFrontFrameImage.enabled = false;
        if (!cardFrontGlowImage.sprite) cardFrontGlowImage.enabled = false;
        if (!cardBackBodyImage.sprite) cardBackBodyImage.enabled = false;
        if (!cardBackGlowImage.sprite) cardBackGlowImage.enabled = false;
    }
    #endregion

    CardDataSpell _data;

    public override bool IsAlive => false;

    public override bool IsDead => true;

 
    public override void Interact(Card card, int iteration)
    {
        PlaySound(_data.InteractSound);
        Dead();
    }
    
    void Dead()
    {
        Destroy(gameObject, 0.25f);
    }
    void UpdateLabels()
    {
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
    }

    public override void DisableGlow()
    {
        cardFrontGlowImage.enabled = false;
        cardBackGlowImage.enabled = false;
    }
}
