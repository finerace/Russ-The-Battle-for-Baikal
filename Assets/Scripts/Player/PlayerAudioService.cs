using UnityEngine;

public class PlayerAudioService : MonoBehaviour
{
    [SerializeField] private PlayerCombatService playerCombatService;
    [SerializeField] private PlayerMoneyService playerMoneyService;
    
    [SerializeField] private AudioSource weaponAttackAudioSource;
    [SerializeField] private AudioSource weaponHitAudioSource;

    [Space] 
    
    [SerializeField] private AudioCastData moneyAddSound;
    
    private void Start()
    {
        FindObjectOfType<GameEvents>().OnRoundStart += OnRoundStart;
        playerCombatService.onAttack += OnAttack;
        playerCombatService.onHit += OnHit;

        playerMoneyService.OnMoneyChange += (int a) =>
        {
            AudioPoolService.audioPoolServiceInstance.CastAudio(moneyAddSound);
            
        };
    }

    private void OnRoundStart()
    {
        weaponAttackAudioSource.clip = playerCombatService.WeaponData.WeaponAttackSound;
        weaponHitAudioSource.clip = playerCombatService.WeaponData.WeaponHitSound;
    }
    
    private void OnAttack()
    {
        weaponAttackAudioSource.Play();
    }

    private void OnHit()
    {
        weaponHitAudioSource.Play();
    }
}
