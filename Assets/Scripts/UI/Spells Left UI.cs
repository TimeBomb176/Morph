using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpellsLeftUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI spellsLeftNumber;

    private PlayerSpellsAvailable spellsAvailable;
    private GameObject player;
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        spellsAvailable = player.GetComponent<PlayerSpellsAvailable>();

    }

    private void Start()
    {
        UpdateSpellsUI();

        spellsAvailable.OnSpellsAvailableChanged += SpellsAvailable_OnSpellsAvailableChanged;
    }

    private void SpellsAvailable_OnSpellsAvailableChanged(object sender, System.EventArgs e)
    {
        UpdateSpellsUI();
    }

    private void UpdateSpellsUI()
    {
        spellsLeftNumber.text = spellsAvailable.GetSpellsAvailable().ToString();
    }
}
