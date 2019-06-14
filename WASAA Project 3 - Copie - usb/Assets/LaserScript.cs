using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace WasaaMP {
    public class LaserScript : MonoBehaviour {
        
        private GameObject laser;
        private Transform laserTransform;
        private Vector3 hitPoint;
        private GameObject mirroredCube;

        public GameObject laserPrefab;
        public GameObject cursor;
        public LineRenderer laserLineRenderer;
        public float laserWidth = 5f;
        public float laserMaxLength = 5f;

        private bool caught;

        public Interactive interactiveObjectToInstanciate;
        private GameObject target;
        MonoBehaviourPun targetParent;

        MonoBehaviourPun player;

        float x, previousX = 0;
        float y, previousY = 0;
        float z, lastZ;
        public bool active;

        void Start() {
            mirroredCube = GameObject.Find("Mirrored Cube").gameObject;
            laser = Instantiate(laserPrefab);
            laserTransform = laser.transform;

            Vector3[] initLaserPositions = new Vector3[2] { Vector3.zero, Vector3.zero };
            laserLineRenderer.SetPositions(initLaserPositions);
            laserLineRenderer.SetWidth(laserWidth, laserWidth);

            active = false;
            caught = false;
            player = (MonoBehaviourPun)this.GetComponentInParent(typeof(Navigation));
            name = player.name + "_" + name;
        }

        private void ShowLaser(RaycastHit hit) {
            mirroredCube.SetActive(false);
            laser.SetActive(true);
            laserTransform.position = Vector3.Lerp(cursor.transform.position, hitPoint, .5f);
            laserTransform.LookAt(hitPoint);
            laserTransform.localScale = new Vector3(laserTransform.localScale.x, laserTransform.localScale.y, hit.distance);
        }

        private void ShowLaser() {
            laser.SetActive(true);
            mirroredCube.SetActive(true);
        }

        void mirroredObject() {
            Vector3 controllerPos = cursor.transform.forward;
            float distance_formula_on_vector = Mathf.Sqrt(controllerPos.x * controllerPos.x + controllerPos.y * controllerPos.y + controllerPos.z * controllerPos.z);
            Vector3 mirroredPos = cursor.transform.position;

            mirroredPos.x = mirroredPos.x + (100f / (distance_formula_on_vector)) * controllerPos.x;
            mirroredPos.y = mirroredPos.y + (100f / (distance_formula_on_vector)) * controllerPos.y;
            mirroredPos.z = mirroredPos.z + (100f / (distance_formula_on_vector)) * controllerPos.z;

            mirroredCube.transform.position = mirroredPos;
            mirroredCube.transform.rotation = cursor.transform.rotation;
        }

        void Update() {

            if (player.photonView.IsMine || !PhotonNetwork.IsConnected) {
                if (Input.GetButtonDown("Fire1")) {
                    Fire1Pressed(Input.mousePosition.x, Input.mousePosition.y);
                }
                if (Input.GetButtonUp("Fire1")) {
                    Fire1Released(Input.mousePosition.x, Input.mousePosition.y);
                }
                if (active) {
                    Fire1Moved(Input.mousePosition.x, Input.mousePosition.y, Input.mouseScrollDelta.y);
                }
                if (Input.GetKeyDown(KeyCode.C)) {
                    CreateInteractiveCube();
                }
                if (Input.GetKeyDown(KeyCode.B)) {
                    Catch();
                }
                if (Input.GetKeyDown(KeyCode.N)) {
                    Release();
                    target = null;
                }
            }

            Vector3 forward = transform.TransformDirection(Vector3.forward) * 30;
            Debug.DrawRay(transform.position, forward, Color.green);

            /*
            if ( Input.GetKeyDown( KeyCode.Space ) ) {
                 ShootLaserFromTargetPosition( transform.position, Vector3.forward, laserMaxLength );
                 laserLineRenderer.enabled = true;
             }
             else {
                 laserLineRenderer.enabled = false;
             }

             mirroredObject();
            ShowLaser();
            Ray ray = Camera.main.ScreenPointToRay(cursor.transform.position);
            RaycastHit hit;
            if (Physics.Raycast(cursor.transform.position, cursor.transform.forward, out hit, 100)) {
                print("Hit "+ hit.transform.name);
                hitPoint = hit.point;                
            }
                        ShowLaser(hit);	 
             */
        }
        
        public void Catch() {
            print("B ?");
            if (target != null) {
                print("B :");
                var tb = target.GetComponent<Interactive>();
                if (tb != null) {
                    if ((!caught) && (this != tb.GetSupport())) { // pour ne pas prendre 2 fois l'objet et lui faire perdre son parent
                        targetParent = tb.GetSupport();
                    }
                    print("ChangeSupport of object " + tb.photonView.ViewID + " to " + tb.photonView.ViewID);
                    tb.photonView.RPC("ChangeSupport", RpcTarget.All, tb.photonView.ViewID, tb.photonView.ViewID);
                    tb.photonView.TransferOwnership(PhotonNetwork.LocalPlayer);
                    PhotonNetwork.SendAllOutgoingCommands();
                    caught = true;
                    print("B !");
                }
            } else {
                print("pas B");
            }
        }

        public void Release() {
            if (target != null) {
                print("N :");
                var tb = target.GetComponent<Interactive>();
                if (tb != null) {
                    if (targetParent != null) {
                        tb.photonView.RPC("ChangeSupport", RpcTarget.All, tb.photonView.ViewID, targetParent.photonView.ViewID);
                        targetParent = null;
                    } else {
                        tb.photonView.RPC("RemoveSupport", RpcTarget.All, tb.photonView.ViewID);
                    }
                    PhotonNetwork.SendAllOutgoingCommands();
                    print("N !");
                    caught = false;
                }
            } else {
                print("pas N");
            }
        }
        

        public void Fire1Pressed(float mouseX, float mouseY) {
            active = true;
            x = mouseX;
            previousX = x;
            y = mouseY;
            previousY = y;
        }

        public void Fire1Released(float mouseX, float mouseY) {
            active = false;
        }

        public void Fire1Moved(float mouseX, float mouseY, float mouseZ) {
            x = mouseX;
            float deltaX = (x - previousX) / 100.0f;
            previousX = x;
            y = mouseY;
            float deltaY = (y - previousY) / 100.0f;
            previousY = y;
            float deltaZ = mouseZ / 10.0f;
            transform.Translate(deltaX, deltaY, deltaZ);
        }

        public void CreateInteractiveCube() {
            var objectToInstanciate = PhotonNetwork.Instantiate(interactiveObjectToInstanciate.name, transform.position, transform.rotation, 0);
        }

        void ShootLaserFromTargetPosition(Vector3 targetPosition, Vector3 direction, float length) {
            Ray ray = new Ray(targetPosition, direction);
            RaycastHit raycastHit;
            Vector3 endPosition = targetPosition + (length * direction);

            if (Physics.Raycast(ray, out raycastHit, length)) {
                endPosition = raycastHit.point;
            }

            laserLineRenderer.SetPosition(0, targetPosition);
            laserLineRenderer.SetPosition(1, endPosition);
        }
    }
}
 