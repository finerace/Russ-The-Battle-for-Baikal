using System;
using UnityEngine;

public class LizardMouthMoving : MonoBehaviour
{

    [SerializeField] private Transform lizardMouthBone;
    [SerializeField] private AudioSource audioSource;
    
    [Space] 
    
    [SerializeField] private float xDefault;

    [SerializeField] private float power;
    
    private float[] spectrumData = new float[64];
    
    private void Update()
    {
        if (!audioSource.isPlaying)
            return;

        var mouthRotation = lizardMouthBone.localRotation.eulerAngles;

        audioSource.GetSpectrumData(spectrumData,0,FFTWindow.Rectangular);

        var currentVolume = Mathf.Log(spectrumData[1]);
        
        mouthRotation.x = xDefault + currentVolume * power;
        
        lizardMouthBone.localRotation = Quaternion.Euler(mouthRotation);
    }
    
}
