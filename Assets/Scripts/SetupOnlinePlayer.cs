using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupOnlinePlayer : Photon.MonoBehaviour
{
    void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        gameObject.name = info.sender.ToString();
        gameObject.transform.parent = GameObject.FindGameObjectWithTag("Dynamic").GetComponent<Transform>();
    }
}
