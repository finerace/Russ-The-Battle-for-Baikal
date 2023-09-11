using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New WeaponData", menuName = "WeaponsData", order = 51)]
public class WeaponData : ScriptableObject
{
    [SerializeField] private GameObject weapon;
    
    [SerializeField] private AudioClip weaponAttackSound;
    [SerializeField] private AudioClip weaponHitSound;
    
    public GameObject Weapon => weapon;

    public AudioClip WeaponAttackSound => weaponAttackSound;
}
