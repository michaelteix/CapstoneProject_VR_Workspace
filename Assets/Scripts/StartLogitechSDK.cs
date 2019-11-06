using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartLogitechSDK : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        System.Diagnostics.Process.Start("v2.0_Logitech_BridgeSDK/Logitech_Bridge.exe");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
