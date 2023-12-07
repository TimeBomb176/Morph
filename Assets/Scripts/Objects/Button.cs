using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour, IInteractable
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {


        if (this.animator.GetCurrentAnimatorStateInfo(0).IsName("push_001"))
        {
            animator.SetBool("Pushed", false);
        }
    }

    public void Interact()
    {
        animator.SetBool("Pushed", true);

        float sphereCastRadius = 5f;
        Collider[] colliderArray = Physics.OverlapSphere(this.transform.position, sphereCastRadius);
        
        foreach (Collider collider in colliderArray)
        {
            if (collider.TryGetComponent(out IButtonActivatable buttonActivatable))
            {
                buttonActivatable.ActivateWithButton();
            } else Debug.Log("None Found");
        }
       
    }
}
