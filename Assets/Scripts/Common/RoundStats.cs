using UnityEngine;

public class RoundStats : MonoBehaviour
{
    [SerializeField] private int earnedCoinsOnRound;
    public int EarnedCoinsOnRound => earnedCoinsOnRound;

    [SerializeField] private bool isReviveUsed;
    public bool IsReviveUsed => isReviveUsed;

    private void Start()
    {
        var gameEvents = FindObjectOfType<GameEvents>();
        gameEvents.OnRoundEnd += OnRoundEnd;

        var playerMoney = FindObjectOfType<PlayerMoneyService>();
        playerMoney.OnMoneyChange += AddEarnedCoins;
        void AddEarnedCoins(int coins)
        {
            if(coins < 0)
                return;

            earnedCoinsOnRound++;
        }

        var playerMain = FindObjectOfType<PlayerMain>();
        playerMain.OnReviveUse += () => { isReviveUsed = true;};

    }

    private void OnRoundEnd()
    {
        isReviveUsed = false;
        earnedCoinsOnRound = 0;
    }
    
}
