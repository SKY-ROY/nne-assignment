using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEditor;
using UnityEngine;

public class NetworkPlayerEnhanced : NetworkBehaviour
{
    // public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>();
    // public NetworkVariable<Vector3> Rotation = new NetworkVariable<Vector3>();

    private FirstPersonController firstPersonController;

    public override void OnNetworkSpawn()
    {
        firstPersonController = GetComponent<FirstPersonController>();

        if (!IsLocalPlayer)
        {
            firstPersonController.ActivateCamera(false);
            // GetComponent<FirstPersonController>().enabled = false;
        }
    }

    void Update()
    {
        if (IsLocalPlayer)
        {
            Vector3 localPos = Vector3.zero, localRot = Vector3.zero;

            firstPersonController.FreeMovement(ref localPos, ref localRot);

            SubmitPosRotRequestServerRpc(localPos, localRot);

            firstPersonController.InteractionHandler();
        }
    }

    [ServerRpc]
    void SubmitPosRotRequestServerRpc(Vector3 moveTowards, Vector3 lookAt/*, ServerRpcParams rpcParams = default*/)
    {
        FetchProcessedPosRotClientRpc(moveTowards, lookAt);
    }

    [ServerRpc]
    void AlignDataCubeRequestServerRpc(Vector3 resPos, Vector3 resRot)
    {
        FetchProcessedAlignmentClientRpc(resPos, resRot);
    }

    [ClientRpc]
    void FetchProcessedPosRotClientRpc(Vector3 moveTowards, Vector3 lookAt/*, ClientRpcParams clientRpcParams = default*/)
    {
        if (IsLocalPlayer)
            return;

        firstPersonController.SimulateMovement(moveTowards, lookAt);
    }

    [ClientRpc]
    void FetchProcessedAlignmentClientRpc(Vector3 resPos, Vector3 resRot)
    {
        if (IsLocalPlayer)
            return;

        // firstPersonController.
    }
}
