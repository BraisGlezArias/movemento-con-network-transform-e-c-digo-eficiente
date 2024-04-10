using Unity.Netcode;
using UnityEngine;

namespace NetworkMovement
{
    public class NetworkMovementPlayer : NetworkBehaviour
    {
        public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>();
        public float moveSpeed;
        public float jumpSpeed;
        public Rigidbody rb;

        public void Start() {
            rb = GetComponent<Rigidbody>();
        }

        public override void OnNetworkSpawn()
        {
            if (IsOwner)
            {
                Move();
            }
        }

        public void Move()
        {
            SubmitPositionRequestServerRpc();
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

        void Update() {
            if (IsOwner) {
                if (Input.GetKeyDown(KeyCode.Space) && transform.position.y <= 1f) {
                    SubmitJumpServerRpc();
                }
            }
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

            //if (transform.position.x > 3f) {
            //    transform.position = new Vector3(3f, transform.position.y, transform.position.z);
            //} else if (transform.position.x < -3f) {
            //    transform.position = new Vector3(-3f, transform.position.y, transform.position.z);
            //} else if (transform.position.z < -3f) {
            //    transform.position = new Vector3(transform.position.x, transform.position.y, -3f);
            //} else if (transform.position.z > 3f) {
            //    transform.position = new Vector3(transform.position.x, transform.position.y, 3f);
            //}

            if (transform.position.y < 1f) {
                transform.position = new Vector3(transform.position.x, 1f, transform.position.z);   
            }
        }

        [Rpc(SendTo.Server)]
        void SubmitJumpServerRpc(RpcParams rpcParams = default) {
            rb.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
        }
    }
}