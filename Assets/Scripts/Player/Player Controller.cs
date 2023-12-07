using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.Universal;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpStrength;

    private float yVelocity;
    private float animationBlend;

    private bool isMoving = false;
    private bool isCrouching = false;

    private bool isSprinting = false;

    [SerializeField] private Transform cam;
    [SerializeField] private Transform cameraTransformObject;
    [SerializeField] private Transform objectGrabbableTransform;

    [SerializeField] LayerMask ignoreLayer;

    private PlayerInteract playerInteract;
    private CharacterController characterController;
    private PlayerInputActions playerInputActions;
    private ScanNMorph scanNMorph;

    private Vector3 defaultControllerCenter;
    private float defaultControllerHeight;
    private float defaultCamPosition;
    private float defaultMoveSpeed;

    [SerializeField] private Vector3 crouchControllerCenter;
    [SerializeField] private float crouchControllerHeight;
    [SerializeField] private float crouchCamPosition;

    [SerializeField] private float sprintSpeed;
    [SerializeField] private float maxStamina;
    [SerializeField] private float sprintStaminaThreshold;
    private float stamina;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        playerInteract = GetComponent<PlayerInteract>();
        characterController = GetComponent<CharacterController>();
        scanNMorph = GetComponent<ScanNMorph>();

        defaultCamPosition = cameraTransformObject.position.y;
        defaultControllerCenter = characterController.center;
        defaultControllerHeight = characterController.height;
        defaultMoveSpeed = moveSpeed;
        stamina = maxStamina;

        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();

        playerInputActions.Player.Jump.performed += Jump;
        playerInputActions.Player.Crouch.performed += Crouch;

        playerInputActions.Player.ScanObject.performed += ScanObject;
        playerInputActions.Player.MorphObject.performed += MorphObject;

        playerInputActions.Player.Interact.performed += Interact;

    }


    private void Update()
    {
        PlayerMovement();
        if (characterController.isGrounded)
        {
            yVelocity = -.5f;
        }

        UpdateSprintThreshold();

    }

    //Player Interactions

    private void MorphObject(InputAction.CallbackContext context)
    {
        scanNMorph.MorphIntoScannedObject();

    }

    private void ScanObject(InputAction.CallbackContext context)
    {
        scanNMorph.SetScannedObject();
    }

    private void Interact(InputAction.CallbackContext context)
    {
        IInteractable interactable = playerInteract.GetInteractableObject();

        interactable?.Interact();
    }

    //Player Movement

    private void PlayerMovement()
    {
        Vector2 inputMoveVector = playerInputActions.Player.Movement.ReadValue<Vector2>();
        //Vector2 inputLookVector = playerInputActions.Player.LookRotation.ReadValue<Vector2>();

        Vector3 move = new(inputMoveVector.x, 0f, inputMoveVector.y);
        float moveMagnitude = Mathf.Clamp01(move.magnitude);
        animationBlend = moveMagnitude;
        move.Normalize();

        // Player Object look rotation. The Virtual camera controls which way is camera.forward is
        float camRotation = Camera.main.transform.localEulerAngles.y;
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, camRotation, transform.rotation.eulerAngles.z );

        yVelocity += Physics.gravity.y * Time.deltaTime;

        if (move != Vector3.zero)
        {
            isMoving = true;
        } else
        {
            isMoving = false;
        }


        move = Quaternion.AngleAxis(cam.rotation.eulerAngles.y, Vector3.up) * move;
        move *= moveMagnitude;
        move.y = yVelocity;
        characterController.Move(moveSpeed * Time.deltaTime * move);
    }

    private void UpdateSprintThreshold()
    {
        if (isSprinting == true)
        {
            stamina -= Time.deltaTime;
            if (stamina <= 0f)
            {
                moveSpeed = defaultMoveSpeed;
                isSprinting = false;
            }
        }

        if (stamina <= maxStamina) stamina += Time.deltaTime;

    }

    private void Jump(InputAction.CallbackContext context)
    {
        if (characterController.isGrounded)
        {
            yVelocity = jumpStrength;
        }

    }

    private void Crouch(InputAction.CallbackContext context)
    {
        if (isCrouching == false)
        {
            characterController.center = crouchControllerCenter;
            characterController.height = crouchControllerHeight;
            cameraTransformObject.position = new Vector3(cameraTransformObject.position.x, crouchCamPosition, cameraTransformObject.position.z);
            moveSpeed /= 2;
            isCrouching = true;
        } else if (isCrouching == true)
        {
            characterController.center = defaultControllerCenter;
            characterController.height = defaultControllerHeight;
            cameraTransformObject.position = new Vector3(cameraTransformObject.position.x, defaultCamPosition, cameraTransformObject.position.z);
            moveSpeed = defaultMoveSpeed;
            isCrouching = false;
        }

    }

    public float MoveAnimBlend()
    {
        return animationBlend;
    }

    public bool IsWalking()
    {
        return isMoving;
    }

    public bool IsCrouching()
    {
        return isCrouching;
    }
}
