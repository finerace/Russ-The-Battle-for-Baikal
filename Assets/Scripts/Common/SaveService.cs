using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveService : MonoBehaviour
{
    [SerializeField] private PlayerMoneyService playerMoneyService;

    [SerializeField] private WeaponShopData[] weaponShopDatas;
    [SerializeField] private LocationShopData[] locationShopDatas;
    private SavedPrefsData prefsData;

    private void Awake()
    {
        aLoadData();
        
        playerMoneyService.OnMoneyChange += (int nul) => {SavePlayerMoney(playerMoneyService.PlayerMoney); prefsData.Save();};
        void SavePlayerMoney(int money)
        {
            prefsData.SetInt("PlayerMoney",money);
        }
        
        InitSavedData();
    }
    
    private void InitSavedData()
    {
        if (prefsData.GetInt("IsFirstPlay") > 0)
            playerMoneyService.PlayerMoney = prefsData.GetInt("PlayerMoney");
        else
        {
            prefsData.SetInt("IsFirstPlay",1);
            prefsData.SetInt("PlayerMoney", playerMoneyService.PlayerMoney);
        }
        foreach (var weaponData in weaponShopDatas)
        {
            if (prefsData.GetInt($"WeaponData_{weaponData.ID}") > 0)
                weaponData.Unlock();
            else
                weaponData.onSell += () => { prefsData.SetInt($"WeaponData_{weaponData.ID}", 1);prefsData.Save();};
        }
        
        foreach (var locationData in locationShopDatas)
        {
            if(prefsData.GetInt($"LocationData_{locationData.ID}") > 0)
                locationData.Unlock();
            else
                locationData.onSell += () => { prefsData.SetInt($"LocationData_{locationData.ID}", 1);prefsData.Save();};
        }
    }

    [DllImport("__Internal")]
    private static extern void aSaveData(string data);

    [DllImport("__Internal")]
    private static extern void aLoadData();

    public void LoadDataa(string JsonData)
    {
        prefsData = new SavedPrefsData();
        prefsData.Load(JsonData);
    }
    
    public static JsonDictionary<TKey,TValue> GetJsonVersion<TKey,TValue>(Dictionary<TKey,TValue> dictionary)
    {
        var jsonDictionary = new JsonDictionary<TKey, TValue>(dictionary);

        return jsonDictionary;
    }
    
    public static void JsonToNormal<TKey,TValue> (JsonDictionary<TKey,TValue> jsonDictionary, Dictionary<TKey,TValue> normalDictionary)
    {
        normalDictionary.Clear();
        
        for (int i = 0; i < jsonDictionary.keys.Count; i++)
        {
            var jsonKey = jsonDictionary.keys[i];
            var jsonValue = jsonDictionary.values[i];
            
            normalDictionary.Add(jsonKey,jsonValue);
        }
        
    }
    
    [Serializable]
    public class JsonDictionary<TKey,TValue>
    {
        public List<TKey> keys = new List<TKey>();
        public List<TValue> values = new List<TValue>();

        public JsonDictionary(Dictionary<TKey,TValue> dictionary)
        {
            foreach (var key in dictionary.Keys)
            {
                keys.Add(key);
            }

            foreach (var value in dictionary.Values)
            {
                values.Add(value);
            }
        }
    }
    
    [Serializable]
    private class SavedPrefsData
    {
        private Dictionary<string, int> playerPrefs = new Dictionary<string, int>();
        [SerializeField] private JsonDictionary<string, int> playerPrefsJson;

        public int GetInt(string key)
        {
            if(!playerPrefs.ContainsKey(key))
                playerPrefs.Add(key,0);
            
            return playerPrefs[key];
        }

        public void SetInt(string key, int value)
        {
            if(!playerPrefs.ContainsKey(key))
                playerPrefs.Add(key,value);

            playerPrefs[key] = value;
        }

        public void Save()
        {
            playerPrefsJson = GetJsonVersion(playerPrefs);
            
            var jsonSave = 
                JsonUtility.ToJson(playerPrefsJson);

            aSaveData(jsonSave);
        }

        public void Load(string jsonData)
        {
            playerPrefsJson = new JsonDictionary<string, int>(playerPrefs);
            JsonUtility.FromJsonOverwrite(jsonData,playerPrefsJson);

            JsonToNormal(playerPrefsJson, playerPrefs);
        }
        
        
        
    }
    
}
