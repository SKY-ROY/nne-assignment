using System;
using Unity.Netcode;
using UnityEngine;

public class NetworkTransformTest : NetworkBehaviour
{
    void Update()
    {
        float theta = Time.frameCount / 10.0f;
        if (IsServer)
        {
            transform.position = new Vector3((float)Math.Cos(theta), 0.0f, (float)Math.Sin(theta));
        }
        else
        {
            transform.position = new Vector3(0, 0, 0);
        }
    }
}