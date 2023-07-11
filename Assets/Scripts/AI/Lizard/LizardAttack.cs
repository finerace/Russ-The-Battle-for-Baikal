using System.Collections;
using UnityEngine;

public sealed class LizardAttack : EnemyAttackBase
{
    [SerializeField] private float damageAllowDistance = 5;
    public override IEnumerator AttackInvoker()
    {
        OnAttack?.Invoke();
        
        isAttack = true;
        
        yield return new WaitForSeconds(0.3f);

        if (enemyMain.ToTargetDistance <= damageAllowDistance && enemyMain.TargetT.TryGetComponent(out IHealth health))
        {
            health.TakeDamage(attackDamage);
        }
        
        yield return new WaitForSeconds(0.5f);
        
        isAttack = false;
    }
}
