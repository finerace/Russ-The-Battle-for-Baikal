using UnityEngine;

public abstract class PlayerWeaponAttack : MonoBehaviour, IPlayerAttack
{
    public PlayerCombatService PlayerCombatService { get; set; }
    
    [SerializeField] protected float damage;
    [SerializeField] protected float attackCooldown;
    [SerializeField] protected float attackPower;
    [SerializeField] protected float attackDistance;
    
    [Space] 
    
    [SerializeField] protected LayerMask attackLayerMask;

    public float AttackCooldown { get => attackCooldown; }

    public abstract bool Attack();
}

public interface IPlayerAttack
{
    public PlayerCombatService PlayerCombatService { get; set; }
    public float AttackCooldown { get;}
    public bool Attack();
}
