using UnityEngine;

public class PurpleLizardMain : EnemyMainBase
{
    [SerializeField] private PurpleLizardAttack purpleLizardAttack;
    
    protected override bool IsWalkAllow()
    {
        return (ToTargetDistance > maxWalkTargetDistance || !isTargetVisible) && purpleLizardAttack.IsWalkAllow;
    }
}
