using System;
using System.Collections;
using UnityEngine;

public abstract class EnemyAttackBase : MonoBehaviour
{
    [SerializeField] protected EnemyMainBase enemyMain;
    [SerializeField] protected float attackDamage;
    private Coroutine currentAttack;
    
    protected bool isAttack;
    public bool IsAttack => isAttack;
    
    protected bool isAnimationAttack;
    public bool IsAnimationAttack => isAnimationAttack;

    
    public Action OnAttack;

    public void Attack()
    {
        currentAttack = StartCoroutine(AttackInvoker());
    }

    public abstract IEnumerator AttackInvoker();

    public void AttackForceStop()
    {
        StopCoroutine(currentAttack);
    }
    
}
