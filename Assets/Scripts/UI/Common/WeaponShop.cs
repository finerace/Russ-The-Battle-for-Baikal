using TMPro;
using UnityEngine;

public class WeaponShop : MonoBehaviour
{
    [SerializeField] private PlayerCombatService playerCombatService;
    [SerializeField] private PlayerMoneyService playerMoneyService;
    [SerializeField] private Transform shopLine;
    [SerializeField] private Transform startShopLinePoint;
    [SerializeField] private float shopDistance;
    
    [SerializeField] private float shopLineScrollSpeed;
    [SerializeField] private WeaponShopData[] shopItems;

    [SerializeField] private int focusedShopItemNum;
    [SerializeField] private WeaponShopData focusedShopItem;
    [SerializeField] private WeaponShopData selectedShopItem;

    [Space] 
    
    [SerializeField] private TMP_Text buyButtonLabel;
    [SerializeField] private TMP_Text weaponNameLabel;
    [SerializeField] private TMP_Text weaponCostLabel;
    
    private void Start()
    {
        SortShopItems();
        void SortShopItems()
        {
            for (int i = 0; i < shopItems.Length; i++)
            {
                var shopItemT = shopItems[i].transform;
                shopItemT.position = startShopLinePoint.position;
                shopItemT.localPosition = startShopLinePoint.right * shopDistance * i;
                
            }
        }
        
        SetStartShopItem();
        void SetStartShopItem()
        {
            focusedShopItem = shopItems[0];
            selectedShopItem = shopItems[0];
        }
        
        SetLabels();
    }

    private void Update()
    {
        MoveShopLine();
        void MoveShopLine()
        {
            var timeStep = Time.deltaTime * shopLineScrollSpeed;

            var targetMovePos = 
                startShopLinePoint.localPosition + -startShopLinePoint.right * shopDistance * focusedShopItemNum;

            shopLine.localPosition = Vector3.Lerp(shopLine.localPosition, targetMovePos, timeStep);
        }
    }

    public void GoLeft()
    {
        if (focusedShopItemNum <= 0)
            return;
        
        focusedShopItemNum--;
        
        SetData();
        SetLabels();
    }
    
    public void GoRight()
    {
        if (focusedShopItemNum >= shopItems.Length-1)
            return;
        
        focusedShopItemNum++;
        
        SetData();
        SetLabels();
    }

    private void SetData()
    {
        focusedShopItem = shopItems[focusedShopItemNum];
    }
    
    private void SetLabels()
    {
        weaponNameLabel.text = focusedShopItem.Name;
        weaponCostLabel.text = focusedShopItem.Cost.ToString();
        
        if (focusedShopItem.ID == selectedShopItem.ID)
            buyButtonLabel.text = "Выбрано";
        else if (focusedShopItem.IsSell)
            buyButtonLabel.text = "Выбрать";
        else 
            buyButtonLabel.text = "Купить";
    }

    public void BuyOrSelect()
    {
        if (focusedShopItem.IsSell)
        {
            selectedShopItem = focusedShopItem;
            SetLabels();   
            
            return;
        }

        if (playerMoneyService.TryTakeMoney(focusedShopItem.Cost))
        {
            focusedShopItem.Buy();
            selectedShopItem = focusedShopItem;
            
            SetLabels();
        }
        
        playerCombatService.SetSelectedWeapon(selectedShopItem.Weapon);
    }
}
