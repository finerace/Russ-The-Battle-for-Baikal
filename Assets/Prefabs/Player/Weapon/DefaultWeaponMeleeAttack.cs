using UnityEngine;

public class DefaultWeaponMeleeAttack : PlayerWeaponAttack
{
    public override void Attack()
    {
        var attackPoint = PlayerCombatService.CombatPointT;
        var attackRay = new Ray(attackPoint.position,attackPoint.forward);

        if (Physics.Raycast(attackRay, out RaycastHit hit, attackDistance, attackLayerMask))
        {
            if (hit.collider.gameObject.TryGetComponent(out IHealth health))
            {
                health.TakeDamage(damage);
                PlayerCombatService.PlayEffect(PlayerCombatService.EnemyAttackEffect,hit.point);
                
                return;
            }
            
            PlayerCombatService.PlayEffect(PlayerCombatService.WallAttackEffect,hit.point);
        }
        
    }
}
