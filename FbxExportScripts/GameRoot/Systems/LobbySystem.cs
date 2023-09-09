using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbySystem : SystemRoot
{
    private static LobbySystem instance;
    public static LobbySystem Instance { get { return instance; } }

    public override void InitSystem()
    {
        base.InitSystem();
        instance = this;
        this.Log("Init LobbySystem Done.");
    }
}
