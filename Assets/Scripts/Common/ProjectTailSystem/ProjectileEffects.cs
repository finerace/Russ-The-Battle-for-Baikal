using System;
using UnityEngine;

public class ProjectileEffects : MonoBehaviour
{
    [SerializeField] protected Projectile projectileMain;

    [Space] 
    
    [SerializeField] protected ParticleSystem flyEffect;
    [SerializeField] protected ParticleSystem hitEffect;
    [SerializeField] protected MeshRenderer projectTailMesh;

    [Space] 
    
    [SerializeField] private AudioCastData onHitSound;
    
    protected void Start()
    {
        projectileMain.OnHit += OnHit;
    }

    protected virtual void OnHit()
    {
        flyEffect.Stop();
        hitEffect.Play();

        onHitSound.castPos = projectileMain.transform.position;
        AudioPoolService.audioPoolServiceInstance.CastAudio(onHitSound);
        
        projectTailMesh.enabled = false;
    }
    
}
