using System;
using System.Collections;
using UnityEngine;

public class EnemyMainBase : HealthBase
{
    [SerializeField] protected Transform enemyT;
    public Transform EnemyT => enemyT;
    [SerializeField] protected Rigidbody enemyRb;
    public Rigidbody EnemyRb => enemyRb;
    [SerializeField] protected Transform groundCheckT;
    [SerializeField] protected EnemyAttackBase enemyAttackBase;
    [SerializeField] protected float groundCheckDistance = 0.2f;

    [Space]
    
    [SerializeField] protected float speed;
    [SerializeField] protected float rotationSpeed;
    [SerializeField] protected float fallingSpeed;
    [SerializeField] protected bool isDied;
    public bool IsDied => isDied;
    
    [Space]
    
    [SerializeField] protected float attackDistance;
    
    [Space]
    
    [SerializeField] protected float maxWalkTargetDistance = 3;
    [SerializeField] protected float annoyedDistance = 3;
    
    [Space] 
    
    [SerializeField] protected LayerMask groundLayerMask;
    protected bool isFly = false;
    protected Vector3 groundNormal;

    [Space] 
    
    [SerializeField] protected LayerMask wallLayerMask;
    [SerializeField] protected float dotFov = 0.45f;
    [SerializeField] protected bool isAnnoyed;
    public bool IsAnnoyed => isAnnoyed;
    public event Action OnAnnoyed;
    [SerializeField] protected Transform targetT;
    public Transform TargetT => targetT;

    [Space] 
    
    [SerializeField] private float onDiedDrag = 4;

    protected bool isTargetVisible;
    public bool IsTargetVisible => isTargetVisible;

    public float ToTargetDistance => Vector3.Distance(targetT.position, enemyT.position);
    
    private void Start()
    {
        targetT = MainAI.instance.MainTarget;
        
        var groundCheckCooldown = 0.15f;
        
        StartCoroutine(GroundChecker());
        IEnumerator GroundChecker()
        {
            var waitCooldown = new WaitForSeconds(groundCheckCooldown);
            
            while (true)
            {
                CheckGround();
                yield return waitCooldown;
            }
        }
        void CheckGround()
        {
            if(isDied)
                return;
            
            var ray = new Ray(groundCheckT.position,Vector3.down);

            if (Physics.Raycast(ray, out RaycastHit hitInfo, groundCheckDistance, groundLayerMask))
            {
                isFly = false;
                groundNormal = hitInfo.normal;
            }
            else
            {
                isFly = true;
                groundNormal = Vector3.up;
            }
        }

        StartCoroutine(TargetLookChecker());
        IEnumerator TargetLookChecker()
        {
            var waitCooldown = new WaitForSeconds(0.5f);
            
            while (true)
            {
                FindTarget();
                yield return waitCooldown;
            }
        }

        FindTarget();
        void FindTarget()
        {
            if(isDied)
                return;
            
            var toTargetDistance = Vector3.Distance(enemyT.position,targetT.position);

            if (toTargetDistance <= annoyedDistance)
                SetAnnoyed();
            
            var toTargetVector = (targetT.position - enemyT.position);
            var isTargetInFOV =
                Vector3.Dot(enemyT.forward, toTargetVector.normalized) >= dotFov;

            if (!isTargetInFOV)
                return;

            var wallCheckRay = new Ray(enemyT.position,toTargetVector);

            var wallCheck = Physics.Raycast(wallCheckRay,toTargetDistance,wallLayerMask);

            isTargetVisible = !wallCheck;

            if (!wallCheck)
                SetAnnoyed();
        }

        OnTakeDamage += (damage) =>  {
            SetAnnoyed();
        };
    }

    protected void SetAnnoyed()
    {
        isAnnoyed = true;
        OnAnnoyed?.Invoke();
    }
    
    private void Update()
    {
        if(isDied)
            return;

        MovementAlgorithm();
        void MovementAlgorithm()
        {
            FallAlgorithm();

            if(targetT == null || !isAnnoyed)
                return;

            RotationToTarget();

            WalkAlgorithm();
        }

        AttackCheck();
    }
    
    protected virtual void FallAlgorithm()
    {
        if(!isFly)
            return;
                    
        var smooth = 100;
        enemyRb.velocity += Vector3.down * Time.deltaTime * fallingSpeed * smooth;    
    }

    protected virtual void RotationToTarget()
    {
        var timeStep = Time.deltaTime * rotationSpeed;

        var toTargetVector = targetT.position - enemyT.position;

        var enemyRotationQ = enemyT.rotation;
                
        var enemyRotationE = enemyRotationQ.eulerAngles;
        enemyRotationE.y = 
            Quaternion.Lerp(enemyRotationQ,Quaternion.LookRotation(toTargetVector),timeStep).eulerAngles.y;
                
        enemyT.rotation = Quaternion.Euler(enemyRotationE);
    }

    protected virtual void WalkAlgorithm()
    {
        if(!IsWalkAllow())
            return;
                
        var smooth = 100;
        var walkDirectionClear = enemyT.forward;

        walkDirectionClear -= Vector3.Dot(walkDirectionClear, groundNormal.normalized) * groundNormal;
                
        var walkDirection = walkDirectionClear * speed * Time.deltaTime * smooth;
                
        enemyRb.velocity += walkDirection;
    }

    protected virtual bool IsWalkAllow()
    {
        return !((ToTargetDistance <= maxWalkTargetDistance || isTargetVisible) || enemyAttackBase.IsAttack);
    }
    
    protected virtual void AttackCheck()
    {
        if (ToTargetDistance <= attackDistance && !enemyAttackBase.IsAttack && isTargetVisible)
            enemyAttackBase.Attack();
    }

    public override void Died()
    {
        if(isDied)
            return;
        
        enemyRb.useGravity = true;
        enemyRb.drag = onDiedDrag;
        gameObject.layer = 7;
        
        isDied = true;
    }
}
