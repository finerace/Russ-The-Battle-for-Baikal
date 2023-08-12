using UnityEngine;

public class LocationShopData : MonoBehaviour
{
    [SerializeField] private int id;
    [SerializeField] private string name;
    [SerializeField] private string desc;
    [SerializeField] private int cost;
    [SerializeField] private GameObject room;

    [SerializeField] private bool isSell;
    
    public int ID => id;

    public string Name => name;

    public string Desc => desc;

    public int Cost => cost;

    public GameObject Room => room;

    public bool IsSell => isSell;

    public void Buy()
    {
        isSell = true;
    }
}
