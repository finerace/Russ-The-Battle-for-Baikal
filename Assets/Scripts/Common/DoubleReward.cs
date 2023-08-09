using UnityEngine;

public class DoubleReward : MonoBehaviour
{
    private RoundStats roundStats;
    private PlayerMoneyService playerMoneyService;

    private void Awake()
    {
        roundStats = FindObjectOfType<RoundStats>();
        playerMoneyService = FindObjectOfType<PlayerMoneyService>();
    }

    public void AddReward()
    {
        playerMoneyService.PlayerMoney += roundStats.EarnedCoinsOnRound;
    }

}
