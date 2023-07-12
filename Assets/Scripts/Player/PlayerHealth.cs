using System;
using UnityEngine;

public sealed class PlayerHealth :  HealthBase
{
    [SerializeField] private PlayerMovementService playerMovementService;
    [SerializeField] private PlayerCombatService playerCombatService;

    private void Start()
    {
        OnTakeDamage += PrintHealth;
        void PrintHealth(float damage)
        {
            print(health);
        }
    }

    public override void Died()
    {
        playerMovementService.isManageActive = false;
        playerCombatService.isManageActive = false;
    }
    
}
