using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScanNMorphIndicatorUI : MonoBehaviour
{
    [SerializeField] private GameObject scannableSpriteContainer;
    [SerializeField] private GameObject morphableSpriteContainer;


    private void Awake()
    {
        HideAll();
    }
    private void Start()
    {
        ScanNMorph.Instance.OnSelectedObjectVisualChanged += ScanNMorph_OnSelectedObjectVisualChanged;
    }

    private void ScanNMorph_OnSelectedObjectVisualChanged(object sender, ScanNMorph.OnSelectedObjectVisualChangedEventArgs e)
    {
        if (e.objectScannable != null && e.objectMorphable != null)
        {
            scannableSpriteContainer.SetActive(true);
            morphableSpriteContainer.SetActive(true);

        } else if (e.objectScannable != null && e.objectMorphable == null)
        {
            scannableSpriteContainer.SetActive(true);
            morphableSpriteContainer.SetActive(false);
        } else if (e.objectMorphable != null && e.objectScannable == null)
        {
            morphableSpriteContainer.SetActive(true);
            scannableSpriteContainer.SetActive(false);
        } else HideAll();
    }

    private void HideAll()
    {
        scannableSpriteContainer.SetActive(false);
        morphableSpriteContainer.SetActive(false);
    }
}
