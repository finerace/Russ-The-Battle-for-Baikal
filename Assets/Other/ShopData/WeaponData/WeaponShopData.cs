using UnityEngine;

public class WeaponShopData : MonoBehaviour
{
    [SerializeField] private int id;
    [SerializeField] private string name;
    [SerializeField] private string desc;
    [SerializeField] private int cost;
    [SerializeField] private WeaponData weapon;

    [SerializeField] private bool isSell;
    
    public int ID => id;

    public string Name => name;

    public string Desc => desc;

    public int Cost => cost;

    public WeaponData Weapon => weapon;

    public bool IsSell => isSell;

    public void Buy()
    {
        isSell = true;
    }
}
