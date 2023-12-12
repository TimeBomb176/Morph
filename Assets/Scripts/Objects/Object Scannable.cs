using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectScannable : MonoBehaviour, IScannable
{
    [SerializeField] private Sprite objectSprite = null;

    [Tooltip("Which Object to return when scanning and morphing. If left empty will just return the object the script is attached to")]
    [SerializeField] private GameObject returnGameObject;

    [SerializeField] private bool isScannable = true;

    private void Awake()
    {
        if (returnGameObject == null) returnGameObject = this.gameObject;
    }

    public GameObject ScanObjectShape()
    {
        if (isScannable)
        {
            return returnGameObject;
        }

        return null;
    }

    public Sprite ObjectSprite()
    {
        if (isScannable)
        {
            return objectSprite;
        }

        return null;
    }
}
