using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float TimeScale;

    private void Update()
    {
        Time.timeScale = TimeScale;
    }
}
