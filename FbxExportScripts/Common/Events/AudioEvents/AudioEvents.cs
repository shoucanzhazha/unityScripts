using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum TrackEventTypes
{
    MuteTrack,
    UnmuteTrack,
    SetVolumeTrack,
    PlayTrack,
    PauseTrack,
    StopTrack,
    FreeTrack
}
/// <summary>
/// 控制声音的音轨的事件
/// </summary>
public struct TrackEvent
{
    public TrackEventTypes TrackEventType;
    public SoundTracks Track;
    public float Volume;

    public TrackEvent(TrackEventTypes trackEventType, SoundTracks track = SoundTracks.BGM, float volume = 1f)
    {
        TrackEventType = trackEventType;
        Track = track;
        Volume = volume;
    }

    static TrackEvent trackEvent;

    public static void Trigger(TrackEventTypes trackEventType, SoundTracks track = SoundTracks.BGM, float volume = 1f)
    {
        trackEvent.TrackEventType = trackEventType;
        trackEvent.Track = track;
        trackEvent.Volume = volume;
        EventManager.TriggerEvent(trackEvent);
    }

}

/// <summary>
/// 控制音轨淡入淡出的事件
/// </summary>
public struct TrackFadeEvent
{
    public SoundTracks Track;
    public float FadeDuration;
    public float FinalVolume;
    public TweenType FadeTween;

    public TrackFadeEvent(SoundTracks track, float fadeDuration, float finalVolume, TweenType fadeTween)
    {
        Track = track;
        FadeDuration = fadeDuration;
        FinalVolume = finalVolume;
        FadeTween = fadeTween;
    }

    static TrackFadeEvent trackFadeEvent;
    public static void Trigger(SoundTracks track, float fadeDuration, float finalVolume, TweenType fadeTween)
    {
        trackFadeEvent.Track = track;
        trackFadeEvent.FadeDuration = fadeDuration;
        trackFadeEvent.FinalVolume = finalVolume;
        trackFadeEvent.FadeTween = fadeTween;
        EventManager.TriggerEvent(trackFadeEvent);
    }
}


public enum SettingsEventTypes
{
    SaveSettings,
    LoadSettings,
    ResetSettings
}
/// <summary>
/// 获取配置文件的事件
/// </summary>
public struct SettingsEvent
{
    public SettingsEventTypes EventType;

    public SettingsEvent(SettingsEventTypes eventType)
    {
        EventType = eventType;
    }

    static SettingsEvent settingEvent;
    public static void Trigger(SettingsEventTypes eventType)
    {
        settingEvent.EventType = eventType;
        EventManager.TriggerEvent(settingEvent);
    }
}


public enum SoundControlEventTypes
{
    Pause,
    Resume,
    Stop,
    Free
}
/// <summary>
/// 控制音频的事件
/// </summary>
public struct SoundControlEvent
{
    public int SoundID;
    public SoundControlEventTypes SoundControlEventType;
    public AudioSource TargetSource;
    public SoundControlEvent(SoundControlEventTypes eventType, int soundID, AudioSource source = null)
    {
        SoundID = soundID;
        TargetSource = source;
        SoundControlEventType = eventType;
    }

    static SoundControlEvent soundControlEvent;
    public static void Trigger(SoundControlEventTypes eventType, int soundID, AudioSource source = null)
    {
        soundControlEvent.SoundID = soundID;
        soundControlEvent.TargetSource = source;
        soundControlEvent.SoundControlEventType = eventType;
        EventManager.TriggerEvent(soundControlEvent);
    }
}


public enum AllSoundsControlEventTypes
{
    Pause, Play, Stop, Free, FreeAllButPersistent, FreeAllLooping
}
/// <summary>
/// 控制所有音频的事件
/// </summary>
public struct AllSoundsControlEvent
{
    public AllSoundsControlEventTypes EventType;

    public AllSoundsControlEvent(AllSoundsControlEventTypes eventType)
    {
        EventType = eventType;
    }

    static AllSoundsControlEvent allSoundsControlEvent;
    public static void Trigger(AllSoundsControlEventTypes eventType)
    {
        allSoundsControlEvent.EventType = eventType;
        EventManager.TriggerEvent(allSoundsControlEvent);
    }
}

/// <summary>
/// 控制音频淡入淡出的事件
/// </summary>
public struct SoundFadeEvent
{
    public int SoundID;
    public float FadeDuration;
    public float FinalVolume;
    public TweenType FadeTween;

    public SoundFadeEvent(int soundID, float fadeDuration, float finalVolume, TweenType fadeTween)
    {
        SoundID = soundID;
        FadeDuration = fadeDuration;
        FinalVolume = finalVolume;
        FadeTween = fadeTween;
    }

    static SoundFadeEvent soundFadeEvent;
    public static void Trigger(int soundID, float fadeDuration, float finalVolume, TweenType fadeTween)
    {
        soundFadeEvent.SoundID = soundID;
        soundFadeEvent.FadeDuration = fadeDuration;
        soundFadeEvent.FinalVolume = finalVolume;
        soundFadeEvent.FadeTween = fadeTween;
        EventManager.TriggerEvent(soundFadeEvent);
    }
}
