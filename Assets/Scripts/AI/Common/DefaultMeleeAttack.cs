using System.Collections;
using UnityEngine;

public class DefaultMeleeAttack : EnemyAttackBase
{
    [SerializeField] protected float damageAllowDistance = 5;
    [SerializeField] protected float damageForce = 5;
    
    [SerializeField] protected float preAttackDelay = 0.3f;
    [SerializeField] protected float postAttackDelay = 0.5f;
    
    public override IEnumerator AttackInvoker()
    {
        OnAttack?.Invoke();
        
        isAttack = true;
        isAnimationAttack = true;
        
        yield return new WaitForSeconds(preAttackDelay);
        
        if(enemyMain.IsDied)
            yield break;
        
        if (enemyMain.ToTargetDistance <= damageAllowDistance && enemyMain.TargetT.TryGetComponent(out IHealth health))
        {
            health.TakeDamage(attackDamage);
            
            if (enemyMain.TargetT.TryGetComponent(out Rigidbody rb))
            {
                var smooth = 100;
                rb.AddForce(enemyMain.EnemyT.forward * damageForce * smooth, ForceMode.Impulse);
            }
        }

        yield return new WaitForSeconds(postAttackDelay);
        
        isAttack = false;
        isAnimationAttack = false;
    }
}
