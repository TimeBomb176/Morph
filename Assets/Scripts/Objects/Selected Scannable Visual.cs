using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedScannableVisual : MonoBehaviour
{

    [SerializeField] private ObjectScannable objectScannable;
    [SerializeField] private GameObject selectedObjectVisual;

    private void Awake()
    {
        Hide();
    }

    private void Start()
    {
        ScanNMorph.Instance.OnSelectedScannableObjectChanged += ScanNMorph_OnSelectedScannableObjectChanged;
    }

    private void ScanNMorph_OnSelectedScannableObjectChanged(object sender, ScanNMorph.OnSelectedScannableObjectChangedEventArgs e)
    {
        if (e.selectedObjectScannable == objectScannable)
        {
            Show();
        } else Hide();
    }

    private void Show()
    {
        if (selectedObjectVisual != null)
        {
            selectedObjectVisual.SetActive(true);
        }
    }

    private void Hide()
    {
        if (selectedObjectVisual != null)
        {
            selectedObjectVisual.SetActive(false);
        }
    }
}
