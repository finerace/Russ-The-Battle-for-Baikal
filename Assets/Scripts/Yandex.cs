using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class Yandex : MonoBehaviour
{
    
    [DllImport("__Internal")]
    public static extern void ShowAdv();
    
    [DllImport("__Internal")]
    public static extern void ShowAdvRevive();
    
    [DllImport("__Internal")]
    public static extern void ShowAdvDouble();

    private AudioListener mainAudioListener;
    private MenuSystem menuSystem;
    
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
        mainAudioListener = FindObjectOfType<AudioListener>();
        menuSystem = FindObjectOfType<MenuSystem>();
    }

    private void OnNewRoomSpawnAdv()
    {
        roomSpawned++;

        if (roomSpawned < 4) 
            return;
        
        roomSpawned = 0;
        ShowAdv();
    }

    public void AudioOn()
    {
        print("audio ON");
        mainAudioListener.enabled = true;
    }

    public void AudioOff()
    {
        print("audio OFF");
        mainAudioListener.enabled = false;
    }

    public void MenuBack()
    {
        menuSystem.Back();
    }
    
}
