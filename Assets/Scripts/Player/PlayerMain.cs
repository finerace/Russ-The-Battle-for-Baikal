using System;
using UnityEngine;

public sealed class PlayerMain :  HealthBase
{
    [SerializeField] private PlayerMovementService playerMovementService;
    [SerializeField] private PlayerCombatService playerCombatService;
    [SerializeField] private PlayerRotationService playerRotationService;
    
    public void SetManageActive(bool state)
    {
        playerMovementService.isManageActive = state;
        playerCombatService.isManageActive = state;
        playerRotationService.isManageActive = state;
    }
    
    public override void Died()
    {
        SetManageActive(false);
    }
    
}
