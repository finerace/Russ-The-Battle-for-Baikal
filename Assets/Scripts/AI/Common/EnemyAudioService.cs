using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyAudioService : MonoBehaviour
{

    [SerializeField] private EnemyMainBase enemyMainBase;
    [SerializeField] private AudioSource speakSource; 
    
    [Space] 
    
    [SerializeField] private AudioClip[] idleSpeak;
    [SerializeField] private AudioClip[] onDamageSpeak;
    [SerializeField] private AudioClip[] onAnnoyedSpeak;
    [SerializeField] private AudioClip[] specialSpeak;

    private bool isAnnoyedSpeakSpeaks;
    
    private void Awake()
    {
        enemyMainBase.OnAnnoyed += OnAnnoyed;
        enemyMainBase.OnTakeDamage += (f => {OnTakeDamage();});

        StartCoroutine(SpecialAndIdleSpeak());
    }
    
    private void OnAnnoyed()
    {
        if(isAnnoyedSpeakSpeaks || enemyMainBase.IsDied)
            return;
        
        isAnnoyedSpeakSpeaks = true;
        
        var rand = Random.Range(0, 16);
        
        if(rand >= onAnnoyedSpeak.Length)
            return;
        
        speakSource.clip = onAnnoyedSpeak[rand];
        speakSource.Play();
    }

    private void OnTakeDamage()
    {
        if(enemyMainBase.IsDied)
            return;
        
        speakSource.clip = onDamageSpeak[Random.Range(0, onDamageSpeak.Length)];
        speakSource.Play();
    }

    private IEnumerator SpecialAndIdleSpeak()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(12, 30));
            
            if(enemyMainBase.IsDied)
                yield break;
            
            if (enemyMainBase.IsAnnoyed)
            {
                if (enemyMainBase.TargetT.position.y - enemyMainBase.EnemyT.position.y > 2f)
                {
                    speakSource.clip = specialSpeak[0];
                    speakSource.Play();

                    continue;
                }
                
                speakSource.clip = specialSpeak[1];
                speakSource.Play();
                    
            }
            else
            {
                speakSource.clip = idleSpeak[Random.Range(0, idleSpeak.Length)];
                speakSource.Play();
            }
        }
    }
    
}
