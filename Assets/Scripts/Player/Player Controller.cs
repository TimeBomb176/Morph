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
    [SerializeField] private float crouchMoveSpeed;
    [SerializeField] private float jumpStrength;

    private float yVelocity;
    private float animationBlend;

    private bool isMoving = false;
    private bool isCrouching = false;


    [SerializeField] private Transform cam;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Transform crouchCamTransform;
    [SerializeField] private Transform defaultCamTransform;

    private PlayerInteract playerInteract;
    private CharacterController characterController;
    private PlayerInputActions playerInputActions;
    private ScanNMorph scanNMorph;

    private Vector3 defaultControllerCenter;
    private float defaultControllerHeight;
    private float defaultMoveSpeed;

    [Header("Character Controller collider Crouch size")]
    [Tooltip("Sets the center point of the character controller so it isn't on the floor")]
    [SerializeField] private Vector3 crouchControllerCenter;
    private float crouchControllerHeight = 1;

    private void Awake()
    {

        playerInteract = GetComponent<PlayerInteract>();
        characterController = GetComponent<CharacterController>();
        scanNMorph = GetComponent<ScanNMorph>();

        defaultControllerCenter = characterController.center;
        defaultControllerHeight = characterController.height;

        defaultMoveSpeed = moveSpeed;

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
    }

    //Player Interactions

    private void MorphObject(InputAction.CallbackContext context)
    {
        scanNMorph.MorphObjectIntoScannedObject();

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
            CrouchMovement();
        } else if (isCrouching == true)
        {
            RegularMovement();
        }
    }


    private void CrouchMovement()
    {
        characterController.height = crouchControllerHeight;
        characterController.center = crouchControllerCenter;

        cameraTransform.position = crouchCamTransform.position;
        moveSpeed = crouchMoveSpeed;
        isCrouching = true;
    }

    private void RegularMovement()
    {
        characterController.height = defaultControllerHeight;
        characterController.center = defaultControllerCenter;

        cameraTransform.position = defaultCamTransform.position;
        moveSpeed = defaultMoveSpeed;
        isCrouching = false;
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
