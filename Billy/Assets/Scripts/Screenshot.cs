using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Screenshot : MonoBehaviour
{
#if UNITY_EDITOR
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ScreenCapture.CaptureScreenshot("Billy Screenshot " + DateTime.Now.Minute + DateTime.Now.Second + ".png");
            Debug.Log("screenshot");
        }
    }
#endif
}
