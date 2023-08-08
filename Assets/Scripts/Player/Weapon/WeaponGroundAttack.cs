using UnityEngine;

public class WeaponGroundAttack : PlayerWeaponAttack
{
    [SerializeField] private float attackRadius;
    [SerializeField] private ParticleSystem attackEffect;
    
    public override void Attack()
    {
        var downRay = new Ray(PlayerCombatService.CombatPointT.position, Vector3.down);

        if (!Physics.Raycast(downRay, out RaycastHit hit, 1000, attackLayerMask)) 
            return;
        
        var attackedColliders = Physics.OverlapSphere(hit.point, attackRadius, attackLayerMask);

        var particleT = attackEffect.transform; 
        particleT.position = hit.point;
        particleT.rotation = Quaternion.Euler(90,0,0);
        
        attackEffect.Play();
        
        foreach (var collider in attackedColliders)
        {
            if (collider.TryGetComponent(out IHealth health))
                health.TakeDamage(damage);

            var smooth = 100;
            if (collider.TryGetComponent(out Rigidbody rb))
                rb.AddForce(Vector3.up * attackPower * smooth,ForceMode.Impulse);
        }
        
    }
}
