using System;
using TMPro;
using UnityEngine;

public class RoundStatsDisplayService : MonoBehaviour
{
    private RoundStats roundStats;
    [SerializeField] private TMP_Text earnedCoinsOnRound;
    [SerializeField] private GameObject reviveAd; 
    
    private void OnEnable()
    {
        if (roundStats == null)
            roundStats = FindObjectOfType<RoundStats>();
        
        if(reviveAd != null)
            reviveAd.SetActive(!roundStats.IsReviveUsed);

        SetLabels();
    }

    private void SetLabels()
    {
        if(earnedCoinsOnRound != null)
            earnedCoinsOnRound.text = roundStats.EarnedCoinsOnRound.ToString();
    }
}
