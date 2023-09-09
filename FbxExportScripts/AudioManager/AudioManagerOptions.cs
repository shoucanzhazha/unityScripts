using System;
using UnityEngine;
using UnityEngine.Audio;

[Serializable]
public class AudioManagerOptions
{

    /// <summary>
    /// 音频播放的轨道
    /// </summary>
    public SoundTracks Track;
    /// <summary>
    /// 播放音频的位置
    /// </summary>
    public Vector3 Location;
    /// <summary>
    /// 是否循环播放音频
    /// </summary>
    public bool Loop;
    /// <summary>
    /// 播放音频的音量
    /// </summary>
    public float Volume;
    /// <summary>
    /// 音频的ID，可用于稍后查找该音频
    /// </summary>
    public int ID;
    /// <summary>
    /// 播放音频时是否淡入淡出
    /// </summary>
    public bool Fade;
    /// <summary>
    /// 淡入淡出前的初始音量
    /// </summary>
    public float FadeInitialVolume;
    /// <summary>
    /// 淡入淡出的持续时间（秒）
    /// </summary>
    public float FadeDuration;
    /// <summary>
    /// 在淡入淡出时使用的缓和类型
    /// </summary>
    public TweenType FadeTween;
    /// <summary>
    /// 音频是否在场景切换后保持播放
    /// </summary>
    public bool Persistent;
    /// <summary>
    /// 如果不想从池中选择一个，则使用的 AudioSource
    /// </summary>
    public AudioSource RecycleAudioSource;
    /// <summary>
    /// 如果不想在任何预设轨道上播放，则使用的音频组
    /// </summary>
    public AudioMixerGroup AudioGroup;
    /// <summary>
    /// 音频的音调
    /// </summary>
    public float Pitch;
    /// <summary>
    /// 以立体声方式将播放中的声音定位在左侧或右侧
    /// </summary>
    public float PanStereo;
    /// <summary>
    /// 设置此 AudioSource 受 3D 空间化计算（衰减、多普勒等）的影响程度。0.0 使声音完全 2D，1.0 使其完全 3D。
    /// </summary>
    public float SpatialBlend;
    /// <summary>
    /// 在其目标轨道上以独奏模式播放此声音，如果是，则此声音开始播放时，该轨道上的所有其他声音都将被静音
    /// </summary>
    public bool SoloSingleTrack;
    /// <summary>
    /// 在所有其他轨道上以独奏模式播放此声音，如果是，则此声音开始播放时，所有其他轨道都将被静音
    /// </summary>
    public bool SoloAllTracks;
    /// <summary>
    /// 在任何独奏模式下，AutoUnSoloOnEnd 会在声音停止播放后自动取消静音轨道
    /// </summary>
    public bool AutoUnSoloOnEnd;
    /// <summary>
    /// 是否绕过效果（从滤波器组件或全局监听器滤波器应用）
    /// </summary>
    public bool BypassEffects;
    /// <summary>
    /// 当设置时，不会将 AudioSource 生成的音频信号路由到与混响区域相关联的全局混响
    /// </summary>
    public bool BypassListenerEffects;
    /// <summary>
    /// 当设置时，不会将来自 AudioSource 的信号路由到与混响区域相关联的全局混响
    /// </summary>
    public bool BypassReverbZones;
    /// <summary>
    /// 设置 AudioSource 的优先级
    /// </summary>
    public int Priority;
    /// <summary>
    /// 该 AudioSource 的信号将混合到与混响区域关联的全局混响中的数量
    /// </summary>
    public float ReverbZoneMix;
    /// <summary>
    /// 设置此 AudioSource 的多普勒缩放
    /// </summary>
    public float DopplerLevel;
    /// <summary>
    /// 设置在扬声器空义中的 3D 立体声声音中的扩散角度（以度为单位）
    /// </summary>
    public int Spread;
    /// <summary>
    /// 设置/获取 AudioSource 在距离上的衰减方式
    /// </summary>
    public AudioRolloffMode RolloffMode;
    /// <summary>
    /// 在最小距离内，AudioSource 将停止变大音量
    /// </summary>
    public float MinDistance;
    /// <summary>
    /// （对数衰减）MaxDistance 是声音停止衰减的距离。
    /// </summary>
    public float MaxDistance;


    public static AudioManagerOptions Default
    {
        get
        {
            AudioManagerOptions defaultOptions = new AudioManagerOptions();
            defaultOptions.Track = SoundTracks.Sfx;
            defaultOptions.Location = Vector3.zero;
            defaultOptions.Loop = false;
            defaultOptions.Volume = 1.0f;
            defaultOptions.ID = 0;
            defaultOptions.Fade = false;
            defaultOptions.FadeInitialVolume = 0f;
            defaultOptions.FadeDuration = 1f;
            defaultOptions.FadeTween = null;
            defaultOptions.Persistent = false;
            defaultOptions.RecycleAudioSource = null;
            defaultOptions.AudioGroup = null;
            defaultOptions.Pitch = 1f;
            defaultOptions.PanStereo = 0f;
            defaultOptions.SpatialBlend = 0.0f;
            defaultOptions.SoloSingleTrack = false;
            defaultOptions.SoloAllTracks = false;
            defaultOptions.AutoUnSoloOnEnd = false;
            defaultOptions.BypassEffects = false;
            defaultOptions.BypassListenerEffects = false;
            defaultOptions.BypassReverbZones = false;
            defaultOptions.Priority = 128;
            defaultOptions.ReverbZoneMix = 1f;
            defaultOptions.DopplerLevel = 1f;
            defaultOptions.Spread = 0;
            defaultOptions.RolloffMode = AudioRolloffMode.Logarithmic;
            defaultOptions.MinDistance = 1f;
            defaultOptions.MaxDistance = 500f;
            return defaultOptions;
        }
    }


}
