using UnityEngine;

public class RoomsGeneration : MonoBehaviour
{
    private GameEvents gameEvents;
    [SerializeField] private GeneratedRoom firstRoom;
    [SerializeField] private Transform firstRoomDoor;

    [Space] 
    
    [SerializeField] private float aheadRoomSpawnDistance;
    [SerializeField] private float sidesRoomSpawnDistance;
    
    [Space]
    
    [SerializeField] private GeneratedRoom currentRoom;
    [SerializeField] private GeneratedRoom previousRoom;

    [Space] 
    
    [SerializeField] private GameObject roomPrefab;
    
    private void Awake()
    {
        currentRoom = firstRoom;
        gameEvents = FindObjectOfType<GameEvents>();
        
        gameEvents.OnRoundStart += OnRoundStart;
        gameEvents.OnRoundEnd += OnRoundEnd;
    }

    private void SpawnNewRoom()
    {
        var entranceDoorIndex = currentRoom.DestroyDoorIndex;

        var roomSpawnVector = CalculateRoomSpawnVector();
        Vector3 CalculateRoomSpawnVector()
        {
            switch (entranceDoorIndex)
            {
                case 0:
                    return -Vector3.forward * aheadRoomSpawnDistance;

                case 1:
                    return -Vector3.right * sidesRoomSpawnDistance;

                case 2:
                    return Vector3.forward * aheadRoomSpawnDistance;

                case 3:
                    return Vector3.right * sidesRoomSpawnDistance;
            }
            
            return Vector3.zero;
        }

        var newRoomPosition = currentRoom.RoomCenterPoint.position + roomSpawnVector;

        previousRoom = currentRoom;

        currentRoom = Instantiate(roomPrefab, newRoomPosition, Quaternion.identity).GetComponent<GeneratedRoom>();
        currentRoom.InitRoom(previousRoom.NextEntranceDoorIndex());

        currentRoom.onPlayerEnter += DestroyPreviousRoom;
        currentRoom.onAllEnemyDie += SpawnNewRoom;
    }

    private void DestroyPreviousRoom()
    {
        if(previousRoom == null)
            return;
        
        if (previousRoom == firstRoom)
        {
            firstRoom.gameObject.SetActive(false);
            return;
        }
        
        previousRoom.DestroyRoom();
    }

    private void OnRoundStart()
    {
        currentRoom = firstRoom;
        
        SpawnNewRoom();
        firstRoomDoor.gameObject.SetActive(false);
    }
    
    private void OnRoundEnd()
    {
        firstRoom.gameObject.SetActive(true);
        firstRoomDoor.gameObject.SetActive(true);
        
        if(previousRoom != null && previousRoom != firstRoom)
            previousRoom.DestroyRoom();
        
        if(currentRoom == null)
            return;
        
        if(currentRoom.RoomCenterPoint.position != firstRoom.RoomCenterPoint.position)
            currentRoom.DestroyRoom();
    }
    
}
