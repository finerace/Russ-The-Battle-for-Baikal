using UnityEngine;

public class DungeonData : MonoBehaviour
{
    [SerializeField] private Transform[] enemiesPoss;
    public Transform[] EnemiesPoss => enemiesPoss;
}
