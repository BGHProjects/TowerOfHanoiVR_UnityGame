using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class floor : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
	{
		if (OVRInput.GetDown(OVRInput.Button.DpadUp)) {
			Renderer rend = GetComponent<Renderer>();
			rend.material.SetColor("_Color", Color.blue);
			//gameObject.material.color = Color.blue;

		} else if (OVRInput.GetDown(OVRInput.Button.DpadDown)) {
            Renderer rend = GetComponent<Renderer>();
            rend.material.SetColor("_Color", Color.red);
        }
        
    }
}
