using System;
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
    
    [DllImport("__Internal")]
    public static extern void OpenAuthDialog();
    
    [DllImport("__Internal")]
    public static extern void CheckPlayerAuth();

    private MenuSystem menuSystem;

    [SerializeField] private AudioMixer mainAudio;
    [SerializeField] private GameObject authWindow;
    
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

    private int advShowCount;
    
    private void Start()
    {
        FindObjectOfType<GameEvents>().OnRoundEnd += OnRoundEndShowAdv;
        menuSystem = FindObjectOfType<MenuSystem>();

        CheckPlayerAuth();
    }

    private void OnRoundEndShowAdv()
    {
        advShowCount++;

        if (advShowCount < 3) 
            return;
        
        advShowCount = 0;

        ShowAdv();
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

    public void OpenAuthMenu()
    {
        authWindow.SetActive(true);
    }

    public void OpenAuthDialogEvent()
    {
        OpenAuthDialog();
    }
    
}
