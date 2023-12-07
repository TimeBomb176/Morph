using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMorphable : MonoBehaviour, IMorphable
{
    [SerializeField] private bool isMorphable = true;
    public Transform GetTransform()
    {
        if (isMorphable)
        {
            return gameObject.transform;
        }

        return null;
    }
}
