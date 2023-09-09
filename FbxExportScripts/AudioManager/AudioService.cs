using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LogUtils;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

/// <summary>
/// 音轨类型枚举
/// </summary>
public enum SoundTracks
{
    Master, BGM, Sfx, UI, Other
}


public class AudioService : MonoBehaviour,
                            EventListener<TrackEvent>,
                            EventListener<SettingsEvent>,
                            EventListener<SoundControlEvent>,
                            EventListener<SoundFadeEvent>,
                            EventListener<AllSoundsControlEvent>,
                            EventListener<TrackFadeEvent>
{

    private static AudioService instance;
    public static AudioService Instance { get { return instance; } }


    [Header("Settings")]
    public AudioSettingsSO settingsSO;

    [Header("Pool")]
    public int AudioSourcePoolSize = 10;
    public bool PoolCanExpand = true;

    protected AudioSourcePool pool;
    protected List<SoundInfo> _sounds;
    protected SoundInfo _soundInfo;
    protected GameObject _tempAudioSourceObj;
    protected List<AudioSource> _tempAudioSourceList;
    protected AudioSource _tempAudioSource;


    protected virtual void OnEnable()
    {
        //SfxEvent.Register(OnSfxEvent);
        AudioPlayEvent.Register(OnAudioPlayEvent);
        this.EventStartListening<SettingsEvent>();
        this.EventStartListening<TrackEvent>();
        this.EventStartListening<SoundControlEvent>();
        this.EventStartListening<TrackFadeEvent>();
        this.EventStartListening<SoundFadeEvent>();
        this.EventStartListening<AllSoundsControlEvent>();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    protected virtual void OnDisable()
    {
        //SfxEvent.Unregister(OnSfxEvent);
        AudioPlayEvent.Unregister(OnAudioPlayEvent);
        this.EventStopListening<SettingsEvent>();
        this.EventStopListening<TrackEvent>();
        this.EventStopListening<SoundControlEvent>();
        this.EventStopListening<TrackFadeEvent>();
        this.EventStopListening<SoundFadeEvent>();
        this.EventStopListening<AllSoundsControlEvent>();
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }




    /// <summary>
    /// 初始化
    /// </summary>
    public void InitAudioService()
    {
        instance = this;

        if (pool == null)
        {
            pool = new AudioSourcePool();
        }
        _sounds = new List<SoundInfo>();
        pool.FillPool(AudioSourcePoolSize);

        if (settingsSO != null && settingsSO.Settings.AutoLoad)
        {
            settingsSO.LoadAudioSettings();
        }

        this.Log("Init AudioService Done.");
    }






    #region PlaySound

    public virtual AudioSource PlaySound(AudioClip audioClip, AudioManagerOptions option)
    {
        return PlaySound(audioClip, option.Track, option.Location,
                            option.Loop, option.Volume, option.ID,
                            option.Fade, option.FadeInitialVolume, option.FadeDuration, option.FadeTween,
                            option.Persistent,
                            option.RecycleAudioSource, option.AudioGroup,
                            option.Pitch, option.PanStereo, option.SpatialBlend,
                            option.SoloSingleTrack, option.SoloAllTracks, option.AutoUnSoloOnEnd,
                            option.BypassEffects, option.BypassListenerEffects, option.BypassReverbZones, option.Priority,
                            option.ReverbZoneMix,
                            option.DopplerLevel, option.Spread, option.RolloffMode, option.MinDistance, option.MaxDistance
                        );
    }

    public virtual AudioSource PlaySound(AudioClip audioClip, SoundTracks track, Vector3 location,
                                        bool loop = false, float volume = 1.0f, int ID = 0,
                                        bool fade = false, float fadeInitialVolume = 0f, float fadeDuration = 1f, TweenType fadeTween = null,
                                        bool persistent = false,
                                        AudioSource recycleAudioSource = null, AudioMixerGroup audioGroup = null,
                                        float pitch = 1f, float panStereo = 0f, float spatialBlend = 0.0f,
                                        bool soloSingleTrack = false, bool soloAllTracks = false, bool autoUnSoloOnEnd = false,
                                        bool bypassEffects = false, bool bypassListenerEffects = false, bool bypassReverbZones = false, int priority = 128, float reverbZoneMix = 1f,
                                        float dopplerLevel = 1f, int spread = 0, AudioRolloffMode rolloffMode = AudioRolloffMode.Logarithmic, float minDistance = 1f, float maxDistance = 500f
                                        )
    {
        if (!audioClip) return null;
        AudioSource audioSource = recycleAudioSource;
        if (audioSource == null)
        {
            audioSource = pool.GetAvailableAudioSource(PoolCanExpand);
            if (audioSource != null && !loop)
            {
                recycleAudioSource = audioSource;
                StartCoroutine(pool.AutoDisableAudioSource(audioClip.length / Mathf.Abs(pitch), audioSource, audioClip));
            }
        }
        if (audioSource == null)
        {
            _tempAudioSourceObj = new GameObject("TempAudioSource_" + audioClip.name);
            //SceneManager.MoveGameObjectToScene(_tempAudioSourceObj, this.gameObject.scene);
            _tempAudioSourceObj.transform.SetParent(ObjectPoolManager._audioSourceEmpty.transform);
            audioSource = _tempAudioSourceObj.AddComponent<AudioSource>();
        }

        //获取AudioSource的设置
        audioSource.transform.position = location;
        audioSource.time = 0f;
        audioSource.clip = audioClip;
        audioSource.pitch = pitch;
        audioSource.spatialBlend = spatialBlend;
        audioSource.panStereo = panStereo;
        audioSource.loop = loop;
        audioSource.bypassEffects = bypassEffects;
        audioSource.bypassListenerEffects = bypassListenerEffects;
        audioSource.bypassReverbZones = bypassReverbZones;
        audioSource.priority = priority;
        audioSource.reverbZoneMix = reverbZoneMix;
        audioSource.dopplerLevel = dopplerLevel;
        audioSource.spread = spread;
        audioSource.rolloffMode = rolloffMode;
        audioSource.minDistance = minDistance;
        audioSource.maxDistance = maxDistance;

        //设置音轨和音量
        if (settingsSO != null)
        {
            audioSource.outputAudioMixerGroup = settingsSO.MasterAMG;
            switch (track)
            {
                case SoundTracks.Master:
                    audioSource.outputAudioMixerGroup = settingsSO.MasterAMG;
                    break;
                case SoundTracks.BGM:
                    audioSource.outputAudioMixerGroup = settingsSO.BGMAMG;
                    break;
                case SoundTracks.Sfx:
                    audioSource.outputAudioMixerGroup = settingsSO.SfxAMG;
                    break;
                case SoundTracks.UI:
                    audioSource.outputAudioMixerGroup = settingsSO.UIAMG;
                    break;
                case SoundTracks.Other:
                    break;
            }
        }

        if (audioGroup)
        {
            audioSource.outputAudioMixerGroup = audioGroup;
        }
        audioSource.volume = volume;
        audioSource.Play();

        //根据设置对其他音轨和自身进行更多设置
        if (!loop && !recycleAudioSource)
        {
            Destroy(_tempAudioSourceObj, audioClip.length);
        }
        if (fade)
        {
            FadeSound(audioSource, fadeDuration, fadeInitialVolume, volume, fadeTween);
        }
        if (soloSingleTrack)
        {
            MuteSoundsOnTrack(track, true, 0f);
            audioSource.mute = false;
            if (autoUnSoloOnEnd)
            {
                MuteSoundsOnTrack(track, false, audioClip.length);
            }
        }
        else if (soloAllTracks)
        {
            MuteAllSounds();
            audioSource.mute = false;
            if (autoUnSoloOnEnd)
            {
                StartCoroutine(MuteAllSoundsCoroutine(audioClip.length, false));
            }
        }

        //获取soundInfo
        _soundInfo.ID = ID;
        _soundInfo.Track = track;
        _soundInfo.Source = audioSource;
        _soundInfo.Persistent = persistent;

        bool alreadyIn = false;
        for (int i = 0; i < _sounds.Count; i++)
        {
            if (_sounds[i].Source == audioSource)
            {
                _sounds[i] = _soundInfo;
                alreadyIn = true;
            }
        }
        if (!alreadyIn)
        {
            _sounds.Add(_soundInfo);
        }

        return audioSource;
    }

    #endregion






    #region Fades

    public virtual void FadeTrack(SoundTracks track, float duration, float initialVolume = 0f, float finalVolume = 1f, TweenType tweenType = null)
    {
        StartCoroutine(FadeTrackCoroutine(track, duration, initialVolume, finalVolume, tweenType));
    }

    public virtual void FadeSound(AudioSource source, float duration, float initialVolume, float finalVolume, TweenType tweenType)
    {
        StartCoroutine(FadeCoroutine(source, duration, initialVolume, finalVolume, tweenType));
    }

    protected virtual IEnumerator FadeTrackCoroutine(SoundTracks track, float duration, float initialVolume, float finalVolume, TweenType tweenType)
    {
        float startedAt = Time.unscaledTime;
        if (tweenType == null)
        {
            tweenType = new TweenType(Tween.TweenCurve.EaseInOutQuartic);
        }
        while (Time.unscaledTime - startedAt <= duration)
        {
            float elapsedTime = Time.unscaledTime - startedAt;
            float newVolume = Tween.TweenMethod(elapsedTime, 0f, duration, initialVolume, finalVolume, tweenType);
            settingsSO.SetTrackVolume(track, newVolume);
            yield return null;
        }
        settingsSO.SetTrackVolume(track, finalVolume);
    }

    protected virtual IEnumerator FadeCoroutine(AudioSource source, float duration, float initialVolume, float finalVolume, TweenType tweenType)
    {
        float startedAt = Time.unscaledTime;
        if (tweenType == null)
        {
            tweenType = new TweenType(Tween.TweenCurve.EaseInOutQuartic);
        }
        while (Time.unscaledTime - startedAt <= duration)
        {
            float elapsedTime = Time.unscaledTime - startedAt;
            float newVolume = Tween.TweenMethod(elapsedTime, 0f, duration, initialVolume, finalVolume, tweenType);
            source.volume = newVolume;
            yield return null;
        }
        source.volume = finalVolume;

    }

    #endregion

    #region Solo
    public virtual void MuteSoundsOnTrack(SoundTracks track, bool mute, float delay = 0f)
    {
        StartCoroutine(MuteSoundsOnTrackCoroutine(track, mute, delay));
    }

    public virtual void MuteAllSounds(bool mute = true)
    {
        StartCoroutine(MuteAllSoundsCoroutine(0f, mute));
    }

    protected virtual IEnumerator MuteSoundsOnTrackCoroutine(SoundTracks track, bool mute, float delay)
    {
        if (delay > 0)
        {
            yield return CoroutineMethods.WaitForUnscaled(delay);
        }
        foreach (SoundInfo sound in _sounds)
        {
            if (sound.Track == track)
            {
                sound.Source.mute = mute;
            }
        }

    }

    protected virtual IEnumerator MuteAllSoundsCoroutine(float delay, bool mute = true)
    {
        if (delay > 0)
        {
            yield return CoroutineMethods.WaitForUnscaled(delay);
        }
        foreach (SoundInfo sound in _sounds)
        {
            sound.Source.mute = mute;
        }
    }

    #endregion

    #region Find

    public virtual List<AudioSource> FindByID(int ID)
    {
        List<AudioSource> result = new List<AudioSource>();
        foreach (SoundInfo sound in _sounds)
        {
            if (sound.ID == ID)
            {
                result.Add(sound.Source);
            }
        }

        return result;
    }

    public virtual List<AudioSource> FindByClip(AudioClip clip)
    {
        List<AudioSource> result = new List<AudioSource>();
        foreach (SoundInfo sound in _sounds)
        {
            if (sound.Source.clip == clip)
            {
                result.Add(sound.Source);
            }
        }

        return result;
    }

    #endregion

    #region TrackControls

    public virtual void MuteTrack(SoundTracks track)
    {
        ControlTrack(track, ControlTrackModes.Mute, 0f);
    }

    public virtual void UnmuteTrack(SoundTracks track)
    {
        ControlTrack(track, ControlTrackModes.Unmute, 0f);
    }

    public virtual void SetTrackVolume(SoundTracks track, float volume)
    {
        ControlTrack(track, ControlTrackModes.SetVolume, volume);
    }

    public virtual float GetTrackVolume(SoundTracks track, bool mutedVolume)
    {
        switch (track)
        {
            case SoundTracks.Master:
                if (mutedVolume)
                {
                    return settingsSO.Settings.MutedMasterVolume;
                }
                else
                {
                    return settingsSO.Settings.MasterVolume;
                }
            case SoundTracks.BGM:
                if (mutedVolume)
                {
                    return settingsSO.Settings.MutedBGMVolume;
                }
                else
                {
                    return settingsSO.Settings.BGMVolume;
                }
            case SoundTracks.Sfx:
                if (mutedVolume)
                {
                    return settingsSO.Settings.MutedSfxVolume;
                }
                else
                {
                    return settingsSO.Settings.SfxVolume;
                }
            case SoundTracks.UI:
                if (mutedVolume)
                {
                    return settingsSO.Settings.MutedUIVolume;
                }
                else
                {
                    return settingsSO.Settings.UIVolume;
                }
        }

        return 1f;
    }

    public virtual void PauseTrack(SoundTracks track)
    {
        foreach (SoundInfo sound in _sounds)
        {
            if (sound.Track == track)
            {
                sound.Source.Pause();
            }
        }
    }

    public virtual void PlayTrack(SoundTracks track)
    {
        foreach (SoundInfo sound in _sounds)
        {
            if (sound.Track == track)
            {
                sound.Source.Play();
            }
        }
    }

    public virtual void StopTrack(SoundTracks track)
    {
        foreach (SoundInfo sound in _sounds)
        {
            if (sound.Track == track)
            {
                sound.Source.Stop();
            }
        }
    }

    public virtual void FreeTrack(SoundTracks track)
    {
        foreach (SoundInfo sound in _sounds)
        {
            if (sound.Track == track)
            {
                sound.Source.Stop();
                sound.Source.gameObject.SetActive(false);
            }
        }
    }

    public virtual void MuteBGM() { MuteTrack(SoundTracks.BGM); }

    public virtual void UnmuteBGM() { UnmuteTrack(SoundTracks.BGM); }

    public virtual void MuteSfx() { MuteTrack(SoundTracks.Sfx); }

    public virtual void UnmuteSfx() { UnmuteTrack(SoundTracks.Sfx); }

    public virtual void MuteUI() { MuteTrack(SoundTracks.UI); }

    public virtual void UnmuteUI() { UnmuteTrack(SoundTracks.UI); }

    public virtual void MuteMaster() { MuteTrack(SoundTracks.Master); }

    public virtual void UnmuteMaster() { UnmuteTrack(SoundTracks.Master); }

    public virtual void SetVolumeBGM(float newVolume) { SetTrackVolume(SoundTracks.BGM, newVolume); }

    public virtual void SetVolumeSfx(float newVolume) { SetTrackVolume(SoundTracks.Sfx, newVolume); }

    public virtual void SetVolumeUI(float newVolume) { SetTrackVolume(SoundTracks.UI, newVolume); }

    public virtual void SetVolumeMaster(float newVolume) { SetTrackVolume(SoundTracks.Master, newVolume); }

    public enum ControlTrackModes { Mute, Unmute, SetVolume }
    protected virtual void ControlTrack(SoundTracks track, ControlTrackModes trackMode, float volume = 0.5f)
    {
        string target = "";
        float savedVolume = 0f;
        switch (track)
        {
            case SoundTracks.Master:
                target = settingsSO.Settings.MasterVolumeParameter;
                if (trackMode == ControlTrackModes.Mute) { settingsSO.TargetAudioMixer.GetFloat(target, out settingsSO.Settings.MutedMasterVolume); settingsSO.Settings.MasterOn = false; }
                else if (trackMode == ControlTrackModes.Unmute) { savedVolume = settingsSO.Settings.MutedMasterVolume; settingsSO.Settings.MasterOn = true; }
                break;
            case SoundTracks.BGM:
                target = settingsSO.Settings.BGMVolumeParameter;
                if (trackMode == ControlTrackModes.Mute)
                {
                    settingsSO.TargetAudioMixer.GetFloat(target, out settingsSO.Settings.MutedBGMVolume);
                    settingsSO.Settings.BGMOn = false;
                }
                else if (trackMode == ControlTrackModes.Unmute)
                {
                    savedVolume = settingsSO.Settings.MutedBGMVolume;
                    settingsSO.Settings.BGMOn = true;
                }
                break;
            case SoundTracks.Sfx:
                target = settingsSO.Settings.SfxVolumeParameter;
                if (trackMode == ControlTrackModes.Mute) { settingsSO.TargetAudioMixer.GetFloat(target, out settingsSO.Settings.MutedSfxVolume); settingsSO.Settings.SfxOn = false; }
                else if (trackMode == ControlTrackModes.Unmute) { savedVolume = settingsSO.Settings.MutedSfxVolume; settingsSO.Settings.SfxOn = true; }
                break;
            case SoundTracks.UI:
                target = settingsSO.Settings.UIVolumeParameter;
                if (trackMode == ControlTrackModes.Mute) { settingsSO.TargetAudioMixer.GetFloat(target, out settingsSO.Settings.MutedUIVolume); settingsSO.Settings.UIOn = false; }
                else if (trackMode == ControlTrackModes.Unmute) { savedVolume = settingsSO.Settings.MutedUIVolume; settingsSO.Settings.UIOn = true; }
                break;
        }
        switch (trackMode)
        {
            case ControlTrackModes.Mute:
                settingsSO.SetTrackVolume(track, 0f);
                break;
            case ControlTrackModes.Unmute:
                settingsSO.SetTrackVolume(track, settingsSO.MixerVolumeToNormalized(savedVolume));
                break;
            case ControlTrackModes.SetVolume:
                settingsSO.SetTrackVolume(track, volume);
                break;
        }
        settingsSO.GetTrackVolumes();
        if (settingsSO.Settings.AutoSave)
        {
            settingsSO.SaveAudioSettings();
        }
    }

    #endregion

    #region SoundControls

    public virtual void PauseSound(AudioSource source)
    {
        source.Pause();
    }

    public virtual void ResumeSound(AudioSource source)
    {
        source.Play();
    }

    public virtual void StopSound(AudioSource source)
    {
        source.Stop();
    }

    public virtual void FreeSound(AudioSource source)
    {
        source.Stop();
        if (!pool.FreeSound(source))
        {
            Destroy(source.gameObject);
        }
    }

    #endregion

    #region AllSoundsControls

    public virtual void PauseAllSounds()
    {
        foreach (SoundInfo sound in _sounds)
        {
            sound.Source.Pause();
        }
    }

    public virtual void PlayAllSounds()
    {
        foreach (SoundInfo sound in _sounds)
        {
            sound.Source.Play();
        }
    }

    public virtual void StopAllSounds()
    {
        foreach (SoundInfo sound in _sounds)
        {
            sound.Source.Stop();
        }
    }

    public virtual void FreeAllSounds()
    {
        foreach (SoundInfo sound in _sounds)
        {
            if (sound.Source != null)
            {
                FreeSound(sound.Source);
            }
        }
    }

    public virtual void FreeAllSoundsButPersistent()
    {
        if (_sounds == null)
            return;

        foreach (SoundInfo sound in _sounds)
        {
            if (sound.Source != null && !sound.Persistent)
            {
                FreeSound(sound.Source);
            }
        }
    }

    public virtual void FreeAllLoopingSounds()
    {
        foreach (SoundInfo sound in _sounds)
        {
            if ((sound.Source.loop) && (sound.Source != null))
            {
                FreeSound(sound.Source);
            }
        }
    }

    #endregion

    #region Events

    protected virtual void OnSceneLoaded(Scene arg0, LoadSceneMode loadSceneMode)
    {
        FreeAllSoundsButPersistent();
    }

    public void OnEvent(TrackEvent trackEvent)
    {
        switch (trackEvent.TrackEventType)
        {
            case TrackEventTypes.MuteTrack:
                MuteTrack(trackEvent.Track);
                break;
            case TrackEventTypes.UnmuteTrack:
                UnmuteTrack(trackEvent.Track);
                break;
            case TrackEventTypes.SetVolumeTrack:
                SetTrackVolume(trackEvent.Track, trackEvent.Volume);
                break;
            case TrackEventTypes.PlayTrack:
                PlayTrack(trackEvent.Track);
                break;
            case TrackEventTypes.PauseTrack:
                PauseTrack(trackEvent.Track);
                break;
            case TrackEventTypes.StopTrack:
                StopTrack(trackEvent.Track);
                break;
            case TrackEventTypes.FreeTrack:
                FreeTrack(trackEvent.Track);
                break;
        }

    }

    public void OnEvent(SettingsEvent settingsEvent)
    {
        switch (settingsEvent.EventType)
        {
            case SettingsEventTypes.SaveSettings:
                SaveSettings();
                break;
            case SettingsEventTypes.LoadSettings:
                settingsSO.LoadAudioSettings();
                break;
            case SettingsEventTypes.ResetSettings:
                settingsSO.ResetAudioSettings();
                break;
        }
    }

    public virtual void SaveSettings()
    {
        settingsSO.SaveAudioSettings();
    }

    public virtual void LoadSettings()
    {
        settingsSO.LoadAudioSettings();
    }

    public virtual void ResetSettings()
    {
        settingsSO.ResetAudioSettings();
    }

    public void OnEvent(SoundControlEvent soundControlEvent)
    {
        if (soundControlEvent.TargetSource == null)
        {
            _tempAudioSourceList = FindByID(soundControlEvent.SoundID);
        }
        else
        {
            _tempAudioSourceList.Add(soundControlEvent.TargetSource);
        }

        if (_tempAudioSourceList.Count == 0)
            return;

        foreach (AudioSource audioSource in _tempAudioSourceList)
        {
            if (audioSource != null)
            {
                switch (soundControlEvent.SoundControlEventType)
                {
                    case SoundControlEventTypes.Pause:
                        PauseSound(audioSource);
                        break;
                    case SoundControlEventTypes.Resume:
                        ResumeSound(audioSource);
                        break;
                    case SoundControlEventTypes.Stop:
                        StopSound(audioSource);
                        break;
                    case SoundControlEventTypes.Free:
                        FreeSound(audioSource);
                        break;
                }
            }
        }

    }

    public void OnEvent(TrackFadeEvent trackFadeEvent)
    {
        FadeTrack(trackFadeEvent.Track, trackFadeEvent.FadeDuration, settingsSO.GetTrackVolume(trackFadeEvent.Track), trackFadeEvent.FinalVolume, trackFadeEvent.FadeTween);
    }

    public void OnEvent(SoundFadeEvent soundFadeEvent)
    {
        _tempAudioSourceList = FindByID(soundFadeEvent.SoundID);

        foreach(AudioSource audioSource in _tempAudioSourceList)
        {
            if(audioSource != null)
            {
                FadeSound(audioSource, soundFadeEvent.FadeDuration, audioSource.volume, soundFadeEvent.FinalVolume,soundFadeEvent.FadeTween);
            }
        }
    }

    public void OnEvent(AllSoundsControlEvent allSoundsControlEvent)
    {
        switch (allSoundsControlEvent.EventType)
        {
            case AllSoundsControlEventTypes.Pause:
                PauseAllSounds();
                break;
            case AllSoundsControlEventTypes.Play:
                PlayAllSounds();
                break;
            case AllSoundsControlEventTypes.Stop:
                StopAllSounds();
                break;
            case AllSoundsControlEventTypes.Free:
                FreeAllSounds();
                break;
            case AllSoundsControlEventTypes.FreeAllButPersistent:
                FreeAllSoundsButPersistent();
                break;
            case AllSoundsControlEventTypes.FreeAllLooping:
                FreeAllLoopingSounds();
                break;
        }
    }

    private void OnMMSfxEvent(AudioClip clipToPlay, AudioMixerGroup audioGroup, float volume, float pitch)
    {
        AudioManagerOptions options = AudioManagerOptions.Default;
        options.Location = this.transform.position;
        options.AudioGroup = audioGroup;
        options.Volume = volume;
        options.Pitch = pitch;
        options.Track = SoundTracks.Sfx;
        options.Loop = false;
        PlaySound(clipToPlay, options);
    }

    public virtual AudioSource OnAudioPlayEvent(AudioClip clip, AudioManagerOptions options)
    {
        return PlaySound(clip, options);
    }

    #endregion




}
