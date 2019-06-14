using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testing : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.DpadUp))
        {
            transform.Translate(0.0f, 1.0f, 0.0f);
        }
        else if (OVRInput.GetDown(OVRInput.Button.DpadDown))
        {
            transform.Translate(0.0f, -1.0f, 0.0f);
        }
    }
}
