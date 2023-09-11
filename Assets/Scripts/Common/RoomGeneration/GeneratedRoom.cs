using System;
using System.Collections.Generic;
using System.Linq;
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

    [SerializeField] private bool isTriggeredRoom = false;

    [SerializeField] private GameObject[] dungeonsPrefabs;
    [SerializeField] private GameObject[] bigDungeonsPrefabs;
    [SerializeField] private GameObject[] enemiesPrefabs;
    private List<Transform> enemyPoss = new List<Transform>();
    private int diedEnemies;
    
    public Action onPlayerEnter;
    public Action onAllEnemyDie;

    private BoxCollider playerTrigger;
    
    private void Awake()
    {
        playerTrigger = GetComponent<BoxCollider>();
        
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

        playerTrigger.enabled = false;
        isTriggeredRoom = true;
        
        onPlayerEnter?.Invoke();
        
        entranceDoor.gameObject.SetActive(true);
        
        SpawnEnemies();
    }

    private void SpawnDungeons()
    {
        var random = Random.Range(0,25);
        
        if(random == 1)
            SpawnBigDungeon();
        else
            SpawnSmallDungeons();
        
        void SpawnBigDungeon()
        {
            enemyPoss = 
                Instantiate(bigDungeonsPrefabs[Random.Range(0, bigDungeonsPrefabs.Length)], roomCenterPoint)
                    .GetComponent<DungeonData>().EnemiesPoss.ToList();
        }
        
        void SpawnSmallDungeons()
        {
            var scale1 = new Vector3(1, 1, 1);
            var scale2 = new Vector3(-1, 1, 1);
            var scale3 = new Vector3(1, 1, -1);
            var scale4 = new Vector3(-1, 1, -1);

            DungeonData SpawnRandomDungeon()
            {
                var randomRoom = dungeonsPrefabs[Random.Range(0, dungeonsPrefabs.Length)];
                return Instantiate(randomRoom, roomCenterPoint).GetComponent<DungeonData>();
            }

            var data1 = SpawnRandomDungeon();
            var data2 = SpawnRandomDungeon();
            var data3 = SpawnRandomDungeon();
            var data4 = SpawnRandomDungeon();

            data1.transform.localScale = scale1;
            data2.transform.localScale = scale2;
            data3.transform.localScale = scale3;
            data4.transform.localScale = scale4;

            FixDungeonEnemyPos(data1);
            FixDungeonEnemyPos(data2);
            FixDungeonEnemyPos(data3);
            FixDungeonEnemyPos(data4);

            AddEnemyPoss();

            void AddEnemyPoss()
            {
                foreach (var pos in data1.EnemiesPoss)
                    enemyPoss.Add(pos);

                foreach (var pos in data2.EnemiesPoss)
                    enemyPoss.Add(pos);

                foreach (var pos in data3.EnemiesPoss)
                    enemyPoss.Add(pos);

                foreach (var pos in data4.EnemiesPoss)
                    enemyPoss.Add(pos);
            }

            void FixDungeonEnemyPos(DungeonData data)
            {
                foreach (var pos in data.EnemiesPoss)
                {
                    if (data.transform.localScale.z > 0)
                        return;

                    pos.eulerAngles = new Vector3(pos.eulerAngles.x, pos.eulerAngles.y - 180, pos.eulerAngles.z);
                }
            }
        }
    }

    private void SpawnEnemies()
    {
        GameObject GetRandomEnemy()
        {
            return enemiesPrefabs[Random.Range(0,enemiesPrefabs.Length)];
        }
        
        foreach (var pos in enemyPoss)
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
        
        if(diedEnemies >= enemyPoss.Count)
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
