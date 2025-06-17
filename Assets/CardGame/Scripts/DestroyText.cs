using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DestroyText : MonoBehaviour
{
    
    void Start()
    {
        var txt = GetComponent<TextMeshProUGUI>();
        if (txt) txt.enabled = false;
    }

    
}