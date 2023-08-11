using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody))]
public class DiceControls : MonoBehaviour
{
    private DiceManager diceManager;
    private Rigidbody rigidBody;
    private Transform diceTransform;
    private Vector3 startPosition;
    
    [SerializeField] private float minForwardForce, maxForwardForce, minTorque, maxTorque;
    
    private float forwardForce => Random.Range(minForwardForce, maxForwardForce);
    private float torque => Random.Range(minTorque, maxTorque);

    private void OnEnable()
    {
        diceManager.OnResetDice += ResetPosition;
    }
    
    private void OnDisable()
    {
        diceManager.OnResetDice -= ResetPosition;
    }

    private void Awake()
    {
        InitializeControls();
    }

    private void InitializeControls()
    {
        diceManager = DiceManager.Instance;
        rigidBody = GetComponent<Rigidbody>();
        diceTransform = transform;
        
        rigidBody.isKinematic = true;
        rigidBody.useGravity = false;

        diceTransform.rotation = Random.rotation;
    }

    public void Roll()
    {
        TogglePhysics();
        
        rigidBody.AddForce(Vector3.forward * forwardForce, ForceMode.Impulse);
        rigidBody.AddTorque(
            Vector3.forward * torque +
            diceTransform.up * (torque * 0.25f) +
            Vector3.right * torque
        );
    }

    private void TogglePhysics()
    {
        rigidBody.isKinematic = !rigidBody.isKinematic;
        rigidBody.useGravity = !rigidBody.useGravity;
    }
    
    private void ResetPosition()
    {
        TogglePhysics();

        diceTransform.position = startPosition;
        diceTransform.rotation = Random.rotation;
    }

    public void SetStartPosition(Vector3 position) => startPosition = position;

}
