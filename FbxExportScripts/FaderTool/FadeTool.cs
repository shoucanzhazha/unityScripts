using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LogUtils;


public struct FadeStartEvent
{
    public int ID;
    public float Duration;
    public float TargetAlpha;
    public TweenType Curve;
    public bool IgnoreTimeScale;

    static FadeStartEvent startEvent;

    public static void Trigger(float duration, float targetAlpha, TweenType curve, int id = 0, bool ignoreTimeScale = true)
    {
        startEvent.ID = id;
        startEvent.Duration = duration;
        startEvent.TargetAlpha = targetAlpha;
        startEvent.Curve = curve;
        startEvent.IgnoreTimeScale = ignoreTimeScale;
        EventManager.TriggerEvent(startEvent);
    }
}

public struct FadeStopEvent
{
    public int ID;
    static FadeStopEvent stopevent;
    public static void Trigger(int id = 0)
    {
        stopevent.ID = id;
        EventManager.TriggerEvent(stopevent);
    }
}

public struct FadeInEvent
{
    public int ID;
    public float Duration;
    public TweenType Curve;
    public bool IgnoreTimeScale;

    static FadeInEvent fadeInEvent;

    public static void Trigger(float duration, TweenType curve, int id = 0, bool ignoreTimeScale = true)
    {
        fadeInEvent.ID = id;
        fadeInEvent.Duration = duration;
        fadeInEvent.Curve = curve;
        fadeInEvent.IgnoreTimeScale = ignoreTimeScale;
        EventManager.TriggerEvent(fadeInEvent);
    }
}

public struct FadeOutEvent
{
    public int ID;
    public float Duration;
    public TweenType Curve;
    public bool IgnoreTimeScale;

    static FadeOutEvent fadeoutEvent;

    public static void Trigger(float duration, TweenType curve, int id = 0, bool ignoreTimeScale = true)
    {
        fadeoutEvent.ID = id;
        fadeoutEvent.Duration = duration;
        fadeoutEvent.Curve = curve;
        fadeoutEvent.IgnoreTimeScale = ignoreTimeScale;
        EventManager.TriggerEvent(fadeoutEvent);
    }

}


[RequireComponent(typeof(CanvasGroup))]
[RequireComponent(typeof(Image))]
public class FadeTool : MonoBehaviour, EventListener<FadeStartEvent>, EventListener<FadeStopEvent>, EventListener<FadeInEvent>, EventListener<FadeOutEvent>
{
    public enum ForcedInitStates
    {
        None, Active, Inactive
    }

    public int ID;

    [Header("Opacity")]
    public float InactiveAlpha = 0f;
    public float ActiveAlpha = 1f;
    public ForcedInitStates InitState = ForcedInitStates.Inactive;

    [Header("Timing")]
    public float DefaultDuration = 0.2f;
    public TweenType DefaultTween = new TweenType(Tween.TweenCurve.LinearTween);
    public bool IgnoreTimescale = true;

    [Header("Interaction")]
    public bool ShouldBlockRaycasts = false;

    protected CanvasGroup _canvasGroup;
    protected Image _image;

    protected float _initialAlpha;
    protected float _currentTargetAlpha;
    protected float _currentDuration;
    protected TweenType _currentCurve;

    protected bool _fading = false;
    protected float _fadeStartedAt;
    protected bool _frameCountOne;

    [Header("Debug")]
    [InspectorButton(nameof(FadeIn1Second))]
    public bool FadeIn1SecondButton;
    public void FadeIn1Second()
    {
        FadeInEvent.Trigger(1f,new TweenType(Tween.TweenCurve.LinearTween),ID);
    }

    [InspectorButton(nameof(FadeOut2Second))]
    public bool FadeOut1SecondButton;
    public void FadeOut2Second()
    {
        FadeOutEvent.Trigger(2f,new TweenType(Tween.TweenCurve.LinearTween),ID);
    }

    [InspectorButton(nameof(DefaultFade))]
    public bool DefaultFadeButton;
    public void DefaultFade()
    {
        if (InitState == ForcedInitStates.Inactive)
        {
            FadeStartEvent.Trigger(DefaultDuration, InactiveAlpha, DefaultTween, ID);
        }
        else if (InitState == ForcedInitStates.Active)
        {
            FadeStartEvent.Trigger(DefaultDuration, ActiveAlpha, DefaultTween, ID);
        }

    }

    [InspectorButton(nameof(ResetFade))]
    public bool ResetFaderButton;
    public void ResetFade()
    {
        _canvasGroup.alpha = InactiveAlpha;
    }

    [InspectorButton(nameof(StartFader))]
    public bool StartFaderButton;
    public void StartFader()
    {
        StartFading(InactiveAlpha, ActiveAlpha, DefaultDuration, DefaultTween, ID, IgnoreTimescale);
    }


    public void Awake()
    {
        Init();
    }

