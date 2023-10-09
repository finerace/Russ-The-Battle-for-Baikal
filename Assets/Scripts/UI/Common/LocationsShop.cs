using TMPro;
using UnityEngine;

public class LocationsShop : MonoBehaviour
{
    [SerializeField] private RoomsGeneration roomGeneration;
    [SerializeField] private PlayerMoneyService playerMoneyService;
    [SerializeField] private Transform shopLine;
    [SerializeField] private Transform startShopLinePoint;
    [SerializeField] private float shopDistance;
    
    [SerializeField] private float shopLineScrollSpeed;
    [SerializeField] private LocationShopData[] shopItems;

    [SerializeField] private int focusedShopItemNum;
    [SerializeField] private LocationShopData focusedShopItem;
    [SerializeField] private LocationShopData selectedShopItem;

    [Space] 
    
    [SerializeField] private TMP_Text buyButtonLabel;
    [SerializeField] private TMP_Text locationNameLabel;
    [SerializeField] private TMP_Text locationDescLabel;
    [SerializeField] private TMP_Text locationCostLabel;
    
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
        locationNameLabel.text = focusedShopItem.Name;
        locationDescLabel.text = focusedShopItem.Desc;
        locationCostLabel.text = focusedShopItem.Cost.ToString();
        
        if (focusedShopItem.ID == selectedShopItem.ID)
            buyButtonLabel.text = "Выбрано";
        else if (focusedShopItem.IsUnlocked)
            buyButtonLabel.text = "Выбрать";
        else 
            buyButtonLabel.text = "Открыть";
    }

    public void BuyOrSelect()
    {
        if (focusedShopItem.IsUnlocked)
        {
            selectedShopItem = focusedShopItem;
        }
        else if (playerMoneyService.TryTakeMoney(focusedShopItem.Cost))
        {
            focusedShopItem.Unlock();
            selectedShopItem = focusedShopItem;
        }
        
        roomGeneration.SetNewRoom(selectedShopItem.Room);
        SetLabels(); 
    }
}
