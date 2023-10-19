using System;
using UnityEngine;

public class DoubleCoindButtonVisibilityService : MonoBehaviour
{
    [SerializeField] private GameObject doubleCoinsButton;

    private void Start()
    {
        doubleCoinsButton.SetActive(FindObjectOfType<RoundStats>().EarnedCoinsOnRound > 0);

        FindObjectOfType<GameEvents>().OnRoundStart += () => {doubleCoinsButton.SetActive(false);};
        FindObjectOfType<PlayerMoneyService>().OnMoneyChange += (int coins) => {doubleCoinsButton.SetActive(true);};
    }

}
