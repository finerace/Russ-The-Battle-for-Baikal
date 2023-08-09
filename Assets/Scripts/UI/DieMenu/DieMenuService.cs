using System;
using TMPro;
using UnityEngine;

public class DieMenuService : MonoBehaviour
{
    private RoundStats roundStats;
    [SerializeField] private TMP_Text earnedCoinsOnRound;
    [SerializeField] private GameObject reviveAd; 
    
    private void OnEnable()
    {
        if (roundStats == null)
            roundStats = FindObjectOfType<RoundStats>();

        reviveAd.SetActive(!roundStats.IsReviveUsed);

        SetLabels();
    }

    private void SetLabels()
    {
        earnedCoinsOnRound.text = roundStats.EarnedCoinsOnRound.ToString();
    }
}
