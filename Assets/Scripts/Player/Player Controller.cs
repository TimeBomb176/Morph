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
    private bool canMorph = true;


    [SerializeField] private Transform cam;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Transform crouchCamTransform;
    [SerializeField] private Transform defaultCamTransform;

    private PlayerInteract playerInteract;
    private CharacterController characterController;
    private PlayerInputActions playerInputActions;
    private ScanNMorph scanNMorph;
    private PlayerSpellsAvailable playerSpellsAvailableScript;

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
        playerSpellsAvailableScript = GetComponent<PlayerSpellsAvailable>();

        defaultControllerCenter = characterController.center;
        defaultControllerHeight = characterController.height;

        defaultMoveSpeed = moveSpeed;

    }

    private void Start()
    {
        GameInput.Instance.OnJumpAction += Jump;
        GameInput.Instance.OnInteractAction += Interact;
        GameInput.Instance.OnCrouchAction += Crouch;
        GameInput.Instance.OnScanObjectAction += ScanObject;
        GameInput.Instance.OnMorphObjectAction += MorphObject;

        GameInput.Instance.OnDisableMorphAction += GameInput_OnDisableMorphAction;

        playerSpellsAvailableScript.OnNoSpellsAvailable += PlayerSpellsAvailable_OnNoSpellsAvaliable;
    }

    private void PlayerSpellsAvailable_OnNoSpellsAvaliable(object sender, EventArgs e)
    {
        canMorph = false;
    }

    private void GameInput_OnDisableMorphAction(object sender, EventArgs e)
    {
        //canMorph = !canMorph;
        //Debug.Log(canMorph);
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

    private void MorphObject(object sender, EventArgs e)
    {
        if (canMorph)
        {
            scanNMorph.MorphObjectIntoScannedObject();
        }

    }

    private void ScanObject(object sender, EventArgs e)
    {
        scanNMorph.SetScannedObject();
    }

    private void Interact(object sender, EventArgs e)
    {
        IInteractable interactable = playerInteract.GetInteractableObject();

        interactable?.Interact();
    }

    //Player Movement

    private void PlayerMovement()
    {
        //Vector2 inputLookVector = playerInputActions.Player.LookRotation.ReadValue<Vector2>();
        Vector2 inputMoveVector = GameInput.Instance.GetMovementVectorNormalized();

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

    private void Jump(object sender, EventArgs e)
    {
        if (characterController.isGrounded)
        {
            yVelocity = jumpStrength;
        }

    }

    private void Crouch(object sender, EventArgs e)
    {
        isCrouching = !isCrouching;

        if (isCrouching == true)
        {
            CrouchMovement();
        } else RegularMovement();
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
