using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioTest : MonoBehaviour
{

    #region AudioPlayEvent

    [Header("AudioPlayEvent")]
    public AudioClip clipLoop;
    public SoundTracks track;
    public AudioMixerGroup otherAMG;

    [InspectorButton(nameof(ClipLoop))]
    public bool ClipLoopButton;
    public virtual void ClipLoop()
    {
        AudioManagerOptions option = AudioManagerOptions.Default;
        option.Loop = true;
        option.Location = Vector3.zero;
        option.Track = track;
        AudioPlayEvent.Trigger(clipLoop, option);
    }

    [InspectorButton(nameof(ClipLoop1))]
    public bool ClipLoopButton1;
    public virtual void ClipLoop1()
    {
        AudioManagerOptions option = AudioManagerOptions.Default;
        option.Loop = true;
        option.Location = Vector3.zero;
        option.Track = track;
        option.ID = 1;
        AudioPlayEvent.Trigger(clipLoop, option);
    }

    [InspectorButton(nameof(ClipLoop2))]
    public bool ClipLoopButton2;
    public virtual void ClipLoop2()
    {
        AudioManagerOptions option = AudioManagerOptions.Default;
        option.Loop = false;
        option.Location = Vector3.zero;
        option.Track = track;
        AudioPlayEvent.Trigger(clipLoop, option);
    }


    public AudioClip clipNoLoop;
    [InspectorButton(nameof(ClipNoLoop))]
    public bool ClipNoLoopButton;
    public virtual void ClipNoLoop()
    {
        AudioManagerOptions option = AudioManagerOptions.Default;
        option.Loop = false;
        option.Location = Vector3.zero;
        option.Track = track;
        AudioPlayEvent.Trigger(clipNoLoop, option);
    }


    [InspectorButton(nameof(ClipPersistent))]
    public bool ClipPersistentButton;
    public virtual void ClipPersistent()
    {
        AudioManagerOptions option = AudioManagerOptions.Default;
        option.Loop = true;
        option.Location = Vector3.zero;
        option.Track = track;
        option.Persistent = true;
        AudioPlayEvent.Trigger(clipLoop, option);
    }


    [InspectorButton(nameof(OtherAudioGroup))]
    public bool OtherAudioGroupButton;
    public virtual void OtherAudioGroup()
    {
        AudioManagerOptions option = AudioManagerOptions.Default;
        option.Loop = true;
        option.Location = Vector3.zero;
        option.AudioGroup = otherAMG;
        AudioPlayEvent.Trigger(clipLoop, option);
    }


    [InspectorButton(nameof(FaderSound))]
    public bool FaderSoundButton;
    public virtual void FaderSound()
    {
        AudioManagerOptions option = AudioManagerOptions.Default;
        option.Loop = true;
        option.Location = Vector3.zero;
        option.Track = track;
        option.Fade = true;
        option.FadeInitialVolume = 1f;
        option.Volume = 0;
        option.FadeDuration = 10f;
        AudioPlayEvent.Trigger(clipLoop, option);
    }


    [InspectorButton(nameof(SoloSingleTarck))]
    public bool SoloSingleTrackButton;
    public virtual void SoloSingleTarck()
    {
        AudioManagerOptions option = AudioManagerOptions.Default;
        option.Loop = false;
        option.Location = Vector3.zero;
        option.Track = SoundTracks.BGM;
        option.SoloSingleTrack = true;
        option.AutoUnSoloOnEnd = true;
        AudioPlayEvent.Trigger(clipNoLoop, option);
    }

    [InspectorButton(nameof(SoloAllTracks))]
    public bool SoloAllTracksButton;
    public virtual void SoloAllTracks()
    {
        AudioManagerOptions option = AudioManagerOptions.Default;
        option.Loop = true;
        option.Location = Vector3.zero;
        option.Track = SoundTracks.BGM;
        option.SoloAllTracks = true;
        option.AutoUnSoloOnEnd = true;
        AudioPlayEvent.Trigger(clipNoLoop,option);
    }

    #endregion

    #region TrackEvent

    [Header("TrackEvent")]
    public SoundTracks TrackEvent_Track = SoundTracks.BGM;
    public float TrackEvent_SetVolume = 1f;

    [InspectorButton(nameof(TrackEvent_MuteTrack))]
    public bool TrackEvent_MuteTrackButton;
    public virtual void TrackEvent_MuteTrack()
    {
        TrackEvent.Trigger(TrackEventTypes.MuteTrack,TrackEvent_Track);
    }


    [InspectorButton(nameof(TrackEvent_UnMuteTrack))]
    public bool TrackEvent_UnMuteTrackButton;
    public virtual void TrackEvent_UnMuteTrack()
    {
        TrackEvent.Trigger(TrackEventTypes.UnmuteTrack,TrackEvent_Track);
    }


    [InspectorButton(nameof(TrackEvent_SetVolumeTrack))]
    public bool TrackEvent_SetVolumeTrackButton;
    public void TrackEvent_SetVolumeTrack()
    {
        TrackEvent.Trigger(TrackEventTypes.SetVolumeTrack, TrackEvent_Track, TrackEvent_SetVolume);
    }


    [InspectorButton(nameof(TrackEvent_PauseTrack))]
    public bool TrackEvent_PauseTrackButton;
    protected virtual void TrackEvent_PauseTrack()
    {
        TrackEvent.Trigger(TrackEventTypes.PauseTrack, SoundTracks.BGM, 0.5f);
    }


    [InspectorButton(nameof(TrackEvent_StopTrack))]
    public bool TrackEvent_StopTrackButton;
    protected virtual void TrackEvent_StopTrack()
    {
        TrackEvent.Trigger(TrackEventTypes.StopTrack, SoundTracks.BGM, 0.5f);
    }


    [InspectorButton(nameof(TrackEvent_PlayTrack))]
    public bool TrackEvent_PlayTrackButton;
    protected virtual void TrackEvent_PlayTrack()
    {
        TrackEvent.Trigger(TrackEventTypes.PlayTrack, SoundTracks.BGM, 0.5f);
    }


    [InspectorButton(nameof(TrackEvent_FreeTrack))]
    public bool TrackEvent_FreeTrackButton;
    protected virtual void TrackEvent_FreeTrack()
    {
        TrackEvent.Trigger(TrackEventTypes.FreeTrack, SoundTracks.BGM, 0.5f);
    }

    #endregion

    #region SettingsEvent

    [Header("SettingsEvent")]
    [InspectorButton(nameof(SettingsEvent_SaveSettings))]
    public bool SettingsEvent_SaveSettingsButton;
    protected virtual void SettingsEvent_SaveSettings()
    {
        SettingsEvent.Trigger(SettingsEventTypes.SaveSettings);
    }

    [InspectorButton(nameof(SettingsEvent_LoadSettings))]
    public bool SettingsEvent_LoadSettingsButton;
    protected virtual void SettingsEvent_LoadSettings()
    {
        SettingsEvent.Trigger(SettingsEventTypes.LoadSettings);
    }

    [InspectorButton(nameof(SettingsEvent_ResetSettings))]
    public bool SettingsEvent_ResetSettingsButton;
    protected virtual void SettingsEvent_ResetSettings()
    {
        SettingsEvent.Trigger(SettingsEventTypes.ResetSettings);
    }

    #endregion

    #region SoundControlEvent

    [Header("SoundControlEvent")]

    [InspectorButton(nameof(SoundControlEvent_Pause))]
    public bool SoundControlEvent_PauseButton;
    protected virtual void SoundControlEvent_Pause()
    {
        SoundControlEvent.Trigger(SoundControlEventTypes.Pause, 1);
    }

    [InspectorButton(nameof(SoundControlEvent_Resume))]
    public bool SoundControlEvent_ResumeButton;
    protected virtual void SoundControlEvent_Resume()
    {
        SoundControlEvent.Trigger(SoundControlEventTypes.Resume, 1);
    }

    [InspectorButton(nameof(SoundControlEvent_Stop))]
    public bool SoundControlEvent_StopButton;
    protected virtual void SoundControlEvent_Stop()
    {
        SoundControlEvent.Trigger(SoundControlEventTypes.Stop, 1);
    }

    [InspectorButton(nameof(SoundControlEvent_Free))]
    public bool SoundControlEvent_FreeButton;
    protected virtual void SoundControlEvent_Free()
    {
        SoundControlEvent.Trigger(SoundControlEventTypes.Free, 1);
    }



    #endregion

    #region AllSoundControlEvent

    [Header("AllSoundControlEvent")]
    [InspectorButton("AllSoundsPause")]
    public bool AllSoundsPauseButton;
    protected virtual void AllSoundsPause()
    {
        AllSoundsControlEvent.Trigger(AllSoundsControlEventTypes.Pause);
    }

    [InspectorButton("AllSoundsPlay")]
    public bool AllSoundsPlayButton;
    protected virtual void AllSoundsPlay()
    {
        AllSoundsControlEvent.Trigger(AllSoundsControlEventTypes.Play);
    }

    [InspectorButton("AllSoundsStop")]
    public bool AllSoundsStopButton;
    protected virtual void AllSoundsStop()
    {
        AllSoundsControlEvent.Trigger(AllSoundsControlEventTypes.Stop);
    }

    [InspectorButton("AllSoundsFree")]
    public bool AllSoundsFreeButton;
    protected virtual void AllSoundsFree()
    {
        AllSoundsControlEvent.Trigger(AllSoundsControlEventTypes.Free);
    }

    [InspectorButton("AllSoundsFreeAllButPersistent")]
    public bool AllSoundsFreeAllButPersistentButton;
    protected virtual void AllSoundsFreeAllButPersistent()
    {
        AllSoundsControlEvent.Trigger(AllSoundsControlEventTypes.FreeAllButPersistent);
    }

    [InspectorButton("AllSoundsFreeAllLooping")]
    public bool AllSoundsFreeAllLoopingButton;
    protected virtual void AllSoundsFreeAllLooping()
    {
        AllSoundsControlEvent.Trigger(AllSoundsControlEventTypes.FreeAllLooping);
    }




    #endregion

    #region SoundFadeEvent

    [Header("SoundFadeEvent")]

    public TweenType SoundFadeEvent_TweenType;
    [InspectorButton(nameof(SoundFadeEvent_Void))]
    public bool SoundFadeEvent_Button;
    public virtual void SoundFadeEvent_Void()
    {
        SoundFadeEvent.Trigger(0, 10f, 0f, SoundFadeEvent_TweenType);
    }
    #endregion

    #region TrackFadeEvent

    public SoundTracks TrackFadeEvent_Track = SoundTracks.BGM;
    public TweenType TrackFadeEvent_TweenType;

    [InspectorButton("TestTrackFadeEvent")]
    public bool TestTrackFadeEventButton;
    protected virtual void TestTrackFadeEvent()
    {
        TrackFadeEvent.Trigger(TrackFadeEvent_Track, 10, 0, TrackFadeEvent_TweenType);
    }

    #endregion



}
