using System;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public Action OnRoundStart;
    public Action OnRoundEnd;
    
    public void StartRound()
    {
        OnRoundStart?.Invoke();
    }
    public void EndRound()
    {
        OnRoundEnd?.Invoke();
    }
}