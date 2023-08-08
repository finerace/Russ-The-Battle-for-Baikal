using UnityEngine;

public class DefaultWeaponMeleeAttack : PlayerWeaponAttack
{
    public override void Attack()
    {
        var attackPoint = PlayerCombatService.CombatPointT;
        var attackRay = new Ray(attackPoint.position,attackPoint.forward);

        if (Physics.Raycast(attackRay, out RaycastHit hit, attackDistance, attackLayerMask))
        {
            var hitGameObject = hit.collider.gameObject; 
            
            if (hitGameObject.TryGetComponent(out IHealth health))
            {
                health.TakeDamage(damage);
                PlayerCombatService.PlayEffect(PlayerCombatService.EnemyAttackEffect,hit.point);

                if (hitGameObject.TryGetComponent(out Rigidbody rb))
                {
                    var smooth = 100;
                    
                    rb.AddForce(attackPoint.forward * attackPower * smooth, ForceMode.Impulse);
                }
                
                return;
            }
            
            PlayerCombatService.PlayEffect(PlayerCombatService.WallAttackEffect,hit.point);
        }
        
    }
}
