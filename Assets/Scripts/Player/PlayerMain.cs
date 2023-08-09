using System;
using UnityEngine;

public sealed class PlayerMain :  HealthBase
{
    private MenuSystem menuSystem;
    private PlayerMoneyService playerMoneyService;
    [SerializeField] private PlayerMovementService playerMovementService;
    [SerializeField] private PlayerCombatService playerCombatService;
    [SerializeField] private PlayerRotationService playerRotationService;
    private bool isDied;

    public Action OnReviveUse;
    
    private void Awake()
    {
        menuSystem = FindObjectOfType<MenuSystem>();
        playerMoneyService = FindObjectOfType<PlayerMoneyService>();

        FindObjectOfType<GameEvents>().OnRoundStart += OnStartRound;
    }

    public void SetManageActive(bool state)
    {
        playerMovementService.isManageActive = state;
        playerCombatService.isManageActive = state;
        playerRotationService.isManageActive = state;
    }

    public void AddOneMoney()
    {
        playerMoneyService.PlayerMoney++;
    }

    public void Revive()
    {
        health = maxHealth;
        isDied = false;
        
        menuSystem.OpenLocalMenu("GameMenu");
        
        OnReviveUse?.Invoke();
    }

    private void OnStartRound()
    {
        isDied = false;
        health = maxHealth;
    }

    public override void Died()
    {
        if(isDied)
            return;
        
        isDied = true;

        SetManageActive(false);
        menuSystem.OpenLocalMenu("DieMenu");
    }
    
}
