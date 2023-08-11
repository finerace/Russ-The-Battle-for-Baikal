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

    [SerializeField] private GameObject[] dungeonsPrefabs;
    [SerializeField] private GameObject[] enemiesPrefabs;
    private List<Transform> enemiesPoss = new List<Transform>();
    private int diedEnemies;
    
    public Action onPlayerEnter;
    public Action onAllEnemyDie;

    private void Awake()
    {
        if(!isTriggeredRoom)
            SpawnDungeons();
    }

    public void InitRoom(int entranceDoorIndex)
    {
        entranceDoor = doors[entranceDoorIndex];
        entranceDoor.gameObject.SetActive(false);
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer != LayerMask.NameToLayer("Player") || isTriggeredRoom)
            return;
        
        isTriggeredRoom = true;
        
        onPlayerEnter?.Invoke();
        
        entranceDoor.gameObject.SetActive(true);
        
        SpawnEnemies();
    }

    private void SpawnDungeons()
    {
        var scale1 = new Vector3(1, 1, 1);
        var scale2 = new Vector3(-1, 1, 1);
        var scale3 = new Vector3(1, 1, -1);
        var scale4 = new Vector3(-1, 1, -1);

        DungeonData SpawnRandomDungeon()
        {
            var randomRoom = dungeonsPrefabs[Random.Range(0, dungeonsPrefabs.Length)];
            return Instantiate(randomRoom,roomCenterPoint).GetComponent<DungeonData>();
        }

        var data1 = SpawnRandomDungeon();
        var data2 = SpawnRandomDungeon();
        var data3 = SpawnRandomDungeon();
        var data4 = SpawnRandomDungeon();
        
        data1.transform.localScale = scale1;
        data2.transform.localScale = scale2;
        data3.transform.localScale = scale3;
        data4.transform.localScale = scale4;

        AddEnemyPoss();
        void AddEnemyPoss()
        {
            foreach (var pos in data1.EnemiesPoss)
                enemiesPoss.Add(pos);

            foreach (var pos in data2.EnemiesPoss)
                enemiesPoss.Add(pos);

            foreach (var pos in data3.EnemiesPoss)
                enemiesPoss.Add(pos);

            foreach (var pos in data4.EnemiesPoss)
                enemiesPoss.Add(pos);
        }
    }

    private void SpawnEnemies()
    {
        GameObject GetRandomEnemy()
        {
            return enemiesPrefabs[Random.Range(0,enemiesPrefabs.Length)];
        }
        
        foreach (var pos in enemiesPoss)
        {
            var enemyBase = 
                Instantiate(GetRandomEnemy(),pos.position,pos.rotation).GetComponent<EnemyMainBase>();
            
            enemyBase.OnDie += AddDiedEnemy;
            spawnedEnemies.Add(enemyBase);
        }
    }

    private void AddDiedEnemy()
    {
        diedEnemies++;
        
        if(diedEnemies >= enemiesPoss.Count)
            OnAllEnemyDie();
        
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
