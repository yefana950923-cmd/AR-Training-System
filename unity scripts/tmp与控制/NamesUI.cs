using TMPro;
using UnityEngine;

public class NamesUI : MonoBehaviour
{
    [Header("Status Texts")]
    public TMP_Text windowText;
    public TMP_Text blindsText;
    public TMP_Text heatingText;

    [Header("Initial Settings")]
    public int heatingLevel = 3;
    public bool windowOpen = false;
    public bool blindsOpen = true;

    private void Start()
    {
        UpdateUI();
    }

    private void Update()
    {
        // Heating: Off → Level 3 → Level 5 → Off
        if (Input.GetKeyDown(KeyCode.H))
        {
            ChangeHeating();
        }

        // Window: Closed ↔ Open
        if (Input.GetKeyDown(KeyCode.W))
        {
            windowOpen = !windowOpen;
            UpdateUI();
        }

        // Blinds: Open ↔ Closed
        if (Input.GetKeyDown(KeyCode.B))
        {
            blindsOpen = !blindsOpen;
            UpdateUI();
        }
    }

    private void ChangeHeating()
    {
        if (heatingLevel == 0)
        {
            heatingLevel = 3;
        }
        else if (heatingLevel == 3)
        {
            heatingLevel = 5;
        }
        else
        {
            heatingLevel = 0;
        }

        UpdateUI();
    }

    private void UpdateUI()
    {
        // Heating
        if (heatingLevel == 0)
        {
            heatingText.text = "Thermostatic Heating ▼\nOff";
        }
        else
        {
            heatingText.text = "Thermostatic Heating ▼\nLevel " + heatingLevel;
        }

        // Window
        if (windowOpen)
        {
            windowText.text = "Window ▶\nOpen";
        }
        else
        {
            windowText.text = "Window ▶\nClosed";
        }

        // Blinds
        if (blindsOpen)
        {
            blindsText.text = "◀ Blinds\nOpen";
        }
        else
        {
            blindsText.text = "◀ Blinds\nClosed";
        }
    }
}