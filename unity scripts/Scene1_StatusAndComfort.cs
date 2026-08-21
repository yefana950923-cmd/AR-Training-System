using UnityEngine;
using TMPro;

public class Scene1_StatusAndComfort : MonoBehaviour
{
    [Header("UI Reference")]
    [SerializeField] private TextMeshProUGUI statusText;

    // Heating:
    // 0 = OFF
    // 3 = LEVEL 3
    // 5 = LEVEL 5
    private int heatingLevel = 0;

    // Window:
    // false = CLOSED
    // true  = OPEN
    private bool windowOpen = false;

    // Blinds:
    // false = CLOSED
    // true  = OPEN
    private bool blindsOpen = true;

    // Once Return is pressed, H/W/B can no longer change the selection.
    private bool isLocked = false;

    private enum ComfortState
    {
        Smile,
        Normal,
        Cry
    }

    private void Start()
    {
        if (statusText != null)
        {
            statusText.richText = true;
            statusText.enableWordWrapping = false;
            statusText.overflowMode = TextOverflowModes.Overflow;
        }

        UpdateStatusText();
    }

    private void Update()
    {
        /*
         * Return:
         * Lock the current selection permanently.
         */
        if (!isLocked &&
            (Input.GetKeyDown(KeyCode.Return) ||
             Input.GetKeyDown(KeyCode.KeypadEnter)))
        {
            LockSelection();
            return;
        }

        // Ignore H, W and B after the selection has been locked.
        if (isLocked)
        {
            return;
        }

        // H: OFF -> LEVEL 3 -> LEVEL 5 -> OFF
        if (Input.GetKeyDown(KeyCode.H))
        {
            ChangeHeatingLevel();
        }

        // W: CLOSED <-> OPEN
        if (Input.GetKeyDown(KeyCode.W))
        {
            windowOpen = !windowOpen;
            UpdateStatusText();
        }

        // B: OPEN <-> CLOSED
        if (Input.GetKeyDown(KeyCode.B))
        {
            blindsOpen = !blindsOpen;
            UpdateStatusText();
        }
    }

    private void LockSelection()
    {
        isLocked = true;

        string heatingStatus =
            heatingLevel == 0
                ? "OFF"
                : "LEVEL " + heatingLevel;

        string windowStatus =
            windowOpen ? "OPEN" : "CLOSED";

        string blindsStatus =
            blindsOpen ? "OPEN" : "CLOSED";

        Debug.Log(
            "Selection locked | " +
            "Heating: " + heatingStatus +
            " | Window: " + windowStatus +
            " | Blinds: " + blindsStatus +
            " | Cost: " + GetCost() + " € / month"
        );
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

        ComfortState thermalComfort = GetThermalComfort();
        ComfortState airQuality = GetAirQuality();

        string heatingOffText =
            FormatOption("OFF", heatingLevel == 0);

        string heatingLevel3Text =
            FormatOption("LEVEL 3", heatingLevel == 3);

        string heatingLevel5Text =
            FormatOption("LEVEL 5", heatingLevel == 5);

        string windowClosedText =
            FormatOption("CLOSED", !windowOpen);

        string windowOpenText =
            FormatOption("OPEN", windowOpen);

        string blindsClosedText =
            FormatOption("CLOSED", !blindsOpen);

        string blindsOpenText =
            FormatOption("OPEN", blindsOpen);

        string thermalComfortIcons =
            FormatComfortOptions(thermalComfort);

        string airQualityIcons =
            FormatComfortOptions(airQuality);

        statusText.text =
            "<b>Current Cost: " + cost + " € / month</b>\n\n" +

            "Heating:" +
            "<pos=14%>" + heatingOffText +
            "<pos=24%>" + heatingLevel3Text +
            "<pos=38%>" + heatingLevel5Text +
            "<pos=62%><b>Thermal Comfort:</b> " +
            thermalComfortIcons + "\n" +

            "Window:" +
            "<pos=14%>" + windowClosedText +
            "<pos=30%>" + windowOpenText +
            "<pos=62%><b>Air Quality:</b> " +
            airQualityIcons + "\n" +

            "Blinds:" +
            "<pos=14%>" + blindsClosedText +
            "<pos=30%>" + blindsOpenText;
    }

    private string FormatOption(string optionText, bool isActive)
    {
        if (isActive)
        {
            return
                "<color=#FFFFFFFF><b>" +
                optionText +
                "</b></color>";
        }

        return
            "<color=#FFFFFF45>" +
            optionText +
            "</color>";
    }

    private string FormatComfortOptions(ComfortState activeState)
    {
        string smile = FormatComfortSymbol(
            "🙂",
            activeState == ComfortState.Smile,
            "#39E75FFF"
        );

        string normal = FormatComfortSymbol(
            "😐",
            activeState == ComfortState.Normal,
            "#FFD43BFF"
        );

        string cry = FormatComfortSymbol(
            "🙁",
            activeState == ComfortState.Cry,
            "#FF3B3BFF"
        );

        return smile + "     " + normal + "     " + cry;
    }

    private string FormatComfortSymbol(
        string symbol,
        bool isActive,
        string activeColor
    )
    {
        if (isActive)
        {
            return
                "<size=150%>" +
                "<color=" + activeColor + ">" +
                "<b>" + symbol + "</b>" +
                "</color>" +
                "</size>";
        }

        return
            "<size=100%>" +
            "<color=#FFFFFF40>" +
            symbol +
            "</color>" +
            "</size>";
    }

    private ComfortState GetThermalComfort()
    {
        // Heating OFF = Cry
        if (heatingLevel == 0)
        {
            return ComfortState.Cry;
        }

        // Heating LEVEL 3 = Smile
        if (heatingLevel == 3)
        {
            return ComfortState.Smile;
        }

        // Heating LEVEL 5 = Normal
        return ComfortState.Normal;
    }

    private ComfortState GetAirQuality()
    {
        // Window CLOSED = Cry
        if (!windowOpen)
        {
            return ComfortState.Cry;
        }

        // Window OPEN + Blinds OPEN = Smile
        if (blindsOpen)
        {
            return ComfortState.Smile;
        }

        // Window OPEN + Blinds CLOSED = Normal
        return ComfortState.Normal;
    }

    private int GetCost()
    {
        // Heating OFF
        if (heatingLevel == 0)
        {
            // Blinds OPEN
            if (blindsOpen)
            {
                return 0;
            }

            // Blinds CLOSED
            return 13;
        }

        // Heating LEVEL 3
        if (heatingLevel == 3)
        {
            // Window CLOSED, Blinds OPEN
            if (!windowOpen && blindsOpen)
            {
                return 16;
            }

            // Window OPEN, Blinds OPEN
            if (windowOpen && blindsOpen)
            {
                return 37;
            }

            // Window CLOSED, Blinds CLOSED
            if (!windowOpen && !blindsOpen)
            {
                return 33;
            }

            // Window OPEN, Blinds CLOSED
            if (windowOpen && !blindsOpen)
            {
                return 54;
            }
        }

        // Heating LEVEL 5
        if (heatingLevel == 5)
        {
            // Window CLOSED, Blinds OPEN
            if (!windowOpen && blindsOpen)
            {
                return 26;
            }

            // Window OPEN, Blinds OPEN
            if (windowOpen && blindsOpen)
            {
                return 57;
            }

            // Window CLOSED, Blinds CLOSED
            if (!windowOpen && !blindsOpen)
            {
                return 43;
            }

            // Window OPEN, Blinds CLOSED
            if (windowOpen && !blindsOpen)
            {
                return 74;
            }
        }

        return 0;
    }
}