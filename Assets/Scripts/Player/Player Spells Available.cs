using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerSpellsAvailable : MonoBehaviour
{
    [Tooltip("The Amount of times the Player can morph an object")]
    [SerializeField] private int spellsAvailable = 3;

    ScanNMorph scanMorph;

    public event EventHandler OnSpellsAvailableChanged;
    public event EventHandler OnNoSpellsAvailable;

    private void Awake()
    {
        scanMorph = GetComponent<ScanNMorph>();
    }

    private void Start()
    {
        scanMorph.OnMorphObject += ScanMorph_OnMorphObject;
    }

    private void ScanMorph_OnMorphObject(object sender, System.EventArgs e)
    {
        spellsAvailable--;
        OnSpellsAvailableChanged?.Invoke(this, EventArgs.Empty);

        if (spellsAvailable <= 0)
        {
            OnNoSpellsAvailable?.Invoke(this, EventArgs.Empty);
        }
    }

    public int GetSpellsAvailable()
    {
        return spellsAvailable;
    }
}
