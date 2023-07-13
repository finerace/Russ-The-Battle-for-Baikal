using System;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    [SerializeField] private Animator enemyAnimator;

    [SerializeField] private EnemyMainBase enemyMainBase;
    [SerializeField] private EnemyAttackBase enemyAttackBase;
    
    private static readonly int IsDie = Animator.StringToHash("IsDied");
    private static readonly int IsAttack = Animator.StringToHash("IsAttack");
    private static readonly int IsRun = Animator.StringToHash("IsRun");

    private void Start()
    {
        enemyMainBase.OnDie += SetDie;
    }

    private void Update()
    {
        enemyAnimator.SetBool(IsAttack,enemyAttackBase.IsAnimationAttack);
        enemyAnimator.SetBool(IsRun,enemyMainBase.EnemyRb.velocity.magnitude > 0.25f);
    }

    private void SetDie()
    {
        enemyAnimator.SetBool(IsDie,true);
    }
}
