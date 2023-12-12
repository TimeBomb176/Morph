using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedVisuals : MonoBehaviour
{
    [Tooltip("The object responsible for being detectable by the player. Should contain either Object Scannable/Morphable Script or both")]
    [SerializeField] private GameObject parentGameObject;

    [Tooltip("Spawns selectable object outlines by using the visual added. Needs to be an empty object with the visuals being the children")]
    [SerializeField] private GameObject visualGameObject;

    private const int materialArraySize = 3;
    [Header("Selectable Visuals")]
    [Tooltip("Element 0 = Scannable Object, Element 1 = Morphable Object, Element 2 = Both")]
    [SerializeField] private Material[] selectedMaterials;


    //FIX LATER: Serialized and Hidden other wise when scanning and morphing the object the values don't seem to be saved
    [SerializeField, HideInInspector] private GameObject scannableVisual;
    [SerializeField, HideInInspector] private GameObject morphableVisual;
    [SerializeField, HideInInspector] private GameObject scanMorphVisual;

    private GameObject[] gameObjectVisualsArray = new GameObject[3];
    private string[] visualNamesArray = { "ScannableObjectOutline", "MorphableObjectOutline", "ScanNMorphOutline" };

    private void OnValidate()
    {
        if (selectedMaterials.Length != materialArraySize)
        {
            Debug.LogWarning("DO NOT resize the Selected Materials array size");
            Array.Resize(ref selectedMaterials, materialArraySize);
        }

    }

    private void Awake()
    {
        if (parentGameObject == null || visualGameObject == null)
        {
            Debug.LogWarning("Parent Game Object or Visual Game Object are null on the Selected Visual Script");
            if (parentGameObject == null)
            {
                Debug.LogWarning("The Parent Game Object is needed for selecting the object. Should contain either Object Scannable/Morphable Script or both");
            }
            if (visualGameObject == null)
            {
                Debug.LogWarning("The Visual Game Object is needed to spawn in the selected visuals, Should be the model that represents the object");
            }
        }
    }

    private void Start()
    {
        if (IsSelectableVisualsEmpty())
        {
            InstantiateSelectedVisuals();
        }

        ScanNMorph.Instance.OnSelectedObjectVisualChanged += ScanNMorph_OnSelectedObjectVisualChanged;
    }


    private void InstantiateSelectedVisuals()
    {
        GameObject instantiatedObject;

        if (visualGameObject != null)
        {
            for (int i = 0; i < visualNamesArray.Length; i++)
            {
                instantiatedObject = Instantiate(visualGameObject, this.gameObject.transform);
                instantiatedObject.name = visualNamesArray[i];

                gameObjectVisualsArray[i] = instantiatedObject;
                
                instantiatedObject.SetActive(false);

                for (int j = 0; j < visualGameObject.transform.childCount; j++)
                {
                    instantiatedObject.transform.GetChild(j).GetComponent<Renderer>().material = selectedMaterials[i];
                }
            }
        }


        if (IsSelectableVisualsEmpty())
        {
            SetSelectedVisuals();
        }
    }

    private void ScanNMorph_OnSelectedObjectVisualChanged(object sender, ScanNMorph.OnSelectedObjectVisualChangedEventArgs e)
    {
        if (parentGameObject != null)
        {
            if (e.gameObject == parentGameObject)
            {
                if (e.objectScannable != null && e.objectMorphable != null)
                {
                    scanMorphVisual.SetActive(true);
                } else scanMorphVisual.SetActive(false);

                if (e.objectScannable != null && e.objectMorphable == null)
                {
                    scannableVisual.SetActive(true);
                } else scannableVisual.SetActive(false);

                if (e.objectMorphable != null && e.objectScannable == null)
                {
                    morphableVisual.SetActive(true);
                } else morphableVisual.SetActive(false);
            } else HideAll();
        }
    }

    private void HideAll()
    {
        scanMorphVisual.SetActive(false);
        scannableVisual.SetActive(false);
        morphableVisual.SetActive(false);
    }

    private void SetSelectedVisuals()
    {
        scannableVisual = gameObjectVisualsArray[0];
        morphableVisual = gameObjectVisualsArray[1];
        scanMorphVisual = gameObjectVisualsArray[2];
    }

    private bool IsSelectableVisualsEmpty()
    {
        if (scanMorphVisual == null && scannableVisual == null && morphableVisual == null)
        {
            return true;
        }

        return false;
    }

}
