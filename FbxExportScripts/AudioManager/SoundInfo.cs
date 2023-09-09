using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct SoundInfo
{
    public int ID;
    public SoundTracks Track;
    public AudioSource Source;
    //在切换场景时是否要保持
    public bool Persistent;

}
