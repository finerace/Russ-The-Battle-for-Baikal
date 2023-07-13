using System;
using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] protected Transform projectTailT;
    [SerializeField] protected Transform target;
    
    [Space]
    
    [SerializeField] protected float damage;
    [SerializeField] protected float hitForce;
    [SerializeField] protected float speed;
    [SerializeField] protected float toTargetRotationSpeed;
    [SerializeField] protected float lifeTime = 6;

    public Action OnHit;
    protected bool isHit = false;

    protected void Start()
    {
        StartCoroutine(DieTimer());
        IEnumerator DieTimer()
        {
            yield return new WaitForSeconds(lifeTime);
            ProjectTailHit(gameObject);
        }
    }

    protected void Update()
    {
        if(toTargetRotationSpeed != 0)
            RotateToTarget();
        
        FlyForward();
    }

    protected virtual void FlyForward()
    {
        var timeStep = Time.deltaTime * speed;
        
        projectTailT.position += projectTailT.forward * timeStep;
    }
    
    protected virtual void RotateToTarget()
    {
        var smooth = 100;
        var timeStep = Time.deltaTime * toTargetRotationSpeed * smooth;
        var lookToTargetRotation = Quaternion.LookRotation(target.position - projectTailT.position);

        projectTailT.rotation = Quaternion.RotateTowards(projectTailT.rotation,lookToTargetRotation,timeStep);
    }

    protected virtual void ProjectTailHit(GameObject hitObject)
    {
        if(isHit)
            return;

        isHit = true;
        var smooth = 100;
        
        if (hitObject.TryGetComponent(out IHealth health))
            health.TakeDamage(damage);
        
        if(hitObject.TryGetComponent(out Rigidbody rigidbody))
            rigidbody.AddForce(projectTailT.forward * hitForce * smooth,ForceMode.Impulse);

        OnHit?.Invoke();
        
        Destroy(gameObject,3);
    }

    protected void OnCollisionEnter(Collision collision)
    {
        ProjectTailHit(collision.gameObject);
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    public void SetDamage(float damage)
    {
        this.damage = damage;
    }
}
