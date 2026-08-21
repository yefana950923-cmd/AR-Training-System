using UnityEngine;
using TMPro;

public class Scene0_Interaction : MonoBehaviour
{
    [Header("Main Status UI")]
    [SerializeField] private TextMeshProUGUI statusText;

    [Tooltip("三行状态文字之间的统一垂直间距")]
    [SerializeField] private float lineSpacing = 24f;

    [Header("Names UI")]
    [SerializeField] private Scene0_NamesUI namesUI;

    [Header("How To Use TMP")]
    [SerializeField] private TextMeshProUGUI howToUseText;

    [Header("Window Visual Object")]
    [SerializeField] private GameObject window;

    [Header("Blinds and Light Objects")]
    [SerializeField] private GameObject blinds;
    [SerializeField] private GameObject light1;
    [SerializeField] private GameObject light2;

    private bool interactionLocked = false;

    // Heating:
    // 0 = OFF
    // 3 = LEVEL 3
    // 5 = LEVEL 5
    private int heatingLevel = 3;

    // Window:
    // false = CLOSED
    // true  = OPEN
    private bool windowOpen = false;

    // Blinds:
    // false = CLOSED
    // true  = OPEN
    private bool blindsOpen = true;

    /*
     * Scene0_NamesUI通过这些只读属性，
     * 读取Scene0_Interaction中的真实状态。
     */
    public int HeatingLevel
    {
        get { return heatingLevel; }
    }

    public bool IsWindowOpen
    {
        get { return windowOpen; }
    }

    public bool AreBlindsOpen
    {
        get { return blindsOpen; }
    }

    public bool IsInteractionLocked
    {
        get { return interactionLocked; }
    }

    private void Start()
    {
        if (statusText != null)
        {
            statusText.richText = true;
            statusText.enableWordWrapping = false;
            statusText.overflowMode = TextOverflowModes.Overflow;
            statusText.lineSpacing = lineSpacing;
        }

        /*
         * Scene0开始时允许交互，
         * 因此显示Howtouse。
         */
        interactionLocked = false;

        if (howToUseText != null)
        {
            howToUseText.gameObject.SetActive(true);
        }

        RefreshAll();
    }

    private void Update()
    {
        /*
         * 锁定后不接受H、W、B输入。
         */
        if (interactionLocked)
        {
            return;
        }

        // H: LEVEL 3 -> LEVEL 5 -> OFF -> LEVEL 3
        if (Input.GetKeyDown(KeyCode.H))
        {
            ChangeHeatingLevel();
        }

        // W: CLOSED <-> OPEN
        if (Input.GetKeyDown(KeyCode.W))
        {
            windowOpen = !windowOpen;
            RefreshAll();
        }

        // B: OPEN <-> CLOSED
        if (Input.GetKeyDown(KeyCode.B))
        {
            blindsOpen = !blindsOpen;
            RefreshAll();
        }
    }

    private void ChangeHeatingLevel()
    {
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

        RefreshAll();
    }

    /// <summary>
    /// 锁定或解除锁定H、W、B交互。
    ///
    /// locked = true：
    /// 锁定交互并隐藏Howtouse。
    ///
    /// locked = false：
    /// 恢复交互并显示Howtouse。
    /// </summary>
    public void SetInteractionLocked(bool locked)
    {
        interactionLocked = locked;

        if (howToUseText != null)
        {
            howToUseText.gameObject.SetActive(!locked);
        }
    }

    /// <summary>
    /// 恢复Scene0的初始状态。
    /// </summary>
    public void ResetToInitialState()
    {
        heatingLevel = 3;
        windowOpen = false;
        blindsOpen = true;

        RefreshAll();
    }

    /// <summary>
    /// 从第二个Element开始调用。
    ///
    /// 恢复初始状态：
    /// Heating Level 3
    /// Window Closed
    /// Blinds Open
    ///
    /// 然后：
    /// 锁定H、W、B
    /// 隐藏Howtouse
    /// NamesUI继续显示
    /// </summary>
    public void EnterLockedInitialState()
    {
        ResetToInitialState();
        SetInteractionLocked(true);
    }

    /// <summary>
    /// 解除锁定。
    /// 当前Scene0可能暂时不需要调用。
    /// </summary>
    public void UnlockInteraction()
    {
        SetInteractionLocked(false);
    }

    /// <summary>
    /// 统一刷新场景物体、状态文字和NamesUI。
    /// </summary>
    private void RefreshAll()
    {
        ApplyWindowVisual();
        ApplyBlindsVisual();
        UpdateStatusText();

        if (namesUI != null)
        {
            namesUI.RefreshUI();
        }
    }

    private void ApplyWindowVisual()
    {
        if (window == null)
        {
            return;
        }

        /*
         * Window CLOSED：
         * 显示window物体。
         *
         * Window OPEN：
         * 隐藏window物体。
         */
        window.SetActive(!windowOpen);
    }

    private void ApplyBlindsVisual()
    {
        /*
         * Blinds OPEN：
         * 隐藏blinds物体；
         * 开启light1和light2。
         *
         * Blinds CLOSED：
         * 显示blinds物体；
         * 关闭light1和light2。
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

    private void UpdateStatusText()
    {
        if (statusText == null)
        {
            return;
        }

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

        statusText.text =
            "<b>Heating:</b>" +
            "<pos=18%>" + heatingOffText +
            "<pos=30%>" + heatingLevel3Text +
            "<pos=45%>" + heatingLevel5Text + "\n" +

            "<b>Window:</b>" +
            "<pos=18%>" + windowClosedText +
            "<pos=34%>" + windowOpenText + "\n" +

            "<b>Blinds:</b>" +
            "<pos=18%>" + blindsClosedText +
            "<pos=34%>" + blindsOpenText;
    }

    private string FormatOption(string optionText, bool isActive)
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
}