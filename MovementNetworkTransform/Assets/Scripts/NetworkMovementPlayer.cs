using Unity.Netcode;
using UnityEngine;

namespace NetworkMovement
{
    public class NetworkMovementPlayer : NetworkBehaviour
    {
        public NetworkVariable<Color> Color = new NetworkVariable<Color>();
        public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>();
        public float moveSpeed;
        public float jumpSpeed;

        public override void OnNetworkSpawn()
        {
            if (IsOwner)
            {
                Move();
                ChangeColor();
            }
        }

        public void Move()
        {
            SubmitPositionRequestServerRpc();
        }

        public void ChangeColor()
        {
            SubmitColorRequestServerRpc();
        }

        [Rpc(SendTo.Server)]
        void SubmitPositionRequestServerRpc(RpcParams rpcParams = default)
        {
            var randomPosition = GetRandomPositionOnPlane();
            transform.position = randomPosition;
            Position.Value = randomPosition;
        }

        static Vector3 GetRandomPositionOnPlane()
        {
            return new Vector3(Random.Range(-3f, 3f), 1f, Random.Range(-3f, 3f));
        }

        [Rpc(SendTo.Server)]
        void SubmitColorRequestServerRpc(RpcParams rpcParams = default)
        {
            int tamano = NetworkMovementManager.instance.colors.Count;
            int rand = Random.Range(0, tamano);
            var randomColor = NetworkMovementManager.instance.colors[rand];
            GetComponent<MeshRenderer>().materials[0].color = randomColor;
            Color.Value = randomColor;
        }

        void Update() {
            if (IsOwner) {
                if (Input.GetKeyDown(KeyCode.Space) && transform.position.y <= 1.01f) {
                    SubmitJumpServerRpc();
                }
            }
            GetComponent<MeshRenderer>().materials[0].color = Color.Value;
        }

        void FixedUpdate() {
            if (IsOwner) {
                float horizontalInput = Input.GetAxis("Horizontal");
                float verticalInput = Input.GetAxis("Vertical");
                Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput) * moveSpeed * Time.fixedDeltaTime;
                SubmitMoveServerRpc(movement);
            }
        }

        [Rpc(SendTo.Server)]
        void SubmitMoveServerRpc(Vector3 movement, RpcParams rpcParams = default) {
            transform.position += movement;

            if (transform.position.x > 4.5f) {
                transform.position = new Vector3(4.5f, transform.position.y, transform.position.z);
            } else if (transform.position.x < -4.5f) {
                transform.position = new Vector3(-4.5f, transform.position.y, transform.position.z);
            } 
            
            if (transform.position.z < -4.5f) {
                transform.position = new Vector3(transform.position.x, transform.position.y, -4.5f);
            } else if (transform.position.z > 4.5f) {
                transform.position = new Vector3(transform.position.x, transform.position.y, 4.5f);
            }

            if (transform.position.y < 1f) {
                transform.position = new Vector3(transform.position.x, 1f, transform.position.z);   
            }
        }

        [Rpc(SendTo.Server)]
        void SubmitJumpServerRpc(RpcParams rpcParams = default) {
            GetComponent<Rigidbody>().AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
        }
    }
}