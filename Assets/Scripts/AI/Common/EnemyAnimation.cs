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
        enemyMainBase.OnAnnoyed += SetRun;
    }

    private void Update()
    {
        enemyAnimator.SetBool(IsAttack,enemyAttackBase.IsAttack);
    }

    private void SetDie()
    {
        enemyAnimator.SetBool(IsDie,true);
    }

    private void SetRun()
    {
        enemyAnimator.SetBool(IsRun,true);
    }
    
}
