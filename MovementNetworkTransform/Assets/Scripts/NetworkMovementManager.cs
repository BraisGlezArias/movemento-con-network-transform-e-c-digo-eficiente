using Unity.Netcode;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace NetworkMovement
{
    public class NetworkMovementManager : MonoBehaviour 
    {
        public List<Color> colors;
        public static int authorityWhere;
        public static NetworkMovementManager instance;

        void Awake() {
            instance = this;
        }

        void OnGUI()
        {
            GUILayout.BeginArea(new Rect(10, 10, 300, 300));
            if (!NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsServer)
            {
                StartButtons();
            } else {
                if (NetworkManager.Singleton.IsServer) {
                    ChangeAuthority();
                }
            }

            GUILayout.EndArea();
        }

        static void StartButtons()
        {
            if (GUILayout.Button("Host")) NetworkManager.Singleton.StartHost();
            if (GUILayout.Button("Client")) NetworkManager.Singleton.StartClient();
        }

        static void ChangeAuthority()
        {
            if (authorityWhere != 0) {
                if (GUILayout.Button("Change Authority to Server")) {
                    authorityWhere = 0;
                }
            }
            if (authorityWhere != 1) {
                if (GUILayout.Button("Change Authority to Server with Rewind")) {
                    authorityWhere = 1;
                } 
            }
            if (authorityWhere  != 2) {
                if (GUILayout.Button("Change Authority to Client")) {
                    authorityWhere = 2;
                }
            }

            foreach (ulong uid in NetworkManager.Singleton.ConnectedClientsIds)
                NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject(uid).GetComponent<NetworkMovementPlayer>().changeAuthorityRpc(authorityWhere);
        }
    }
}