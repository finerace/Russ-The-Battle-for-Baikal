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
    [SerializeField] private GameObject authWindow;
    
    public void RevivePlayer()
    {
        FindObjectOfType<PlayerMainService>().Revive();
    }

    public void DoubleCoins()
    {
        if (roundsCount >= neededRoundsToShowAdv)
            roundsCount--;
        
        FindObjectOfType<DoubleReward>().AddReward();
        FindObjectOfType<GameEvents>().EndRound();
        FindObjectOfType<MenuSystem>().Back();
    }

    private int roundsCount;
    private readonly int neededRoundsToShowAdv = 1;
    
    private void Start()
    {
        FindObjectOfType<GameEvents>().OnRoundEnd += OnRoundEndShowAdv;
        menuSystem = FindObjectOfType<MenuSystem>();
    }

    private void OnRoundEndShowAdv()
    {
        roundsCount++;
        
        if (roundsCount <= neededRoundsToShowAdv) 
            return;
        
        roundsCount = 0;
        
        StartCoroutine(AdvWait());
        IEnumerator AdvWait()
        {
            yield return new WaitForSeconds(0.25f);
            ShowAdv();    
        }
    }

    public void AudioOn()
    {
        mainAudio.SetFloat("Volume", 0);
        StartCoroutine(SetTimeScale(1));
    }

    public void AudioOff()
    {
        mainAudio.SetFloat("Volume", -80);
        StartCoroutine(SetTimeScale(0));
    }

    IEnumerator SetTimeScale(float timeScale)
    {
        yield return new WaitForSecondsRealtime(0.1f);
        Time.timeScale = timeScale;
    }

    public void MenuBack()
    {
        menuSystem.OpenLocalMenu("OnRoundMenu");
    }

    public void OpenAuthMenu()
    {
        //authWindow.SetActive(true);
    }
    
}
