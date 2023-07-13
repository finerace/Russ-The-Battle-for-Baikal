using System.Collections;
using UnityEngine;

public class PurpleLizardAttack : EnemyAttackBase
{
    [SerializeField] private Transform bulletSpawnPointT;
    [SerializeField] private GameObject bulletPrefab;

    [SerializeField] protected float preAttackTime = 0.25f;
    [SerializeField] private float postAttackWalkTime = 1.75f;

    [Space] 
    
    [SerializeField] protected ParticleSystem projectTailCastEffect;
    
    private bool isWalkAllow;
    public bool IsWalkAllow => isWalkAllow;
    
    public override IEnumerator AttackInvoker()
    {
        isAttack = true;
        isAnimationAttack = true;
        isWalkAllow = false;

        projectTailCastEffect.Play();
        
        yield return new WaitForSeconds(preAttackTime);
        
        isAnimationAttack = false;
        isWalkAllow = true;
        
        var projectTail = 
            Instantiate(bulletPrefab, bulletSpawnPointT.position, bulletSpawnPointT.rotation).GetComponent<Projectile>();
        
        projectTail.SetTarget(enemyMain.TargetT);
        projectTail.SetDamage(attackDamage);
        
        yield return new WaitForSeconds(postAttackWalkTime);
        
        isAttack = false;
    }

}
