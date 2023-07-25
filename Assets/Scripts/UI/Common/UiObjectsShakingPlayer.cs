using System;
using UnityEngine;

public class UiObjectsShakingPlayer : MonoBehaviour
{
    [SerializeField] private Rigidbody targetRb;
    [SerializeField] private Transform shakingObjectT;
    private Vector3 startPos;


    [Space]

    [SerializeField] private float shakingSpeed;
    [SerializeField] private float shakingPower;
    [SerializeField] private float shakingMultiplier;
    [SerializeField] private float yShakeMultiplier;
    
    private void Start()
    {
        targetRb = GameObject.Find("Player").GetComponent<Rigidbody>();
        startPos = shakingObjectT.localPosition;
    }

    private void Update()
    {
        ShakingAlgorithm();
        void ShakingAlgorithm()
        {
            var resultPos = startPos;
            var rbVelocity = targetRb.velocity;

            rbVelocity *= shakingPower;
            rbVelocity *= shakingMultiplier;

            resultPos.x += rbVelocity.x;
            resultPos.x += rbVelocity.z;

            resultPos.y += rbVelocity.y *= yShakeMultiplier;

            var timeStep = Time.deltaTime * shakingSpeed;
            shakingObjectT.localPosition = Vector3.Lerp(shakingObjectT.localPosition,resultPos,timeStep);
        }
    }
}
