using System;
using UnityEngine;

public class PlayerCombatService : MonoBehaviour
{
    [SerializeField] private Transform combatPointT;
    
    [Space]
    
    [SerializeField] private float damage;

    [SerializeField] private float attackPower = 15f;
    
    [Space]
    
    [SerializeField] private float attackDistance = 6;
    [SerializeField] private LayerMask attackLayerMask;
    
    [SerializeField] private bool isAttack;

    [SerializeField] private float attackTime;
    [SerializeField] private float attackCooldown;
    private float cooldownTimer = 9999;

    [Space] 
    
    public bool isManageActive = true;
    
    public bool IsAttack => isAttack;
    public event Action onAttack;

    private void Update()
    {
        isAttack = cooldownTimer < attackTime;
        
        if (Input.GetKey(KeyCode.Mouse0) && cooldownTimer >= attackCooldown && isManageActive)
        {
            cooldownTimer = 0;
            
            Attack();
        }

        cooldownTimer += Time.deltaTime;
    }

    private void Attack()
    {
        var attackRay = new Ray(combatPointT.position,combatPointT.forward);

        if (Physics.Raycast(attackRay, out RaycastHit raycastHit, attackDistance, attackLayerMask))
        {
            if(raycastHit.collider.gameObject.TryGetComponent(out IHealth health))
                health.TakeDamage(damage);

            if (raycastHit.rigidbody != null)
            {
                var smooth = 100;
                raycastHit.rigidbody.AddForce(combatPointT.forward * attackPower * smooth,ForceMode.Impulse);
            }
        }
        
        onAttack?.Invoke();
    }

}
