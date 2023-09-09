using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSystem : SystemRoot
{
    private static BattleSystem instance;
    public static BattleSystem Instance { get { return instance; } }

    public override void InitSystem()
    {
        base.InitSystem();
        instance = this;
        this.Log("Init BattleSystem Done.");
    }
}
