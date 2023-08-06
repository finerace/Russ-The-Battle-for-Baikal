using System;
using UnityEngine;

public class PlayerMoneyService : MonoBehaviour
{
    [SerializeField] private int playerMoney;
    public int PlayerMoney
    {
        get => playerMoney;

        set
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException("Value of player coins can not be less than zero!");

            var oldValue = playerMoney;
            playerMoney = value;
            
            OnMoneyChange.Invoke(playerMoney - oldValue);
        }
    }

    public bool TryTakeMoney(int moneyCount)
    {
        if (playerMoney < moneyCount) 
            return false;
        
        PlayerMoney -= moneyCount;

        return true;
    }
    
    
    public event Action<int> OnMoneyChange;
}
