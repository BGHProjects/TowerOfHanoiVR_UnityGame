using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace WasaaMP {
public class Interactive : MonoBehaviourPun {
    // Start is called before the first frame update

    MonoBehaviourPun support = null ;

    void Start () {
        
    }

    // Update is called once per frame
    void Update () {
        //Debug.Log("test");
    }
	
	
    void OnTriggerEnter (Collider other) {

        //Debug.Log("test");
        //print (name + " : OnCollisionEnter") ;
		var hit = other.gameObject ;
		var cursor = hit.GetComponent<LaserScript> () ;
		//if (cursor != null) {
			//Renderer renderer = GetComponentInChildren <Renderer> () ;
		    //renderer.material.color = Color.blue ;
		//}
	}
    
    void OnTriggerExit (Collider other) {
        //print (name + " : OnCollisionExit") ;
		var hit = other.gameObject ;
		var cursor = hit.GetComponent<LaserScript> () ;
		//if (cursor != null) {
			//Renderer renderer = GetComponentInChildren <Renderer> () ;
		    //renderer.material.color = Color.red ;
		//}
	}

    public void SetSupport (MonoBehaviourPun support) {
        this.support = support ;

        if (support != null)
        {
                if (GetComponent<Rigidbody>() != null) {
                    GetComponent<Rigidbody>().isKinematic = true;
                    print("Picked up");
                    transform.SetParent(support.transform);
                }
        } else {
                transform.SetParent(null);
                
        }       
    }

    public void RemoveSupport () {
        GetComponent<Rigidbody>().isKinematic = false;
        transform.SetParent (null) ;
        support = null ;
    }

    public MonoBehaviourPun GetSupport () {
        return support ;
    }
}

}
