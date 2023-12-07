using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ScanNMorph : MonoBehaviour
{
    public static ScanNMorph Instance { get; private set; }


    public event EventHandler<OnSelectedScannableObjectChangedEventArgs> OnSelectedScannableObjectChanged;
    public class OnSelectedScannableObjectChangedEventArgs : EventArgs
    {
        public ObjectScannable selectedObjectScannable;
    }

    public event EventHandler<OnSelectedScannableSpriteChangedEventArgs> OnSelectedScannableSpriteChanged;
    public class OnSelectedScannableSpriteChangedEventArgs : EventArgs
    {
        public Sprite scannedObjectSprite;
    }


    [SerializeField] private GameObject scannedObject;
    [SerializeField] private GameObject morphableObjectsParent;

    [SerializeField] private UpdateScannedObjectUI updateScannedOpjectUI;
    private GetObjectData getObjectData;
    private ObjectScannable selectedObjectScannable;
    

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one ScanNMorph Instance");
        }
        Instance = this;

        getObjectData = GetComponent<GetObjectData>();
    }

    private void Update()
    {
        SelectedScannableObject();
    }

    public void SetScannedObject()
    {
        ObjectScannable scannableObject = getObjectData.GetScannableObject();

        if (scannableObject != null)
        {
            if (scannedObject != scannableObject.gameObject) // If scanned object differ from currently scanned object then set as new object
            {
                scannedObject = scannableObject.ScanObjectShape();

                OnSelectedScannableSpriteChanged?.Invoke(this, new OnSelectedScannableSpriteChangedEventArgs
                {
                    scannedObjectSprite = scannableObject.ObjectSprite()
                });
            }
        }
    }

    private void SelectedScannableObject()
    {
        ObjectScannable scannableObject = getObjectData.GetScannableObject();

        if (scannableObject != null)
        {
            SetSelectedScannableObject(getObjectData.GetScannableObject());
        } 
        else
        {
            SetSelectedScannableObject(null);
        }
    }

    public void MorphIntoScannedObject()
    {
        ObjectMorphable morphableObject = getObjectData.GetMorphableObject();

        if (morphableObject != null)
        {
            if (getObjectData.GetMorphableObject().GetTransform() != null)
            {
                GameObject go = Instantiate(scannedObject, morphableObject.GetTransform().position,
                    morphableObject.GetTransform().rotation, morphableObjectsParent.transform);

                go.name = scannedObject.name.Replace("(clone)", "").Trim();
                scannedObject = go;

                SelectedScannableObject();

                Destroy(morphableObject.gameObject);
            }
        }
    }

    private void SetSelectedScannableObject(ObjectScannable objectScannable)
    {
        selectedObjectScannable = objectScannable;

        OnSelectedScannableObjectChanged?.Invoke(this, new OnSelectedScannableObjectChangedEventArgs
        {
            selectedObjectScannable = this.selectedObjectScannable
        });
    }
}
