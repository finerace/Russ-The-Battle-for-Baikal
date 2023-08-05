using System;
using UnityEngine;

public sealed class PlayerCombatService : MonoBehaviour,IOnRoundStart,IOnRoundEnd
{
    [SerializeField] private Transform combatPointT;
    public Transform CombatPointT => combatPointT;
    
    [Space] 
    
    [SerializeField] private Transform weaponSpawnPoint;
    [SerializeField] private WeaponData weaponPrefab;
    private GameObject weapon;
    
    private IPlayerAttack playerAttack;
    
    [Space]
    
    [SerializeField] private bool isAttack;
    private float attackTime;
    private float attackCooldown;
    private float cooldownTimer = 9999;

    [Space]
    
    [SerializeField] private ParticleSystem wallAttackEffect;
    [SerializeField] private ParticleSystem enemyAttackEffect;

    public ParticleSystem WallAttackEffect => wallAttackEffect;
    public ParticleSystem EnemyAttackEffect => enemyAttackEffect;
    
    [Space]
    
    public bool isManageActive = true;
    
    public bool IsAttack => isAttack;
    public event Action onAttack;

    private void Start()
    {
        OnRoundStart();
    }

    private void Update()
    {
        isAttack = cooldownTimer < attackTime;
        
        if (Input.GetKey(KeyCode.Mouse0) && !isAttack)
        {
            cooldownTimer = 0;
            
            playerAttack.Attack();
        }

        cooldownTimer += Time.deltaTime;
    }

    public void OnRoundStart()
    {
        weapon = Instantiate(weaponPrefab.Weapon, weaponSpawnPoint);
        playerAttack = weapon.GetComponent<IPlayerAttack>();
        playerAttack.PlayerCombatService = this;
        
        CalculateCooldown();
        void CalculateCooldown()
        {
            attackCooldown = playerAttack.AttackCooldown;

            if (attackCooldown < 0.1)
                throw new ArgumentException("Attack cooldown is too small!");

            attackTime = attackCooldown - 0.01f;
        }
    }

    public void OnRoundEnd()
    {
        Destroy(weapon);
        playerAttack = null;
    }

    public void PlayEffect(ParticleSystem effect,Vector3 pos)
    {
        effect.transform.position = pos;
        //effect.transform.localRotation = Quaternion.Euler(-combatPointT.forward);
        
        effect.Play();
    }
}
