using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IButtonActivatable
{
    [SerializeField] private Animator animator;

    //[SerializeField] private bool isOpened = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void ActivateWithButton()
    {
        animator.SetBool("IsOpened", true);
    }

    public Transform GetTransform()
    {
        return transform;
    }
}
