using UnityEngine;

public sealed class PlayerHealth :  HealthBase
{
    [SerializeField] private PlayerMovementService playerMovementService;
    [SerializeField] private PlayerCombatService playerCombatService;

    public override void Died()
    {
        playerMovementService.isManageActive = false;
        playerCombatService.isManageActive = false;
    }
    
}
