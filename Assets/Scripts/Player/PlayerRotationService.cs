using System;
using UnityEngine;

public class PlayerRotationService : MonoBehaviour
{
    [SerializeField] private Transform cameraT;
    [SerializeField] private Transform playerT;

    [Space] 
    
    [SerializeField] private float sensitivity = 1f;

    public bool isManageActive = true;
    
    private void Update()
    {
        if(!isManageActive)
            return;
        
        var sensitivitySmooth = 100f;
        var timeSenseSmooth = sensitivity; //* sensitivitySmooth;
        
        var mouseX = Input.GetAxis("Mouse X") * timeSenseSmooth;
        var mouseY = Input.GetAxis("Mouse Y") * timeSenseSmooth;

        playerT.localEulerAngles += new Vector3(0,mouseX,0);

        float lookX = cameraT.eulerAngles.x;

        if (lookX - mouseY >= 90 && lookX - mouseY <= 235)
            cameraT.localEulerAngles = new Vector3(90, 0, 0);

        else if (lookX - mouseY >= 235 && lookX - mouseY <= 270)
            cameraT.localEulerAngles = new Vector3(270, 0, 0);

        else cameraT.localEulerAngles = new Vector3(lookX - mouseY, 0, 0);
    }
    
    
}
