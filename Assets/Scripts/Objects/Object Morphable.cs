using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMorphable : MonoBehaviour, IMorphable
{
    [Tooltip("Which Object to return when scanning and morphing. If left empty will just return the object the script is attached to")]
    [SerializeField] GameObject returnGameObject;

    [SerializeField] private bool isMorphable = true;

    private void Awake()
    {
        if (returnGameObject == null) returnGameObject = this.gameObject;
    }

    public Transform GetTransform()
    {
        if (isMorphable)
        {
            return returnGameObject.transform;
        }

        return null;
    }
}
