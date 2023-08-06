using TMPro;
using UnityEngine;

public class MoneyLabel : MonoBehaviour
{
    private PlayerMoneyService playerMoneyService;
    [SerializeField] private TMP_Text moneyLabel;

    private void Awake()
    {
        playerMoneyService = FindObjectOfType<PlayerMoneyService>();
        playerMoneyService.OnMoneyChange += no => {UpdateMoneyLabel();} ;
        
        UpdateMoneyLabel();
    }

    private void UpdateMoneyLabel()
    {
        moneyLabel.text = playerMoneyService.PlayerMoney.ToString();
    }
    
}
