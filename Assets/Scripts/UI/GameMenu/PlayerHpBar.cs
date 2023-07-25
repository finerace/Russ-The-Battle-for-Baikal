using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHpBar : MonoBehaviour
{
    [SerializeField] private PlayerMain playerHealth;
    [SerializeField] private TMP_Text playerHpLabel;
    [SerializeField] private Image playerHpBar;
    [SerializeField] private TextAlphaSpawner textSpawner;
    
    [Space]
    
    [SerializeField] private float hpBarSpeed;

    private void Awake()
    {
        playerHealth = FindObjectOfType<PlayerMain>();
    }

    private void Start()
    {
        playerHealth.OnTakeDamage += (a) => {OnTakeDamage((int)a);};
        
        UpdateHpLabel();        
    }

    private void Update()
    {
        var timeStep = Time.deltaTime * hpBarSpeed;
        
        playerHpBar.fillAmount = Mathf.Lerp(playerHpBar.fillAmount,playerHealth.Health/playerHealth.MaxHealth,timeStep);
    }

    private void OnTakeDamage(int damage)
    {
        if(damage == 0)
            return;

        UpdateHpLabel();
        
        textSpawner.SpawnText($"-{damage}");
    }

    private void UpdateHpLabel()
    {
        playerHpLabel.text = $"{(int)playerHealth.Health}/{(int)playerHealth.MaxHealth}";
    }
    
}
