using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{

    [SerializeField] private Transform cam;

    [SerializeField] private float interactDistance = 2f;
    [SerializeField] private LayerMask ignoreLayer;


    public IInteractable GetInteractableObject()
    {
        IInteractable interactable;

        Vector2 camera = new(Screen.width / 2, Screen.height / 2);
        Ray ray = Camera.main.ScreenPointToRay(camera);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, interactDistance, ~ignoreLayer, QueryTriggerInteraction.Collide))
        {
            if (hitInfo.collider != null)
            {
                interactable = hitInfo.collider.gameObject.GetComponent<IInteractable>();
                return interactable;
            } else return null;

        }

        return null;
    }
}
