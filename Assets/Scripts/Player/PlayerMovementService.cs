using System;
using UnityEngine;

public class PlayerMovementService : MonoBehaviour
{
    [SerializeField] private Transform playerT;
    [SerializeField] private Rigidbody playerRb;

    [Space] 
    
    [SerializeField] private float playerSpeed = 1f;
    [SerializeField] private float playerJumpForce = 1f;
    [SerializeField] private float fallSpeed;
    [SerializeField] private float fallVelocitySpeed = 3;
    
    [Space]
    
    [SerializeField] private Transform onGroundCheckT;
    [SerializeField] private LayerMask groundLayer;
    
    [SerializeField] private bool isPlayerOnGround;
    [SerializeField] private float groundCheckDistance = 0.025f;
    
    [SerializeField] private float groundIgnoreTime = 0.5f;
    private bool groundIsIgnored;
    private float groundIgnoreTimer;
    
    private Vector3 groundNormal;

    private float fallTimer;

    public Vector3 CurrentPlayerVelocity => playerRb.velocity;

    public bool IsPlayerOnGround => isPlayerOnGround;

    private void Update()
    {
        PlayerGroundCheckAlgorithm();
        void PlayerGroundCheckAlgorithm()
        {
            var checkGroundRay = new Ray(onGroundCheckT.position,onGroundCheckT.forward);

            isPlayerOnGround = Physics.Raycast(checkGroundRay, out var hitInfo, groundCheckDistance, groundLayer);

            groundNormal = isPlayerOnGround ? hitInfo.normal : Vector3.up;

            if (!isPlayerOnGround)
                groundIgnoreTimer += Time.deltaTime;
            else
            {
                groundIgnoreTimer = 0;
                groundIsIgnored = false;
                
                return;
            }
            
            groundIsIgnored = groundIgnoreTimer <= groundIgnoreTime;
        }
        
        MoveAlgorithm();
        void MoveAlgorithm()
        {
            var playerMoveVector = Vector3.zero;

            playerMoveVector = GetPlayerWalkVector();

            Vector3 GetPlayerWalkVector()
            {
                var playerMoveVector = Vector3.zero;

                if (Input.GetKey(KeyCode.W))
                    playerMoveVector += playerT.forward;
                else if (Input.GetKey(KeyCode.S))
                    playerMoveVector += -playerT.forward;

                if (Input.GetKey(KeyCode.A))
                    playerMoveVector += -playerT.right;
                else if (Input.GetKey(KeyCode.D))
                    playerMoveVector += playerT.right;

                if (playerMoveVector.magnitude > 1f)
                    playerMoveVector /= 1.5f;

                return playerMoveVector;
            }

            playerMoveVector -= Vector3.Dot(playerMoveVector, groundNormal) * groundNormal;

            var smoothness = 50f;
            playerMoveVector *= smoothness;
            playerMoveVector *= playerSpeed;

            playerRb.velocity += playerMoveVector * Time.deltaTime;
        }

        JumpAlgorithm();
        void JumpAlgorithm()
        {
            if (Input.GetKeyDown(KeyCode.Space) && (isPlayerOnGround || groundIsIgnored))
            {
                playerT.position += Vector3.up * (groundCheckDistance + 0.005f);

                var smooth = 10f;
                
                playerRb.velocity += Vector3.up * playerJumpForce * smooth;

                groundIgnoreTimer = groundIgnoreTime + 1;
            }
        }

        FallAlgorithm();
        void FallAlgorithm()
        {
            if (isPlayerOnGround)
            {
                fallTimer = 0;
                return;
            }

            fallTimer += Time.deltaTime * fallVelocitySpeed;
            
            var smooth = 100;
            playerRb.velocity += -Vector3.up * fallSpeed * Time.deltaTime * smooth * fallTimer;
        }
    }

}
