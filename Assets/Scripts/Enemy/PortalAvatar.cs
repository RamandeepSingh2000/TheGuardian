using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalAvatar : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DeactivatePortalScript dPortalScript = gameObject.GetComponentInParent<DeactivatePortalScript>();
        if ( dPortalScript != null)
        {
            dPortalScript.OnPortalAttacked += () => HurtAnimation();
        }
    }

    private void HurtAnimation()
    {
        Debug.Log("HurtAnimation not implemented yet");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
