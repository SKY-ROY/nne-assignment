using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEditor;
using UnityEngine;

public class NetworkPlayer : NetworkBehaviour
{
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

            localPos = Vector3.zero;
            localRot = Vector3.zero;

            if (firstPersonController.InteractionHandler(ref localPos, ref localRot))
                AlignDataCubeRequestServerRpc(localPos, localRot);
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

        firstPersonController.SimulateDataCubeOrientation(resPos, resRot);
    }
}
