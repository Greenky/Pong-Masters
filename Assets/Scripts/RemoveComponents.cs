using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveComponents : Photon.MonoBehaviour
{
    [SerializeField] Behaviour[] RemoveComponent;
    void Awake()
    {
       
        if (photonView.isMine)
        {
          
        }
        else
        {
            foreach(Behaviour item in RemoveComponent)
            {
                item.enabled = false;
            }
        }

      
    }
}
