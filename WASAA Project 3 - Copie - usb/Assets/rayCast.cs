using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEditor;

namespace WasaaMP{
    public class rayCast : MonoBehaviourPun
    {
        private GameObject laser;
        private Transform laserTransform;
        private Vector3 hitPoint;
        private GameObject mirroredCube;

        public GameObject laserPrefab;
        public bool isCaught = false;

        void Start() {
            mirroredCube = GameObject.Find("Mirrored Cube").gameObject;
            laser = Instantiate(laserPrefab);
            laserTransform = laser.transform;
        }

        private void ShowLaser(RaycastHit hit) {
            mirroredCube.SetActive(false);
            laser.SetActive(true);
            laserTransform.position = Vector3.Lerp(transform.position, hitPoint, .5f);
            laserTransform.LookAt(hitPoint);
            laserTransform.localScale = new Vector3(laserTransform.localScale.x, laserTransform.localScale.y, hit.distance);
        }

        private void ShowLaser() {
            laser.SetActive(true);
            mirroredCube.SetActive(true);
        }

        void mirroredObject() {
            Vector3 controllerPos = transform.forward;
            float distance_formula_on_vector = Mathf.Sqrt(controllerPos.x * controllerPos.x + controllerPos.y * controllerPos.y + controllerPos.z * controllerPos.z);
            Vector3 mirroredPos = transform.position;

            mirroredPos.x = mirroredPos.x + (100f / (distance_formula_on_vector)) * controllerPos.x;
            mirroredPos.y = mirroredPos.y + (100f / (distance_formula_on_vector)) * controllerPos.y;
            mirroredPos.z = mirroredPos.z + (100f / (distance_formula_on_vector)) * controllerPos.z;

            mirroredCube.transform.position = mirroredPos;
            mirroredCube.transform.rotation = transform.rotation;
        }

        public void Catch(GameObject obj) {
            print("Catch obj ");
            var tb = obj.GetComponent<Interactive>();
            photonView.RPC("ChangeSupport", RpcTarget.All, tb.photonView.ViewID, photonView.ViewID);
            tb.photonView.TransferOwnership(PhotonNetwork.LocalPlayer);
            PhotonNetwork.SendAllOutgoingCommands();
        }

        public void Release(GameObject obj) {
            var tb = obj.GetComponent<Interactive>();
            photonView.RPC("RemoveSupport", RpcTarget.All, tb.photonView.ViewID);
            PhotonNetwork.SendAllOutgoingCommands();
        }

        [PunRPC] public void ChangeSupport(int interactiveID, int newSupportID) {
            Interactive go = PhotonView.Find(interactiveID).gameObject.GetComponent<Interactive>();
            MonoBehaviourPun s = PhotonView.Find(newSupportID).gameObject.GetComponent<MonoBehaviourPun>();
            print("ChangeSupport of object " + go.name + " to " + s.name);
            go.SetSupport(s);
        }

        [PunRPC]
        public void RemoveSupport(int interactiveID)
        {
            Interactive go = PhotonView.Find(interactiveID).gameObject.GetComponent<Interactive>();
            print("RemoveSupport of object " + go.name);
            go.RemoveSupport();
        }

        public void pickupp(GameObject hit) {

                print("pickup called");

                //Vector3 temp = hit.transform.position;
                //this.transform.position = temp;
                hit.transform.SetParent(this.transform);
                
                //hit.transform.position = new Vector3(hit.transform.position.x, hit.transform.position.y, - 1.45f);
                hit.GetComponent<Rigidbody>().isKinematic = true;                  

        }

        public void dropp(GameObject hit) {
            //print("drop called");
            if (OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger) || Input.GetKeyDown(KeyCode.B))
            {
                hit.transform.SetParent(null);
                selectedObject = null;
                hit.GetComponent<Rigidbody>().isKinematic = false;
            }
        }

        private GameObject selectedObject;
        private float timer = 0f;

        void pickup(GameObject hit) {
            if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger) || Input.GetKeyDown(KeyCode.B)) {
                if (selectedObject == null)
                {

                    var col = hit.GetComponent<Collision>();
                    bool above = col.getAbove();
                    bool below = col.getBelow();

                    //only catch if none above
                    if (!above)
                    {
                        print(hit.name);

                        hit.transform.eulerAngles = new Vector3(-90, 0, 0);
                        selectedObject = hit.gameObject;
                        //Catch(hit);
                        pickupp(hit);

                        selectedObject.transform.position = new Vector3(selectedObject.transform.position.x, 8.0f, 1.45f);
                    }
                    else
                    {
                        //EditorUtility.DisplayDialog("Error", "Can't pick this ring up", "Okay");
                        print("cant pickup this ring");
                    }
                }
                }
            }

        void changeDepth() {

        }

        void Update() {
            mirroredObject();
            ShowLaser();
            if (selectedObject != null) {
                dropp(selectedObject);
            }
            Ray ray = Camera.main.ScreenPointToRay(transform.position);
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, 100)) {
                //print("Hit " + hit.transform.name);
                if (hit.transform.gameObject.GetComponent<Interactive>() != null) {
                    //pickup(hit.transform.gameObject);
                    pickup(hit.transform.gameObject);
                }
                hitPoint = hit.point;
            }
            ShowLaser(hit);

            if (OVRInput.GetDown(OVRInput.Button.DpadUp) && selectedObject != null)
            {
                selectedObject.transform.Translate(0.0f, -0.5f, 0.0f);
            }
            if (OVRInput.GetDown(OVRInput.Button.DpadDown) && selectedObject != null)
            {
                selectedObject.transform.Translate(0.0f, 0.5f, 0.0f);
            }

            //reset to be on the z axis correctly
            selectedObject.transform.position = new Vector3(selectedObject.transform.position.x, selectedObject.transform.position.y, 1.45f);
        }



    }
}