    protected virtual void OnEnable()
    {
        this.EventStartListening<FadeStartEvent>();
        this.EventStartListening<FadeStopEvent>();
        this.EventStartListening<FadeInEvent>();
        this.EventStartListening<FadeOutEvent>();
    }

    protected virtual void OnDisable()
    {
        this.EventStopListening<FadeStartEvent>();
        this.EventStopListening<FadeStopEvent>();
        this.EventStopListening<FadeInEvent>();
        this.EventStopListening<FadeOutEvent>();
    }

    private void Update()
    {
        if (_canvasGroup == null)
            return;
        if (_fading)
        {
            Fade();
        }
    }

    /// <summary>
    /// 初始化
    /// </summary>
    public void Init()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _image = GetComponent<Image>();

        if (InitState == ForcedInitStates.Inactive)
        {
            _canvasGroup.alpha = InactiveAlpha;
            _image.enabled = false;
        }
        else if (InitState == ForcedInitStates.Active)
        {
            _canvasGroup.alpha = ActiveAlpha;
            _image.enabled = true;
        }
    }

    /// <summary>
    /// 在update中调用的淡入方法
    /// </summary>
    protected virtual void Fade()
    {
        float currentTime = IgnoreTimescale ? Time.unscaledTime : Time.time;
        if (_frameCountOne)
        {
            if (Time.frameCount <= 2)
            {
                _canvasGroup.alpha = _initialAlpha;
                return;
            }
            _fadeStartedAt = IgnoreTimescale ? Time.unscaledTime : Time.time;
            currentTime = _fadeStartedAt;
            _frameCountOne = false;

        }

        float endTime = _fadeStartedAt + _currentDuration;
        if (currentTime - _fadeStartedAt < _currentDuration)
        {
            float result = Tween.TweenMethod(currentTime, _fadeStartedAt, endTime, _initialAlpha, _currentTargetAlpha, _currentCurve);
            _canvasGroup.alpha = result;
        }
        else
        {
            StopFading();
        }



    }

    /// <summary>
    /// 开始淡入
    /// </summary>
    /// <param name="initialAlpha"></param>
    /// <param name="endAlpha"></param>
    /// <param name="duration"></param>
    /// <param name="curve"></param>
    /// <param name="id"></param>
    /// <param name="ignoreTimeScale"></param>
    protected virtual void StartFading(float initialAlpha, float endAlpha, float duration, TweenType curve, int id, bool ignoreTimeScale)
    {
        if (id != ID)
        {
            return;
        }

        IgnoreTimescale = ignoreTimeScale;

        EnableFader();

        _fading = true;
        _initialAlpha = initialAlpha;
        _currentTargetAlpha = endAlpha;
        _fadeStartedAt = IgnoreTimescale ? Time.unscaledTime : Time.time;
        _currentCurve = curve;
        _currentDuration = duration;

        if (Time.frameCount == 1)
        {
            _frameCountOne = true;
        }

    }

    /// <summary>
    /// 开启淡入面板
    /// </summary>
    protected virtual void EnableFader()
    {
        _image.enabled = true;
        if (ShouldBlockRaycasts)
        {
            _canvasGroup.blocksRaycasts = true;
        }
    }

    /// <summary>
    /// 淡入结束
    /// </summary>
    protected virtual void StopFading()
    {
        _canvasGroup.alpha = _currentTargetAlpha;
        _fading = false;
        if (_canvasGroup.alpha == InactiveAlpha)
        {
            DisableFader();
        }
    }

    /// <summary>
    /// 关闭淡入面板
    /// </summary>
    protected virtual void DisableFader()
    {
        _image.enabled = false;
        if (ShouldBlockRaycasts)
        {
            _canvasGroup.blocksRaycasts = false;
        }
    }

    public void OnEvent(FadeStartEvent fadeStartEvent)
    {
        _currentTargetAlpha = (fadeStartEvent.TargetAlpha == -1) ? ActiveAlpha : fadeStartEvent.TargetAlpha;
        StartFading(_canvasGroup.alpha, _currentTargetAlpha, fadeStartEvent.Duration, fadeStartEvent.Curve, fadeStartEvent.ID, fadeStartEvent.IgnoreTimeScale);
    }
    public void OnEvent(FadeStopEvent fadeStopEvent)
    {
        if (fadeStopEvent.ID == ID)
        {
            _fading = false;
        }
    }
    public void OnEvent(FadeInEvent fadeInEvent)
    {
        StartFading(InactiveAlpha, ActiveAlpha, fadeInEvent.Duration, fadeInEvent.Curve, fadeInEvent.ID, fadeInEvent.IgnoreTimeScale);
    }
    public void OnEvent(FadeOutEvent fadeOutEvent)
    {
        StartFading(ActiveAlpha, InactiveAlpha, fadeOutEvent.Duration, fadeOutEvent.Curve, fadeOutEvent.ID, fadeOutEvent.IgnoreTimeScale);
    }

}
