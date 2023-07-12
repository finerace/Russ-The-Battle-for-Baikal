using System.Collections;
using UnityEngine;

public sealed class YellowLizardAttack : DefaultMeleeAttack
{
    [SerializeField] private float jerkForce;
    private float jerkTimer;

    private bool isJerking;
    
    public override IEnumerator AttackInvoker()
    {
        OnAttack?.Invoke();
        
        isAttack = true;

        isJerking = true;
        isAnimationAttack = true;

        var smooth = 100;
        var toTargetDirection = enemyMain.TargetT.position - enemyMain.EnemyT.position;
        enemyMain.EnemyRb.AddForce(toTargetDirection * jerkForce * smooth, ForceMode.Impulse);
        
        yield return new WaitForSeconds(preAttackDelay);

        isJerking = false;

        yield return new WaitForSeconds(postAttackDelay/2);
        isAnimationAttack = false;
        yield return new WaitForSeconds(postAttackDelay/2);

        isAttack = false;
    }

    private void Update()
    {
        if(enemyMain.IsDied)
            return;
        
        if (isJerking && enemyMain.ToTargetDistance <= damageAllowDistance && enemyMain.IsTargetVisible)
        {
            if (enemyMain.TargetT.TryGetComponent(out IHealth health))
            {
                health.TakeDamage(attackDamage);
                isJerking = false;
            }

            if (enemyMain.TargetT.TryGetComponent(out Rigidbody rb))
                rb.AddForce(enemyMain.EnemyT.forward * damageForce,ForceMode.Impulse);
        }
    }
}

