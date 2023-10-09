using System;
using UnityEngine;

public class LocationShopData : MonoBehaviour
{
    [SerializeField] private int id;
    [SerializeField] private string name;
    [SerializeField] private string desc;
    [SerializeField] private int cost;
    [SerializeField] private GameObject room;

    [SerializeField] private bool isUnlocked;
    public event Action onSell;
    
    public int ID => id;

    public string Name => name;

    public string Desc => desc;

    public int Cost => cost;

    public GameObject Room => room;

    public bool IsUnlocked => isUnlocked;

    public void Unlock()
    {
        isUnlocked = true;
        onSell?.Invoke();
    }
}
