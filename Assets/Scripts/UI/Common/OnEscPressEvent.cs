using UnityEngine;
using UnityEngine.Events;

public class OnEscPressEvent : MonoBehaviour
{
    [SerializeField] private UnityEvent onEscPressEvent;
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            onEscPressEvent.Invoke();
        }
    }
}
