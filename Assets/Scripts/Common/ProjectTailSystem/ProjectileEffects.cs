using System;
using UnityEngine;

public class ProjectileEffects : MonoBehaviour
{
    [SerializeField] protected Projectile projectileMain;

    [Space] 
    
    [SerializeField] protected ParticleSystem flyEffect;
    [SerializeField] protected ParticleSystem hitEffect;
    [SerializeField] protected MeshRenderer projectTailMesh;

    protected void Start()
    {
        projectileMain.OnHit += OnHit;
    }

    protected virtual void OnHit()
    {
        flyEffect.Stop();
        hitEffect.Play();

        projectTailMesh.enabled = false;
    }
    
}
