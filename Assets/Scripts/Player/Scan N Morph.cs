using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ScanNMorph : MonoBehaviour
{
    public static ScanNMorph Instance { get; private set; }

    // Updates the selectable Object UI
    public event EventHandler<OnSelectedObjectVisualChangedEventArgs> OnSelectedObjectVisualChanged;
    public class OnSelectedObjectVisualChangedEventArgs : EventArgs
    {
        public GameObject gameObject;
        public ObjectScannable objectScannable;
        public ObjectMorphable objectMorphable;
    }





    // Updates the Scanned Object UI Visual
    public event EventHandler<OnSelectedScannableSpriteChangedEventArgs> OnSelectedScannableSpriteChanged;
    public class OnSelectedScannableSpriteChangedEventArgs : EventArgs
    {
        public Sprite scannedObjectSprite;
    }


    [SerializeField] private GameObject scannedObject;
    [SerializeField] private GameObject morphableObjectsParent;

    [SerializeField] private UpdateScannedObjectUI updateScannedOpjectUI;
    private GetObjectData getObjectData;

    private ObjectScannable scannableObject;
    

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
        SetSelectableObjectVisual();
    }

       /* ---------------------------------
       ---------- Scan and Morph ----------
        ---------------------------------*/

    public void SetScannedObject()
    {
        scannableObject = getObjectData.GetScannableObject();

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

    public void MorphObjectIntoScannedObject()
    {
        ObjectMorphable morphableObject = getObjectData.GetMorphableObject();

        if (morphableObject != null)
        {
            if (morphableObject.GetTransform() != null)
            {
                if (scannableObject != null)
                {
                    GameObject go = Instantiate(scannedObject, morphableObject.GetTransform().position,
                    morphableObject.GetTransform().rotation, morphableObjectsParent.transform);

                    go.name = scannedObject.name.Replace("(clone)", "").Trim();
                    scannedObject = go;

                    Destroy(morphableObject.gameObject);
                }
            }
        }
    }


    /* ------------------------------------------
      ---------- Scan, Morphable Visuals ----------
       -----------------------------------------*/


    private void SetSelectableObjectVisual()
    {
        ObjectScannable objectScannable = getObjectData.GetScannableObject();
        ObjectMorphable objectMorphable = getObjectData.GetMorphableObject();
        GameObject selectableObject = getObjectData.GetGameObject();

        if (objectScannable != null || objectMorphable != null)
        {
            SendSelectableObjectInfo(selectableObject, objectScannable, objectMorphable);
        } else SendSelectableObjectInfo(null, null, null);
    }

    // Publishes event to set correct visual outline on Scannable/Morphable.
    // All Scannable and Morphable object are subscribed. May need to Refactor if there are performance issues
    private void SendSelectableObjectInfo(GameObject gO, ObjectScannable oS, ObjectMorphable oM)
    {
        OnSelectedObjectVisualChanged?.Invoke(this, new OnSelectedObjectVisualChangedEventArgs
        {
            gameObject = gO,
            objectScannable = oS,
            objectMorphable = oM
        });

    }
}
