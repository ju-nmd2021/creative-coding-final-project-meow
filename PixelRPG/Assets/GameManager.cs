using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // set vsync to maximize fps. idk how it works
        // QualitySettings.vSyncCount = 1;
        // Application.targetFrameRate = 80; // This will update the current setting, but be ignored due to vSyncCount being greater than 0.
    }

    // Update is called once per frame
    void Update()
    {
    }
}
