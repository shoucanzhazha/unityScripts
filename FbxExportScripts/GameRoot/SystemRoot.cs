using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemRoot : MonoBehaviour
{
    protected GameRoot gameRoot;
    protected NetService netService;
    protected AudioService audioService;
    protected ResService resService;

    public virtual void InitSystem()
    {
        gameRoot = GameRoot.Instance;
        netService = NetService.Instance;
        audioService = AudioService.Instance;
        resService = ResService.Instance;
    }


}
