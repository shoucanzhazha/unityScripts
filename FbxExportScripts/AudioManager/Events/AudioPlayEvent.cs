using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioPlayEvent
{
    public delegate AudioSource Delegate(AudioClip clip, AudioManagerOptions options);
    static private event Delegate OnEvent;

    static public void Register(Delegate callback)
    {
        OnEvent += callback;
    }

    static public void Unregister(Delegate callback)
    {
        OnEvent -= callback;
    }

    static public AudioSource Trigger(AudioClip clip, AudioManagerOptions options)
    {
        return OnEvent?.Invoke(clip, options);
    }

    static public AudioSource Trigger(AudioClip audioClip, SoundTracks track, Vector3 location,
           bool loop = false, float volume = 1.0f, int ID = 0,
           bool fade = false, float fadeInitialVolume = 0f, float fadeDuration = 1f, TweenType fadeTween = null,
           bool persistent = false,AudioSource recycleAudioSource = null, AudioMixerGroup audioGroup = null,
           float pitch = 1f, float panStereo = 0f, float spatialBlend = 0.0f,bool soloSingleTrack = false,
           bool soloAllTracks = false, bool autoUnSoloOnEnd = false,bool bypassEffects = false, 
           bool bypassListenerEffects = false, bool bypassReverbZones = false, int priority = 128,
           float reverbZoneMix = 1f,float dopplerLevel = 1f, int spread = 0, 
           AudioRolloffMode rolloffMode = AudioRolloffMode.Logarithmic, float minDistance = 1f, float maxDistance = 500f)
    {
        AudioManagerOptions options = AudioManagerOptions.Default;
        options.Track = track;
        options.Location = location;
        options.Loop = loop;
        options.Volume = volume;
        options.ID = ID;
        options.Fade = fade;
        options.FadeInitialVolume = fadeInitialVolume;
        options.FadeDuration = fadeDuration;
        options.FadeTween = fadeTween;
        options.Persistent = persistent;
        options.RecycleAudioSource = recycleAudioSource;
        options.AudioGroup = audioGroup;
        options.Pitch = pitch;
        options.PanStereo = panStereo;
        options.SpatialBlend = spatialBlend;
        options.SoloSingleTrack = soloSingleTrack;
        options.SoloAllTracks = soloAllTracks;
        options.AutoUnSoloOnEnd = autoUnSoloOnEnd;
        options.BypassEffects = bypassEffects;
        options.BypassListenerEffects = bypassListenerEffects;
        options.BypassReverbZones = bypassReverbZones;
        options.Priority = priority;
        options.ReverbZoneMix = reverbZoneMix;
        options.DopplerLevel = dopplerLevel;
        options.Spread = spread;
        options.RolloffMode = rolloffMode;
        options.MinDistance = minDistance;
        options.MaxDistance = maxDistance;

        return OnEvent?.Invoke(audioClip, options);
    }



}
