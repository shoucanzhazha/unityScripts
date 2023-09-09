using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class AudioSettings
{
    public const float minVolume = 0.0001f;
    public const float maxVolume = 100f;
    public const float defaultVolume = 50f;

    [Header("Audio Mixer Control")]
    [Tooltip("是否支持修改")]
    public bool OverrideMixerSettings = true;

    public string MasterVolumeParameter = "MasterVolume";
    public string BGMVolumeParameter = "BGMVolume";
    public string SfxVolumeParameter = "SfxVolume";
    public string UIVolumeParameter = "UIVolume";

    [Header("Master")]
    [ReadOnly]
    [Range(minVolume, maxVolume)]
    public float MasterVolume = defaultVolume;
    [ReadOnly]
    public bool MasterOn = true;
    [ReadOnly]
    public float MutedMasterVolume;

    [Header("BGM")]
    [ReadOnly]
    [Range(minVolume, maxVolume)]
    public float BGMVolume = defaultVolume;
    [ReadOnly]
    public bool BGMOn = true;
    [ReadOnly]
    public float MutedBGMVolume;

    [Header("Sound Effects")]
    [ReadOnly]
    [Range(minVolume, maxVolume)]
    public float SfxVolume = defaultVolume;
    [ReadOnly]
    public bool SfxOn = true;
    [ReadOnly]
    public float MutedSfxVolume;

    [Header("UI")]
    [ReadOnly]
    [Range(minVolume, maxVolume)]
    public float UIVolume = defaultVolume;
    [ReadOnly]
    public bool UIOn = true;
    [ReadOnly]
    public float MutedUIVolume;

    [Header("Save & Load")]
    public bool AutoLoad = true;
    public bool AutoSave = false;

}
