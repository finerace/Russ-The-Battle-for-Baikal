using System;
using UnityEngine;

public class PlayerAnimationService : MonoBehaviour
{
    [SerializeField] private PlayerMovementService playerMovementService;
    [SerializeField] private PlayerCombatService playerCombatService;
    [SerializeField] private Animator playerAnimator;

    [SerializeField] private float attackAnimationDefaultSpeed = 1;
    
    [SerializeField] private float idleAnimationsDefaultSpeed = 1;
    [SerializeField] private float idleAnimationsDynamicSpeedMultiplier;

    [SerializeField] private float idleAnimationOnFlySpeed = 0.25f;
    
    private static readonly int IsAttackAnimHash = Animator.StringToHash("IsAttack");

    private void Update()
    {
        DynamicIdleAnimationsSpeedAlgorithm();
        void DynamicIdleAnimationsSpeedAlgorithm()
        {
            if (playerCombatService.IsAttack)
            {
                playerAnimator.speed = attackAnimationDefaultSpeed;
                
                return;
            }
            
            var animationSpeed = idleAnimationsDefaultSpeed;
            
            animationSpeed += playerMovementService.CurrentPlayerVelocity.LengthXZ() * idleAnimationsDynamicSpeedMultiplier;

            if (!playerMovementService.IsPlayerOnGround)
                animationSpeed = idleAnimationOnFlySpeed;
            
            playerAnimator.speed = animationSpeed;
        }
        
        SetAnimatorParameters();
        void SetAnimatorParameters()
        {
            playerAnimator.SetBool(IsAttackAnimHash,playerCombatService.IsAttack);
        }
    }
}
