using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GeneratedRoom : MonoBehaviour
{
    [SerializeField] private Transform roomCenterPoint;
    public Transform RoomCenterPoint => roomCenterPoint;

    private List<EnemyMainBase> spawnedEnemies = new List<EnemyMainBase>();

    [SerializeField] private Transform[] doors;
    public Transform[] Doors => doors;
    
    private Transform entranceDoor;
    [SerializeField] private int destroyDoorIndex;
    public int DestroyDoorIndex => destroyDoorIndex;

    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private bool isTriggeredRoom = false;
    
    public Action onPlayerEnter;
    public Action onAllEnemyDie;
    
    public void InitRoom(int entranceDoorIndex)
    {
        entranceDoor = doors[entranceDoorIndex];
        entranceDoor.gameObject.SetActive(false);
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer != 4 && isTriggeredRoom)
            return;
        
        isTriggeredRoom = true;
        
        onPlayerEnter?.Invoke();
        
        entranceDoor.gameObject.SetActive(true);
        SpawnEnemy();
    }

    public void UnlockRandomDoor()
    {
        while (true)
        {
            destroyDoorIndex = Random.Range(0, doors.Length);
            var randomDoor = doors[destroyDoorIndex];

            if (randomDoor.position != entranceDoor.position)
            {
                Destroy(randomDoor.gameObject);
                break;
            }
        }
    }

    private void SpawnEnemy()
    {
        var spawnPos = roomCenterPoint.position + Vector3.up;
        
        var enemy = 
            Instantiate(enemyPrefab, spawnPos, Quaternion.identity).GetComponent<HealthBase>();
        spawnedEnemies.Add((EnemyMainBase)enemy);
        
        enemy.OnDie += OnAllEnemyDie;
    }
    
    public int NextEntranceDoorIndex()
    {
        switch (destroyDoorIndex)
        {
            case 0:
                return 2;
            
            case 1:
                return 3;
            
            case 2:
                return 0;
            
            case 3:
                return 1; 
        }

        return -1;
    }

    private void OnAllEnemyDie()
    {
        UnlockRandomDoor();
        onAllEnemyDie?.Invoke();
    }

    public void DestroyRoom()
    {
        foreach (var enemy in spawnedEnemies)
        {
            Destroy(enemy.gameObject);
        }
        
        Destroy(gameObject);
    }
}
