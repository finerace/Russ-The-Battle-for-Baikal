using System;
using UnityEngine;

public class PlayerCombatService : MonoBehaviour
{
    [SerializeField] private float damage;
    
    [SerializeField] private bool isAttack;

    [SerializeField] private float attackTime;
    [SerializeField] private float attackCooldown;
    private float cooldownTimer = 9999;


    public bool IsAttack => isAttack;
    public event Action onAttack;

    private void Update()
    {
        isAttack = cooldownTimer < attackTime;
        
        if (Input.GetKey(KeyCode.Mouse0) && cooldownTimer >= attackCooldown)
        {
            cooldownTimer = 0;
            
            Attack();
        }

        cooldownTimer += Time.deltaTime;
    }

    private void Attack()
    {
        onAttack?.Invoke();
    }

}
