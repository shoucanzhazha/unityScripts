using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResService : MonoBehaviour
{
    private static ResService instance;
    public static ResService Instance {get {return instance;}}

    public void InitResService()
    {
        instance = this;
        this.Log("Init ResService Done.");
    }
}
