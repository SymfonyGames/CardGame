using DG.Tweening;
using Level;
using Managers;
using Misc;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public abstract class Card : MonoBehaviour
{
    [SerializeField] RectTransform container;
    [SerializeField] Canvas canvas;
    [SerializeField] CanvasGroup canvasGroup;
    [FormerlySerializedAs("cardArtworkImage")]
    [SerializeField] protected Image art;
    [Header("VFX")]
    [SerializeField] protected Transform VFXparent;
    public void PlayHitVFX(ParticleSystem vfx)
    {
         Instantiate(vfx, VFXparent);
    }
    public void Hide(float alpha = 0)
    {
        canvasGroup.DOFade(alpha, 0.2f);
    }

    public enum State
    {
        Close,
        Busy,
        Open
    }

    public float RotateChance { get; protected set; }
    public State CurrentState { get; private set; } = State.Open;
    public Cell Cell { get; set; }
    Material _grayMaterial;
    Material _creatureMaterial;

    public Vector3 Position => transform.position;

    public void SetBusy() => CurrentState = State.Busy;
    public void SetFree() => CurrentState = State.Open;
    public void SetClose() => CurrentState = State.Close;

    public virtual void UpdateLabels(int iteration = 0)
    {
    }

    Canvas Canvas
    {
        get
        {
            if (!canvas) canvas = GetComponent<Canvas>();
            return canvas;
        }
    }

    public int SortingOrder
    {
        get => Canvas.sortingOrder;
        set => Canvas.sortingOrder = value;
    }

    public abstract void Interact(Card card, int iteration = 0);
    public abstract bool IsAlive { get; }
    public abstract bool IsDead { get; }

    public RectTransform Container => container;

    bool flipDisabled;
    Material _heroMaterial;
    Material _artefactMaterial;

    public void ArtGray()
    {
        art.material = _grayMaterial;
    }

      bool forceEmptyMaterial;

    public void ThisIsRoad()
    {
         forceEmptyMaterial = true;
        RemoveMaterial();
        if (this is CardArtefact a)
        {
            a.DisableFrame();
            a.HideValues();
        }
    }
    public void RemoveMaterial()
    {
        art.material = null;
    }

    public void ArtNormal()
    {
        // if (forceEmptyMaterial)
        // {
        //     art.material = null;
        //     return;
        // }

        switch (this)
        {
            case CardHero:
                art.material = _heroMaterial;
                return;
            case CardArtefact:
                art.material = forceEmptyMaterial ? null : _artefactMaterial;
                return;
            case CardCreature:
                art.material = _creatureMaterial;
                return;
            default:
                art.material = null;
                break;
        }
    }

    public void Flip()
    {
        if (flipDisabled) return;

        transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
        CurrentState = State.Close;
        CardBackState();
    }

    public void FlipBack()
    {
        if (flipDisabled) return;

        CurrentState = State.Busy;
        var rotateTime = 0.5f;

        EventManager.Instance.FlipBack(this);

        var sequence = DOTween.Sequence();
        sequence.Append(transform.DOLocalRotate(new Vector3(0, -90, 0), rotateTime / 2).OnComplete(CardFrontState));
        sequence.Append(transform.DOLocalRotate(Vector3.zero, rotateTime / 2).OnComplete(FlipBackEnded));
    }

    public void FlipBackEnded()
    {
        CurrentState = State.Open;
        EventManager.Instance.FlipBackFinish(this);
    }


    public void DisableFlip() => flipDisabled = true;
    public void EnableFlip() => flipDisabled = false;


    protected abstract void CardBackState();
    protected abstract void CardFrontState();

    public abstract void EnableGlow(Color clr);
    public abstract void DisableGlow();

    public void Glow()
    {
        if (this is CardArtefact)
            EnableGlow(Color.white);
        if (this is CardCreature cc)
        {
            cc.EnableGlow(Color.red);
            // if (cc.Data.Dangerous == CreatureDangerous.Easy)
            //     cc.EnableGlow(Color.yellow);
            // if (cc.Data.Dangerous == CreatureDangerous.Normal)
            //     cc.EnableGlow(new Color(1, 0.5f, 0));
            // if (cc.Data.Dangerous == CreatureDangerous.Hard)
            //     cc.EnableGlow(Color.red);
        }
    }

    public void MoveBack()
    {
        CurrentState = State.Busy;
        transform.DOLocalMove(Vector3.zero, 0.5f).OnComplete(() => CurrentState = State.Open);
    }

    public void AgressiveAttackAnimation(Vector3 attackPosition)
    {
        PlayAttackSound();

        CurrentState = State.Busy;
        var offset = new Vector3(0, 4.6f, 0);
        var offsetSmall = new Vector3(0, 0.4f, 0);
        var position = attackPosition + offset;

        var sequence = DOTween.Sequence();
        var moveDur = 0.3f;

        transform.DOScale(1f, moveDur).SetDelay(moveDur);

        sequence.Append(transform.DOScale(1.3f, 0.1f));
        sequence.Append(transform.DOMove(attackPosition + offsetSmall, moveDur).SetEase(Ease.InQuad)
            .OnComplete(PlayAttackVFX));

        sequence.Append(transform.DOMove(position, moveDur));
        sequence.Append(transform.DOShakePosition(0.3f, new Vector2(10, 10)));
        sequence.OnComplete(() => CurrentState = State.Open);
    }

    public void TouchAttackAnimation(Vector3 attackPosition, bool isAttack = true)
    {
        transform.DOKill();
        if (isAttack) PlayAttackSound();

        CurrentState = State.Busy;
        var offset = new Vector3(0, -2.5f, 0);
        var offsetSmall = new Vector3(0, -0.5f, 0);
        var position = attackPosition + offset;

        var sequence = DOTween.Sequence();
        var moveDur = 0.3f;

        transform.DOScale(1f, moveDur).SetDelay(moveDur);

        sequence.Append(transform.DOScale(1.3f, 0.1f));
        var mov = transform.DOMove(attackPosition + offsetSmall, moveDur).SetEase(Ease.InQuad);
        sequence.Append(mov);
        if (isAttack) mov.OnComplete(PlayAttackVFX);
        sequence.OnComplete(() => CurrentState = State.Open);
    }

    public void SwipeAttackAnim(Vector3 attackPosition, bool isAttack = true)
    {
        transform.DOKill();
        if (isAttack) PlayAttackSound();

        CurrentState = State.Busy;
        var offset = new Vector3(0, -2.5f, 0);
        var offsetSmall = new Vector3(0, -1f, 0);
        var position = attackPosition + offset;

        var sequence = DOTween.Sequence();
        var moveDur = 0.2f;

        var a = Random.Range(10, 20);
        var b = Random.value > 0.5f ? 1 : -1f;
        var rot = new Vector3(0, 0, a * b);
        var rotTime = 0.50f;

        var mov = transform.DOMove(attackPosition + offsetSmall, moveDur).SetEase(Ease.InQuad);
        sequence.Append(mov);
        if (isAttack) mov.OnComplete(PlayAttackVFX);

        sequence.Append(transform.DOScale(0.8f, 0.1f));
        sequence.Append(transform.DOScale(1f, 0.1f));
        sequence.OnComplete(() => CurrentState = State.Open);
    }

    public void PlayCollectAnimation(Vector3 collectPosition)
    {
        CurrentState = State.Busy;
        var offset = new Vector3(0, -2.5f, 0);
        //   var offsetSmall = this is CardHero ? new Vector3(0, -0.5f, 0) : new Vector3(0, 0.4f, 0);
        var position = collectPosition + offset;

        var sequence = DOTween.Sequence();
        var moveDur = 0.3f;

        //   transform.DOScale(1f, moveDur).SetDelay(moveDur);

        var mov =
            //  sequence.Append(transform.DOScale(1.3f, 0.2f));
            sequence.Append(transform.DOMove(position, moveDur).SetEase(Ease.OutQuad));
        //  .OnComplete(PlayAttackVFX));

        // sequence.Append(transform.DOMove(position, moveDur));
        // sequence.Append(transform.DOShakePosition(0.3f, new Vector2(10, 10)));
        sequence.OnComplete(() => CurrentState = State.Open);
    }

    protected virtual void PlayAttackSound()
    {
    }

    public virtual void PlayAttackVFX()
    {
    }

    public virtual void Revive()
    {
    }

    public void Set(GeneratorData data)
    {
        _artefactMaterial = data.ArtifactMaterial;
        _heroMaterial = data.HeroMaterial;
        _grayMaterial = data.GrayMaterial;
        _creatureMaterial = data.CreatureMaterial;
    }

    public virtual void Init(CardDataHero data, LevelThemeData theme)
    {
    }

    public virtual void Init(CardDataCreature data, LevelThemeData theme, ConfigData config)
    {
    }

    public virtual void Init(CardDataSpell data, LevelThemeData theme)
    {
    }

    public virtual void Init(CardDataPortal data, LevelThemeData theme)
    {
    }

    public virtual void Init(CardDataArtifact data, LevelThemeData theme)
    {
    }

    public virtual void Init(CardDataArtifact data, LevelThemeData theme, bool portalMode = false)
    {
    }

    public virtual void Init(CardDataBoss data, LevelThemeData theme)
    {
    }
}