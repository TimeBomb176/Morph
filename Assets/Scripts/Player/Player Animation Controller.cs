using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerAnimationController : MonoBehaviour
{
    //Serialize 1st, Private 2nd, Public 3rd
    //Variables
    [SerializeField] private float animationBlendDampTime = .1f;

    //References

    [SerializeField] private PlayerController playerController;
    private Animator animator;

    // Start is called before the first frame update
    void Start() 
    {

        animator = GetComponent<Animator>();

    }

    void Update()
    {
        animator.transform.localPosition = Vector3.zero;
        animator.transform.localEulerAngles = Vector3.zero;
        animator.SetFloat("Blend", playerController.MoveAnimBlend(), animationBlendDampTime, Time.deltaTime);
        animator.SetBool("IsWalking", playerController.IsWalking());
        animator.SetBool("IsCrouching", playerController.IsCrouching());

    }
}
