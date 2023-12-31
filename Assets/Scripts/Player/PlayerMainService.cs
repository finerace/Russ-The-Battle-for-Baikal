using System;
using UnityEngine;

public sealed class PlayerMainService :  HealthBase
{
    private MenuSystem menuSystem;
    private PlayerMoneyService playerMoneyService;
    [SerializeField] private PlayerMovementService playerMovementService;
    [SerializeField] private PlayerCombatService playerCombatService;
    [SerializeField] private PlayerRotationService playerRotationService;

    public Action OnReviveUse;
    
    private void Awake()
    {
        menuSystem = FindObjectOfType<MenuSystem>();
        playerMoneyService = FindObjectOfType<PlayerMoneyService>();

        var gameEvents = FindObjectOfType<GameEvents>();
        
        gameEvents.OnRoundStart += OnStartRound;
        gameEvents.OnRoundEnd += OnRoundEnd;
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
        isDie = false;
        
        menuSystem.OpenLocalMenu("GameMenu");
        
        OnReviveUse?.Invoke();
    }

    private void OnStartRound()
    {
        isDie = false;
        health = maxHealth;
    }

    private void OnRoundEnd()
    {
        var playerT = transform;
        
        playerT.position = Vector3.zero + Vector3.up;
        playerT.rotation = Quaternion.identity;
    }

    public override void Died()
    {
        SetManageActive(false);
        menuSystem.OpenLocalMenu("DieMenu");
    }
    
}
