using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationFix : MonoBehaviour
{
   
    void Update()
    {
        transform.rotation = Quaternion.Euler(0,0,0);
    }
}
