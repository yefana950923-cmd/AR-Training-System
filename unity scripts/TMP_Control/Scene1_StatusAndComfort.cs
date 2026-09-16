using UnityEngine;
using TMPro;

public class Scene1_StatusAndComfort : MonoBehaviour
{
    [Header("UI References")]

    [Tooltip("显示 Heating、Window 和 Blinds 的状态选项")]
    [SerializeField] private TextMeshProUGUI statusText;

    [Tooltip("整个 Feedback 背景面板")]
    [SerializeField] private GameObject feedbackPanel;

    [Tooltip("显示 Current Cost、Temperature 和 Air Quality")]
    [SerializeField] private TextMeshProUGUI feedbackText;

    [Header("Names UI")]

    [Tooltip("显示 Heating、Window 和 Blinds 名称及当前状态")]
    [SerializeField] private Scene1_NamesUI namesUI;

    [Header("Text Layout")]

    [Tooltip("Heating、Window、Blinds 的统一行间距")]
    [SerializeField] private float statusLineSpacing = 20f;

    [Tooltip("Feedback 文字的统一行间距")]
    [SerializeField] private float feedbackLineSpacing = 10f;

    [Tooltip("Feedback 分隔线长度")]
    [SerializeField] private int separatorLength = 52;

    [Header("Temperature Layout")]

    [Tooltip("温度在 Temperature 行中的固定位置")]
    [Range(0f, 100f)]
    [SerializeField] private float temperaturePosition = 52f;

    [Tooltip("温度表情在 Temperature 行中的固定位置")]
    [Range(0f, 100f)]
    [SerializeField] private float thermalEmojiPosition = 72f;

    [Tooltip("温度表情的大小")]
    [SerializeField] private int thermalEmojiSize = 140;

    [Header("Window Visual Object")]

    [Tooltip("Window CLOSED 时显示，Window OPEN 时隐藏")]
    [SerializeField] private GameObject window;

    [Header("Blinds Visual Objects")]

    [SerializeField] private GameObject blinds;
    [SerializeField] private GameObject light1;
    [SerializeField] private GameObject light2;

    /*
     * Heating:
     * 0 = OFF
     * 3 = LEVEL 3
     * 5 = LEVEL 5
     */
    private int heatingLevel = 3;

    /*
     * Window:
     * false = CLOSED
     * true  = OPEN
     */
    private bool windowOpen = false;

    /*
     * Blinds:
     * false = CLOSED
     * true  = OPEN
     */
    private bool blindsOpen = true;

    // Return 按下后锁定 H、W、B
    private bool isLocked = false;

    /*
     * 1 = Attempt 1
     * 2 = Attempt 2
     */
    private int currentAttempt = 1;

    private enum ComfortState
    {
        Smile,
        Normal,
        Cry
    }

    private void Start()
    {
        SetUpTextComponents();

        // Attempt 1 和 Attempt 2 都显示 Feedback
        SetFeedbackVisible(true);

        ResetToInitialState();
        UpdateAllUI();
    }

    private void SetUpTextComponents()
    {
        if (statusText != null)
        {
            statusText.richText = true;
            statusText.enableWordWrapping = false;
            statusText.overflowMode = TextOverflowModes.Overflow;
            statusText.lineSpacing = statusLineSpacing;
        }

        if (feedbackText != null)
        {
            feedbackText.richText = true;
            feedbackText.enableWordWrapping = false;
            feedbackText.overflowMode = TextOverflowModes.Overflow;
            feedbackText.lineSpacing = feedbackLineSpacing;
        }
    }

    private void Update()
    {
        /*
         * Attempt 1 锁定后：
         * 按下箭头开始 Attempt 2。
         */
        if (currentAttempt == 1 &&
            isLocked &&
            Input.GetKeyDown(KeyCode.DownArrow))
        {
            StartSecondAttempt();
            return;
        }

        /*
         * Return：
         * 锁定当前选择。
         */
        if (!isLocked &&
            (Input.GetKeyDown(KeyCode.Return) ||
             Input.GetKeyDown(KeyCode.KeypadEnter)))
        {
            LockSelection();
            return;
        }

        // 锁定后不再响应 H、W、B
        if (isLocked)
        {
            return;
        }

        // H：LEVEL 3 → LEVEL 5 → OFF → LEVEL 3
        if (Input.GetKeyDown(KeyCode.H))
        {
            ChangeHeatingLevel();
        }

        // W：CLOSED ↔ OPEN
        if (Input.GetKeyDown(KeyCode.W))
        {
            windowOpen = !windowOpen;

            ApplyWindowVisual();
            UpdateAllUI();
        }

        // B：OPEN ↔ CLOSED
        if (Input.GetKeyDown(KeyCode.B))
        {
            blindsOpen = !blindsOpen;

            ApplyBlindsVisual();
            UpdateAllUI();
        }
    }

