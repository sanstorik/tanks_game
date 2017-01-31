using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class ChangeRotate : NetworkBehaviour {

    [ClientRpc]
    public void RpcSetRotationAndFire(GameObject go, Quaternion rot, float force)
    {
        if (!isServer)
        {
            go.transform.rotation = rot;
            print(rot.eulerAngles.z);

            go.GetComponent<WeaponBehaviourNetwork>().FireAndAddForce(force);
        }
    }
}
