using UnityEngine;
using UnityEngine.UI;

// [ExecuteInEditMode]
public class CustomSlider : MonoBehaviour
{
    [Header("VALUE")]
    [SerializeField] [Range(0, 1)] float sliderValue;
    [Header("Settings")]
    [SerializeField] bool useGradient;
    [SerializeField] Gradient gradient;
    [Header("Setup")]
    public bool useSlider = true;
    [SerializeField] Slider fill;
    [SerializeField] Image fillImage;
    [SerializeField] Image fillShadowImage;
    [SerializeField] Slider fillShadow;
    [Header("Colors")]
    [SerializeField] Color shadowPlusColor;
    [SerializeField] Color shadowMinusColor;

    [Header("Shadow")]
    [SerializeField] bool useShadow;
    [SerializeField] float shadowThreshold = 0.05f;
    [SerializeField] float shadowCatchSpeed = 0.05f;

    [Header("TEST")]
    public bool testPlus;
    public bool testMinus;
    ShadowMoveType _moveType;
    bool _isMove;
    public float Value { get; private set; }

    public enum ShadowMoveType
    {
        SliderMove,
        ShadowMove
    }

    public void AddValue(float value)
    {
        var newValue = Value += value;
        SetValue(newValue);
    }

    public void SetValue(float value)
    {
        Value = value;
        if (useShadow)
        {
            if (!_isMove)
            {
                var gap = value - sliderValue;
                if (Mathf.Abs(gap) < shadowThreshold)
                {
                    FillInstant(value);
                }
                else
                {
                    FillShadow(value, gap);
                }
            }
            else
            {
                var dif = _moveType == ShadowMoveType.SliderMove
                    ? value - fill.value
                    : value - fillShadow.value;
                FillShadow(value, dif);
            }
        }
        else
        {
            FillInstant(value);
        }

        sliderValue = value;
        sliderValue = Mathf.Clamp01(sliderValue);
    }

    void FillShadow(float value, float dif)
    {
        _isMove = true;
        fillShadow.gameObject.SetActive(true);
        if (dif > 0)
        {
            //++
            fillShadowImage.color = shadowPlusColor;
            fillShadow.value = value;
            _moveType = ShadowMoveType.SliderMove;
        }
        else
        {
            //--
            fillShadowImage.color = shadowMinusColor;
            if (useSlider) fill.value = value;
            else fillImage.fillAmount = value;
            _moveType = ShadowMoveType.ShadowMove;
        }
    }

    public void FillInstant(float value)
    {
        if (useSlider) fill.value = value;
        else fillImage.fillAmount = value;
        fillShadow.value = value;
        RefreshColor();
    }


    void RefreshColor()
    {
        if (useGradient)
            fillImage.color = gradient.Evaluate(fill.value);
    }


    void Update()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            fill.value = sliderValue;
            RefreshColor();
        }


        if (testPlus)
        {
            testPlus = false;
            SetValue(sliderValue + 0.2f);
        }

        if (testMinus)
        {
            testMinus = false;
            SetValue(sliderValue - 0.2f);
        }

#endif

        if (_isMove)
        {
            var gap = fillShadow.value - fill.value;

            if (_moveType == ShadowMoveType.SliderMove)
            {
                if (Mathf.Abs(gap) > 0.02f)
                {
                    float dir = gap > 0 ? 1 : -1;
                    fill.value += dir * shadowCatchSpeed * Time.deltaTime;
                    fillShadow.value = sliderValue;
                }
                else
                {
                    _isMove = false;
                    if (useSlider) fill.value = sliderValue;
                    else fillImage.fillAmount = sliderValue;
                    fillShadow.gameObject.SetActive(false);
                }

                RefreshColor();
            }

            if (_moveType == ShadowMoveType.ShadowMove)
            {
                if (Mathf.Abs(gap) > 0.02f)
                {
                    float dir = gap > 0 ? -1 : 1;
                    fillShadow.value += dir * shadowCatchSpeed * Time.deltaTime;
                    if (useSlider) fill.value = sliderValue;
                    else fillImage.fillAmount = sliderValue;
                    RefreshColor();
                }
                else
                {
                    _isMove = false;
                    fillShadow.value = sliderValue;
                    fillShadow.gameObject.SetActive(false);
                }
            }
        }
    }
}