    private void StartSecondAttempt()
    {
        currentAttempt = 2;
        isLocked = false;

        /*
         * Attempt 2 初始状态：
         * Heating LEVEL 3
         * Window CLOSED
         * Blinds OPEN
         */
        ResetToInitialState();

        SetFeedbackVisible(true);
        UpdateAllUI();

        Debug.Log("Attempt 2 started.");
    }

    private void SetFeedbackVisible(bool visible)
    {
        if (feedbackPanel != null)
        {
            feedbackPanel.SetActive(visible);
        }
    }

    private void ResetToInitialState()
    {
        heatingLevel = 3;
        windowOpen = false;
        blindsOpen = true;

        ApplyWindowVisual();
        ApplyBlindsVisual();
    }

    private void ApplyWindowVisual()
    {
        /*
         * Window OPEN：
         * 隐藏 window 物体。
         *
         * Window CLOSED：
         * 显示 window 物体。
         */
        if (window != null)
        {
            window.SetActive(!windowOpen);
        }
    }

    private void ApplyBlindsVisual()
    {
        /*
         * Blinds OPEN：
         * 隐藏 blinds，开启 light1 和 light2。
         *
         * Blinds CLOSED：
         * 显示 blinds，关闭 light1 和 light2。
         */
        if (blindsOpen)
        {
            if (blinds != null)
            {
                blinds.SetActive(false);
            }

            if (light1 != null)
            {
                light1.SetActive(true);
            }

            if (light2 != null)
            {
                light2.SetActive(true);
            }
        }
        else
        {
            if (blinds != null)
            {
                blinds.SetActive(true);
            }

            if (light1 != null)
            {
                light1.SetActive(false);
            }

            if (light2 != null)
            {
                light2.SetActive(false);
            }
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
            windowOpen
                ? "OPEN"
                : "CLOSED";

        string blindsStatus =
            blindsOpen
                ? "OPEN"
                : "CLOSED";

        Debug.Log(
            "Attempt " + currentAttempt +
            " selection locked | " +
            "Heating: " + heatingStatus +
            " | Window: " + windowStatus +
            " | Blinds: " + blindsStatus +
            " | Cost: " + GetCost() +
            " € / month" +
            " | Temperature: " +
            GetIndoorTemperature() + " °C" +
            " | Air Quality: " +
            GetAirQuality()
        );
    }

    private void ChangeHeatingLevel()
    {
        // LEVEL 3 → LEVEL 5 → OFF → LEVEL 3
        if (heatingLevel == 3)
        {
            heatingLevel = 5;
        }
        else if (heatingLevel == 5)
        {
            heatingLevel = 0;
        }
        else
        {
            heatingLevel = 3;
        }

        UpdateAllUI();
    }

    /*
     * 所有 UI 都由主交互脚本统一更新。
     */
    private void UpdateAllUI()
    {
        UpdateStatusText();
        UpdateFeedbackText();
        UpdateNamesUI();
    }

    private void UpdateNamesUI()
    {
        if (namesUI == null)
        {
            return;
        }

        /*
         * 即使 Scene1_NamesUI 被取消勾选，
         * 仍然把最新状态传给它保存。
         *
         * 重新勾选时，它会显示最新状态。
         */
        namesUI.UpdateUI(
            heatingLevel,
            windowOpen,
            blindsOpen
        );
    }

    private void UpdateStatusText()
    {
        if (statusText == null)
        {
            return;
        }

        string heatingOffText =
            FormatOption(
                "OFF",
                heatingLevel == 0
            );

        string heatingLevel3Text =
            FormatOption(
                "LEVEL 3",
                heatingLevel == 3
            );

        string heatingLevel5Text =
            FormatOption(
                "LEVEL 5",
                heatingLevel == 5
            );

        string windowClosedText =
            FormatOption(
                "CLOSED",
                !windowOpen
            );

        string windowOpenText =
            FormatOption(
                "OPEN",
                windowOpen
            );

        string blindsClosedText =
            FormatOption(
                "CLOSED",
                !blindsOpen
            );

        string blindsOpenText =
            FormatOption(
                "OPEN",
                blindsOpen
            );

        statusText.text =
            "<b>Heating:</b> " +
            "<pos=18%>" + heatingOffText +
            "<pos=30%>" + heatingLevel3Text +
            "<pos=45%>" + heatingLevel5Text +
            "\n" +

            "<b>Window:</b> " +
            "<pos=18%>" + windowClosedText +
            "<pos=34%>" + windowOpenText +
            "\n" +

            "<b>Blinds:</b> " +
            "<pos=18%>" + blindsClosedText +
            "<pos=34%>" + blindsOpenText;
    }

    private void UpdateFeedbackText()
    {
        if (feedbackText == null)
        {
            return;
        }

        int cost = GetCost();

        string temperatureDisplay =
            GetTemperatureDisplay();

        string airQualityIcons =
            FormatComfortOptions(
                GetAirQuality()
            );

        int safeSeparatorLength =
            Mathf.Max(
                1,
                separatorLength
            );

        string separator =
            "<color=#9BB9C5FF>" +
            new string(
                '-',
                safeSeparatorLength
            ) +
            "</color>";

        feedbackText.text =
            "<b>Current Cost:</b> " +
            "<pos=52%>" +
            cost +
            " € / month\n" +

            separator +
            "\n" +

            "<b>Temperature:</b> " +
            temperatureDisplay +
            "\n" +

            separator +
            "\n" +

            "<b>Air Quality:</b> " +
            "<pos=52%>" +
            airQualityIcons +

            "\n\n<size=50%><i>" +
            "Air quality, temperature, and energy costs are estimated based on the selected settings." +
            "</i></size>";
    }

    private string FormatOption(
        string optionText,
        bool isActive)
    {
        if (isActive)
        {
            return
                "<color=#FFFFFFFF>" +
                optionText +
                "</color>";
        }

        return
            "<color=#FFFFFF45>" +
            optionText +
            "</color>";
    }

    private string FormatComfortOptions(
        ComfortState activeState)
    {
        string smile =
            FormatComfortSymbol(
                "🙂",
                activeState == ComfortState.Smile,
                "#39E75FFF"
            );

        string normal =
            FormatComfortSymbol(
                "😐",
                activeState == ComfortState.Normal,
                "#FFD43BFF"
            );

        string cry =
            FormatComfortSymbol(
                "🙁",
                activeState == ComfortState.Cry,
                "#FF3B3BFF"
            );

        return
            smile +
            "     " +
            normal +
            "     " +
            cry;
    }

    private string FormatComfortSymbol(
        string symbol,
        bool isActive,
        string activeColor)
    {
        if (isActive)
        {
            return
                "<size=150%>" +
                "<color=" +
                activeColor +
                ">" +
                "<b>" +
                symbol +
                "</b>" +
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

    /*
     * Heating OFF：
     * 3°C，蓝色寒冷表情。
     *
     * LEVEL 3：
     * Window CLOSED = 20°C
     * Window OPEN   = 19°C
     *
     * LEVEL 5：
     * Window CLOSED = 28°C
     * Window OPEN   = 27°C
     */
    private string GetTemperatureDisplay()
    {
        int temperature =
            GetIndoorTemperature();

        if (heatingLevel == 0)
        {
            return
                "<pos=" +
                temperaturePosition +
                "%>" +
                temperature +
                " °C" +

                "<pos=" +
                thermalEmojiPosition +
                "%>" +

                "<size=" +
                thermalEmojiSize +
                "%>" +

                "<color=#4FC3F7>" +
                "🥶" +
                "</color>" +

                "</size>";
        }

        if (heatingLevel == 3)
        {
            return
                "<pos=" +
                temperaturePosition +
                "%>" +
                temperature +
                " °C" +

                "<pos=" +
                thermalEmojiPosition +
                "%>" +

                "<size=" +
                thermalEmojiSize +
                "%>" +

                "<color=#39E75F>" +
                "🙂" +
                "</color>" +

                "</size>";
        }

        return
            "<pos=" +
            temperaturePosition +
            "%>" +
            temperature +
            " °C" +

            "<pos=" +
            thermalEmojiPosition +
            "%>" +

            "<size=" +
            thermalEmojiSize +
            "%>" +

            "<color=#FF5252>" +
            "🥵" +
            "</color>" +

            "</size>";
    }

    private int GetIndoorTemperature()
    {
        /*
         * Heating OFF：
         * 室内稳态温度采用室外温度 3°C。
         */
        if (heatingLevel == 0)
        {
            return 3;
        }

        int temperature;

        if (heatingLevel == 3)
        {
            temperature = 20;
        }
        else
        {
            temperature = 28;
        }

        // 开窗后稳态温度降低 1°C
        if (windowOpen)
        {
            temperature -= 1;
        }

        return temperature;
    }

    private ComfortState GetAirQuality()
    {
        // Window CLOSED = 空气质量差
        if (!windowOpen)
        {
            return ComfortState.Cry;
        }

        // Window OPEN + Blinds OPEN = 空气质量好
        if (blindsOpen)
        {
            return ComfortState.Smile;
        }

        // Window OPEN + Blinds CLOSED = 空气质量一般
        return ComfortState.Normal;
    }

    private int GetCost()
    {
        // Heating OFF
        if (heatingLevel == 0)
        {
            if (blindsOpen)
            {
                return 0;
            }

            return 13;
        }

        // Heating LEVEL 3
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

            return 54;
        }

        // Heating LEVEL 5
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

            return 74;
        }

        return 0;
    }
}