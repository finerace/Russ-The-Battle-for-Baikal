using UnityEngine;

public class SaveService : MonoBehaviour
{
    [SerializeField] private PlayerMoneyService playerMoneyService;

    [SerializeField] private WeaponShopData[] weaponShopDatas;
    [SerializeField] private LocationShopData[] locationShopDatas;

    private void Awake()
    {
        playerMoneyService.OnMoneyChange += (int nul) => {SavePlayerMoney(playerMoneyService.PlayerMoney); PlayerPrefs.Save();};
        void SavePlayerMoney(int money)
        {
            PlayerPrefs.SetInt("PlayerMoney",money);
        }
        
        InitSavedData();
    }
    
    private void InitSavedData()
    {
        if (PlayerPrefs.GetInt("IsFirstPlay") > 0)
            playerMoneyService.PlayerMoney = PlayerPrefs.GetInt("PlayerMoney");
        else
        {
            PlayerPrefs.SetInt("IsFirstPlay",1);
            PlayerPrefs.SetInt("PlayerMoney", playerMoneyService.PlayerMoney);
        }
        foreach (var weaponData in weaponShopDatas)
        {
            if (PlayerPrefs.GetInt($"WeaponData_{weaponData.ID}") > 0)
                weaponData.Unlock();
            else
                weaponData.onSell += () => { PlayerPrefs.SetInt($"WeaponData_{weaponData.ID}", 1);PlayerPrefs.Save();};
        }
        
        foreach (var locationData in locationShopDatas)
        {
            if(PlayerPrefs.GetInt($"LocationData_{locationData.ID}") > 0)
                locationData.Unlock();
            else
                locationData.onSell += () => { PlayerPrefs.SetInt($"LocationData_{locationData.ID}", 1);PlayerPrefs.Save();};
        }
    }
    
}
