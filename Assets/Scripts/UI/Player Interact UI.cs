using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractUI : MonoBehaviour
{
    [SerializeField] private GameObject container;
    [SerializeField] private PlayerInteract playerInteract;

    void Start()
    {
        container.SetActive(false);
    }

    void Update()
    {
        if (playerInteract.GetInteractableObject() != null)
        {
            Show();
        } else Hide();
    }

    private void Show()
    {
        container.SetActive(true);
    }

    private void Hide()
    {
        container.SetActive(false);
    }
}
