using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private TouchControls touchControls;
    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
        touchControls = new TouchControls();
    }

    private void PrimaryTouchPosition(InputAction.CallbackContext ctx)
    {
        var position = ctx.ReadValue<Vector2>();
        RaycastHit hit;
        Ray ray = new Ray(Utils.ScreenToWorld(mainCamera, position), mainCamera.transform.forward);

        if (!Physics.Raycast(ray, out hit, 50f)) return;
        if (!hit.transform.TryGetComponent(out IClickable clickable)) return;

        Debug.Log("Clickable hit");
        clickable.Click();
    }


    private void OnEnable()
    {
        touchControls.Touch.TouchPosition.performed += PrimaryTouchPosition;
        touchControls.Enable();
    }

    private void OnDisable()
    {
        touchControls.Touch.TouchPosition.performed -= PrimaryTouchPosition;
    }
}
