using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class MainGameMusic : MonoBehaviour
{
    [SerializeField] private AudioClip[] musicTracks;
    [SerializeField] private AudioSource musicSource;
    private int currentTrackNum;
    
    private void Awake()
    {
        StartCoroutine(PlayTracks());
    }

    private IEnumerator PlayTracks()
    {
        currentTrackNum = Random.Range(0, musicTracks.Length);
        
        musicSource.clip = musicTracks[currentTrackNum];
        musicSource.Play();
        
        var wait = new WaitForSeconds(1);
        
        while (true)
        {
            yield return wait;

            if (musicSource.isPlaying)
                continue;
            
            currentTrackNum++;
            if (currentTrackNum >= musicTracks.Length)
                currentTrackNum = 0;

            musicSource.clip = musicTracks[currentTrackNum];
            musicSource.Play();
        }
    }

}
