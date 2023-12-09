using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour, IInteractable
{
    [Tooltip("If true will only activate closest object, else will activate all objects with IButtonActivatable")]
    [SerializeField] private bool activateClosestObject = true;

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
            ClosestButtonActivatableObject(buttonActivatableList).ActivateWithButton();
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
