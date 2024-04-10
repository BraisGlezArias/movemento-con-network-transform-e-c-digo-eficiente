using Unity.Netcode;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace NetworkMovement
{
    public class NetworkMovementManager : MonoBehaviour
    {
        public List<Color> colors;
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
            }

            GUILayout.EndArea();
        }

        static void StartButtons()
        {
            if (GUILayout.Button("Host")) NetworkManager.Singleton.StartHost();
            if (GUILayout.Button("Client")) NetworkManager.Singleton.StartClient();
        }
    }
}