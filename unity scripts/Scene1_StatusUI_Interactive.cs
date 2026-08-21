using UnityEngine;
using TMPro;

public class Scene1_StatusUI_Interactive : MonoBehaviour
{
    [Header("UI Reference")]
    [SerializeField] private TextMeshProUGUI statusText;

    /*
     * Heating:
     * 0 = OFF
     * 3 = LEVEL 3
     * 5 = LEVEL 5
     */
    private int heatingLevel = 0;

    /*
     * Window:
     * false = CLOSED
     * true  = OPEN
     *
     * Original table:
     * Window Off = CLOSED
     * Window On  = OPEN
     */
    private bool windowOpen = false;

    /*
     * Blinds:
     * false = CLOSED
     * true  = OPEN
     *
     * Original table:
     * Blinds Off = CLOSED
     * Blinds On  = OPEN
     *
     * Initial state is OPEN, corresponding to Configuration 1.
     */
    private bool blindsOpen = true;

    private void Start()
    {
        if (statusText != null)
        {
            // Prevent automatic line wrapping.
            statusText.enableWordWrapping = false;

            // Keep the complete text visible.
            statusText.overflowMode = TextOverflowModes.Overflow;
        }

        UpdateStatusText();
    }

    private void Update()
    {
        /*
         * H:
         * Heating OFF -> LEVEL 3 -> LEVEL 5 -> OFF
         */
        if (Input.GetKeyDown(KeyCode.H))
        {
            ChangeHeatingLevel();
        }

        /*
         * W:
         * Window CLOSED <-> OPEN
         */
        if (Input.GetKeyDown(KeyCode.W))
        {
            windowOpen = !windowOpen;
            UpdateStatusText();
        }

        /*
         * B:
         * Blinds OPEN <-> CLOSED
         */
        if (Input.GetKeyDown(KeyCode.B))
        {
            blindsOpen = !blindsOpen;
            UpdateStatusText();
        }
    }

    private void ChangeHeatingLevel()
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

        UpdateStatusText();
    }

    private void UpdateStatusText()
    {
        if (statusText == null)
        {
            Debug.LogWarning(
                "Please assign the StatusText object in the Inspector."
            );

            return;
        }

        int cost = GetCost();

        string heatingText;

        if (heatingLevel == 0)
        {
            heatingText = "OFF";
        }
        else
        {
            heatingText = "LEVEL " + heatingLevel;
        }

        string windowText = windowOpen ? "OPEN" : "CLOSED";
        string blindsText = blindsOpen ? "OPEN" : "CLOSED";

        /*
         * <pos=64%> places the Controls column on the right.
         */
        statusText.text =
            "<b>Current Cost: " + cost + " € </b>" +
            "<pos=64%><b>Controls</b>\n" +

            "Heating: " + heatingText +
            "<pos=64%>[H] Heating\n" +

            "Window: " + windowText +
            "<pos=64%>[W] Window\n" +

            "Blinds: " + blindsText +
            "<pos=64%>[B] Blinds";
    }

    private int GetCost()
    {
        /*
         * HEATING OFF
         *
         * Window CLOSED + Blinds OPEN   = 0 €
         * Window OPEN   + Blinds OPEN   = 0 €
         * Window CLOSED + Blinds CLOSED = 13 €
         * Window OPEN   + Blinds CLOSED = 13 €
         */
        if (heatingLevel == 0)
        {
            if (blindsOpen)
            {
                return 0;
            }

            return 13;
        }

        /*
         * HEATING LEVEL 3
         *
         * Window CLOSED + Blinds OPEN   = 16 €
         * Window OPEN   + Blinds OPEN   = 37 €
         * Window CLOSED + Blinds CLOSED = 33 €
         * Window OPEN   + Blinds CLOSED = 54 €
         */
        if (heatingLevel == 3)
        {
            if (!windowOpen && blindsOpen)
            {
                return 16;
            }

            if (windowOpen && blindsOpen)
            {
                return 37;
            }

            if (!windowOpen && !blindsOpen)
            {
                return 33;
            }

            if (windowOpen && !blindsOpen)
            {
                return 54;
            }
        }

        /*
         * HEATING LEVEL 5
         *
         * Window CLOSED + Blinds OPEN   = 26 €
         * Window OPEN   + Blinds OPEN   = 57 €
         * Window CLOSED + Blinds CLOSED = 43 €
         * Window OPEN   + Blinds CLOSED = 74 €
         */
        if (heatingLevel == 5)
        {
            if (!windowOpen && blindsOpen)
            {
                return 26;
            }

            if (windowOpen && blindsOpen)
            {
                return 57;
            }

            if (!windowOpen && !blindsOpen)
            {
                return 43;
            }

            if (windowOpen && !blindsOpen)
            {
                return 74;
            }
        }

        return 0;
    }
}