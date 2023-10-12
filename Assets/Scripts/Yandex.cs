using System;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Audio;

public class Yandex : MonoBehaviour
{
    
    [DllImport("__Internal")]
    public static extern void ShowAdv();
    
    [DllImport("__Internal")]
    public static extern void ShowAdvRevive();
    
    [DllImport("__Internal")]
    public static extern void ShowAdvDouble();

    private MenuSystem menuSystem;

    [SerializeField] private AudioMixer mainAudio;
    
    public void RevivePlayer()
    {
        FindObjectOfType<PlayerMainService>().Revive();
    }

    public void DoubleCoins()
    {
        FindObjectOfType<DoubleReward>().AddReward();
        FindObjectOfType<GameEvents>().EndRound();
        FindObjectOfType<MenuSystem>().Back();
    }

    private int roomSpawned;
    
    private void Start()
    {
        FindObjectOfType<RoomsGeneration>().OnNewRoomSpawn += OnNewRoomSpawnAdv;
        menuSystem = FindObjectOfType<MenuSystem>();
    }

    private void OnNewRoomSpawnAdv()
    {
        roomSpawned++;

        if (roomSpawned < 4) 
            return;
        
        roomSpawned = 0;

        StartCoroutine(Wait());
        
        IEnumerator Wait()
        {
            yield return new WaitForSeconds(1);
            
            ShowAdv();
        }
    }

    public void AudioOn()
    {
        mainAudio.SetFloat("Volume", 0);
    }

    public void AudioOff()
    {
        mainAudio.SetFloat("Volume", -80);
    }

    public void MenuBack()
    {
        menuSystem.OpenLocalMenu("OnRoundMenu");
    }
    
}
