using UnityEngine;

public class ProjectileWeaponAttack : PlayerWeaponAttack
{
    [SerializeField] private GameObject projectilePrefab;
    
    public override void Attack()
    {
        var startPoint = PlayerCombatService.CombatPointProjectilesT;

        var projectile = 
            Instantiate(projectilePrefab, startPoint.position, startPoint.rotation).GetComponent<Projectile>();
        
        projectile.SetDamage(damage);
    }
}
