using System;
using System.Collections;


using UnityEngine;
using UnityEngine.SceneManagement;


using Photon.Pun;
using Photon.Realtime;


namespace WasaaMP {
    public class GameManager : MonoBehaviourPunCallbacks {

        #region Public Fields

        public static GameManager Instance ;

        [Tooltip("The prefab to use for representing the player")]
        public Navigation playerPrefab ;

        #endregion
		
		public GameObject ringPrefab;

        void Start () {
            Instance = this ;
            if (playerPrefab == null) {
                Debug.LogError ("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'", this) ;
            } else {
                //Debug.LogFormat("We are Instantiating LocalPlayer from {0}", Application.loadedLevelName);
                // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
                if (Navigation.LocalPlayerInstance == null) {
                    Debug.LogFormat ("We are Instantiating LocalPlayer from {0}", SceneManagerHelper.ActiveSceneName) ;
                    // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
                    PhotonNetwork.Instantiate (this.playerPrefab.name, new Vector3 (0f, 0.5f, 0f), Quaternion.identity, 0) ;
                } else {
                    Debug.LogFormat ("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName) ;
                }
            }
			
			if (ringPrefab == null)
            {
                Debug.LogError("<Color=Red><a>Missing</a></Color> ringPrefab Reference. Please set it up in GameObject 'Game Manager'", this);
            }
            else
            {
                float y = 6;
                float s = 0.00001f;
                for (int i = 0; i < 9; i++)
                {
                    GameObject ring = PhotonNetwork.Instantiate(this.ringPrefab.name, new Vector3(10f, y, -1.45f), Quaternion.identity, 0);
                    ring.transform.Rotate(90, 0, 0);
                    ring.transform.localScale += new Vector3(s, s, 0);
                    ring.name = "ring_" + i;
                    //print(scale);
                    y -= 0.5f;
                    s += 0.000015f;

                }
                //GameObject ring = PhotonNetwork.Instantiate(this.ringPrefab.name, new Vector3(10f, 1f, 1f), Quaternion.identity, 0);
                //ring.transform.Rotate(90,0,0);
                //PhotonNetwork.Instantiate(this.ringPrefab.name, new Vector3(5f, 1f, 1f), Quaternion.identity, 8);
            }
        }

        #region Private Methods

        #endregion

        #region Photon Callbacks

        /// <summary>
        /// Called when the local player left the room. We need to load the launcher scene.
        /// </summary>
        public override void OnLeftRoom () {
            SceneManager.LoadScene (0) ;
        }

        public override void OnPlayerEnteredRoom (Player other) {
            Debug.LogFormat ("OnPlayerEnteredRoom() {0}", other.NickName) ; // not seen if you're the player connecting
            // we load the Arena only once, for the first user who connects, it is made by the launcher
            if (PhotonNetwork.IsMasterClient) {
                Debug.LogFormat ("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient) ; // called before OnPlayerLeftRoom
            }
        }

        public override void OnPlayerLeftRoom (Player other) {
            Debug.LogFormat ("OnPlayerLeftRoom() {0}", other.NickName) ; // seen when other disconnects
        }

        #endregion

        #region Public Methods

        public void LeaveRoom () {
            PhotonNetwork.LeaveRoom () ;
        }

        #endregion

    }
}