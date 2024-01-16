using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateSwitch : MonoBehaviour, IInteractable
{
    [Tooltip("If true will only activate closest object, else will activate all objects with IButtonActivatable")]
    [SerializeField] private bool activateClosestObject = true;

    [Tooltip("Returns animation to starting point. Needs to have two animation clips(starting and ending). " +
        "The bool parameter that swaps between both in the Animator NEEDS to be called IsActivated or it won't work properly")]
    [SerializeField] private bool revertToStartingPosition = false;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    
    

    private void Update()
    {

        if (revertToStartingPosition)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).length > animator.GetCurrentAnimatorStateInfo(0).normalizedTime)
            {
                animator.SetBool("IsActivated", false);
            }
        }

    }

    // Is called directly from the PlayerInteract Script
    public void Interact()
    {
        animator.SetBool("IsActivated", true);


        float sphereCastRadius = 5f;
        Collider[] colliderArray = Physics.OverlapSphere(this.transform.position, sphereCastRadius);

        List<IButtonActivatable> buttonActivatableList = new List<IButtonActivatable>();

        foreach (Collider collider in colliderArray)
        {
            if (collider.TryGetComponent(out IButtonActivatable buttonActivatable))
            {
                buttonActivatableList.Add(buttonActivatable);
            } 
        }

        if (activateClosestObject)
        {
            if (ClosestButtonActivatableObject(buttonActivatableList) != null)
            {
                ClosestButtonActivatableObject(buttonActivatableList).ActivateWithButton();
            }
        } else
        {
            foreach (IButtonActivatable buttonActivatable in buttonActivatableList)
            {
                buttonActivatable.ActivateWithButton();
            }
        }
    }

    private IButtonActivatable ClosestButtonActivatableObject(List<IButtonActivatable> buttonActivatableList)
    {
        IButtonActivatable closestButtonActivatable = null;
        
        foreach (IButtonActivatable buttonactivatable in buttonActivatableList)
        {
            if (closestButtonActivatable == null)
            {
                closestButtonActivatable = buttonactivatable;
            } else
            {
                if (Vector3.Distance(transform.position, buttonactivatable.GetTransform().position) <
                    Vector3.Distance(transform.position, closestButtonActivatable.GetTransform().position)){

                    closestButtonActivatable = buttonactivatable;
                }
            }
        }

        return closestButtonActivatable;
    }
}
