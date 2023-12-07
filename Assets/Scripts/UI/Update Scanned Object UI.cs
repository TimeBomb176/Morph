using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateScannedObjectUI : MonoBehaviour
{
    [SerializeField] private Image scannedObjectSprite;
    [SerializeField] private Sprite emptyImage;

    private void Start()
    {
        ScanNMorph.Instance.OnSelectedScannableSpriteChanged += ScanNMorph_OnSelectedScannableSpriteChanged;
    }

    private void ScanNMorph_OnSelectedScannableSpriteChanged(object sender, ScanNMorph.OnSelectedScannableSpriteChangedEventArgs e)
    {
        scannedObjectSprite.sprite = e.scannedObjectSprite;
    }

    private void Update()
    {
        if (scannedObjectSprite.sprite == null)
        {
            scannedObjectSprite.sprite = emptyImage;
        }
    }
}
