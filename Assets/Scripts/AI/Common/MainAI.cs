using UnityEngine;

public class MainAI : MonoBehaviour
{
    public static MainAI instance;

    [SerializeField] private Transform mainTarget;
    public Transform MainTarget => mainTarget;
    
    public void Awake()
    {
        instance = this;
    }
}
