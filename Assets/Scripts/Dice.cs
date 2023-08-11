using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Random = UnityEngine.Random;

public class Dice : MonoBehaviour
{
    private Rigidbody rigidBody;
    private Vector3 startPosition;
    private DiceManager diceManager;
    private bool hasCheckedValue;
    private bool shouldCheckValue;
    private Outline outline;

    private float ForwardForce => Random.Range(minForwardForce, maxForwardForce);
    private float Torque => Random.Range(minTorque, maxTorque);

    [SerializeField] private float minForwardForce, maxForwardForce, minTorque, maxTorque;

    [field: SerializeField] public int DiceValue { get; private set; }

    public delegate void AnnounceRoll(int rollValue);
    public event AnnounceRoll OnAnnounceRoll;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        diceManager = DiceManager.Instance;
        outline = GetComponent<Outline>();
        outline.enabled = false;
    }

    private void Update()
    {
        if (!shouldCheckValue) return;
        if (hasCheckedValue && rigidBody.velocity.magnitude == 0f) return;
        if (rigidBody.velocity.magnitude > 0f)
        {
            hasCheckedValue = false;
            return;
        }
        
        CheckRoll();
    }

    public void Initialize(Vector3 position)
    {
        startPosition = position;
        transform.position = startPosition;
        transform.rotation = Random.rotation;
        diceManager.OnRollDice += RollDice;
        diceManager.OnResetDice += ResetPosition;
        diceManager.OnEnableDiceOutline += EnableOutline;
        diceManager.OnDisableDiceOutline += DisableOutline;
    }

    private void OnDisable()
    {
        diceManager.OnRollDice -= RollDice;
        diceManager.OnResetDice -= ResetPosition;
    }

    private void RollDice()
    {
        RevertPhysics(true);
        rigidBody.AddForce(Vector3.forward * ForwardForce, ForceMode.Impulse);
        rigidBody.AddTorque(
            transform.forward * Torque +
            transform.up * Torque +
            transform.right * Torque
        );

        StartCoroutine(EnableShouldCheckValue());
    }

    private IEnumerator EnableShouldCheckValue()
    {
        yield return new WaitForSeconds(.1f);
        shouldCheckValue = true;
    }

    private void RevertPhysics(bool enable)
    {
        rigidBody.isKinematic = !enable;
        rigidBody.useGravity = enable;
    }

    private void ResetPosition()
    {
        RevertPhysics(false);
        DiceValue = 0;
        transform.position = startPosition;
        transform.rotation = Random.rotation;
    }

    private void CheckRoll()
    {
        float yDot, xDot, zDot;
        int rollValue = -1;

        yDot = Mathf.Round(Vector3.Dot(transform.up.normalized, Vector3.up));
        xDot = Mathf.Round(Vector3.Dot(transform.forward.normalized, Vector3.up));
        zDot = Mathf.Round(Vector3.Dot(transform.right.normalized, Vector3.up));

        rollValue = yDot switch
        {
            1 => 3,
            -1 => 4,
            _ => rollValue
        };
        rollValue = xDot switch
        {
            1 => 5,
            -1 => 2,
            _ => rollValue
        };
        rollValue = zDot switch
        {
            1 => 6,
            -1 => 1,
            _ => rollValue
        };
        
        DiceValue = rollValue;
        shouldCheckValue = false;
        hasCheckedValue = true;
        OnAnnounceRoll?.Invoke(DiceValue);
    }

    private void EnableOutline() => outline.enabled = true;

    private void DisableOutline() => outline.enabled = false;
}