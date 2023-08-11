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
    private bool canReadValue;
    private bool hasReadValue;
    
    [SerializeField] private float minForwardForce, maxForwardForce, minTorque, maxTorque;
    
    private float ForwardForce => Random.Range(minForwardForce, maxForwardForce);
    private float Torque => Random.Range(minTorque, maxTorque);

    public event Action OnDiceValueAvailable; 

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

    private void Update()
    {
        if (!canReadValue) return;
        if (hasReadValue) return;
        if (!Mathf.Approximately(rigidBody.velocity.magnitude, 0f)) return;

        OnDiceValueAvailable?.Invoke();
        hasReadValue = true;
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
        
        rigidBody.AddForce(Vector3.forward * ForwardForce, ForceMode.Impulse);
        rigidBody.AddTorque(
            Vector3.forward * Torque +
            diceTransform.up * (Torque * 0.25f) +
            Vector3.right * Torque
        );

        StartCoroutine(AllowReadValue());
    }

    private void TogglePhysics()
    {
        rigidBody.isKinematic = !rigidBody.isKinematic;
        rigidBody.useGravity = !rigidBody.useGravity;
    }
    
    private void ResetPosition()
    {
        TogglePhysics();

        canReadValue = false;
        hasReadValue = false;
        diceTransform.position = startPosition;
        diceTransform.rotation = Random.rotation;
    }

    public void SetStartPosition(Vector3 position) => startPosition = position;

    public IEnumerator AllowReadValue()
    {
        yield return new WaitForSeconds(.1f);
        canReadValue = true;
    }

}
