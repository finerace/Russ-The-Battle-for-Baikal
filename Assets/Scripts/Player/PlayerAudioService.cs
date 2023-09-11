using UnityEngine;

public class PlayerAudioService : MonoBehaviour
{
    [SerializeField] private PlayerCombatService playerCombatService;
    [SerializeField] private AudioSource weaponAudioSource;
    
    private void Start()
    {
        FindObjectOfType<GameEvents>().OnRoundStart += OnRoundStart;
        playerCombatService.onAttack += OnAttack;
    }

    private void OnRoundStart()
    {
        weaponAudioSource.clip = playerCombatService.WeaponData.WeaponAttackSound;
    }
    
    private void OnAttack()
    {
        weaponAudioSource.Play();
    }
}
