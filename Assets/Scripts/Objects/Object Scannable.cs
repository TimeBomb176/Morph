using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectScannable : MonoBehaviour, IScannable
{
    [SerializeField] private Sprite objectSprite = null;

    [SerializeField] private bool isScannable = true;

    public GameObject ScanObjectShape()
    {
        if (isScannable)
        {
            return this.gameObject;
        }

        return null;
    }

    public Sprite ObjectSprite()
    {
        return objectSprite;
    }
}
