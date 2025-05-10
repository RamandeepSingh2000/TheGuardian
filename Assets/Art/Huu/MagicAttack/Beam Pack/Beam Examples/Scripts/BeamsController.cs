using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class BeamsController : MonoBehaviour
{
    
    public VisualEffect fireBeam;
    
    void Update()
    {
       
        //Laser Beam Controller
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            fireBeam.Play();
        }
        
    }
}
