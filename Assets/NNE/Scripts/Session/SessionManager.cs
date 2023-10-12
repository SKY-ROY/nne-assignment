using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SessionManager : MonoBehaviour
{
    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, 10, 500, 500));
        if (!NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsServer)
        {
            StartButtons();
        }
        else
        {
            StatusLabels();
        }

        GUILayout.EndArea();
    }

    static void StartButtons()
    {
        if (GUILayout.Button("Host", GUILayout.Width(100), GUILayout.Height(50))) NetworkManager.Singleton.StartHost();
        if (GUILayout.Button("Client", GUILayout.Width(100), GUILayout.Height(50))) NetworkManager.Singleton.StartClient();
        if (GUILayout.Button("Server", GUILayout.Width(100), GUILayout.Height(50))) NetworkManager.Singleton.StartServer();
    }

    static void StatusLabels()
    {
        var mode = NetworkManager.Singleton.IsHost ? "Host" : (NetworkManager.Singleton.IsServer ? "Server" : "Client");

        GUILayout.Label("Transport: " + NetworkManager.Singleton.NetworkConfig.NetworkTransport.GetType().Name);

        GUILayout.Label("Mode: " + mode);
    }
}
