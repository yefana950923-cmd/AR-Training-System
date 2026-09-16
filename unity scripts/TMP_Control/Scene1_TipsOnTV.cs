using TMPro;
using UnityEngine;

public class Scene1_TipsOnTV : MonoBehaviour
{
    [Header("电视上的 TextMeshPro 文本组件")]
    [SerializeField] private TMP_Text TextTips;

    [Header("Attempt 1 固定 Tips")]

    [Tooltip("Attempt 1 开始时显示。")]
    [TextArea(3, 8)]
    [SerializeField] private string attempt1InitialTip;

    [Tooltip("Attempt 1 按 Enter 锁定后显示。")]
    [TextArea(3, 8)]
    [SerializeField] private string attempt1FinalTip;

    [Header("Attempt 2 固定 Tips")]

    [Tooltip("按下箭头进入 Attempt 2 时显示。")]
    [TextArea(3, 8)]
    [SerializeField] private string attempt2InitialTip;

    [Tooltip("Attempt 2 按 Enter 锁定后显示。")]
    [TextArea(3, 8)]
    [SerializeField] private string attempt2FinalTip;

    [Header("Attempt 2 Heating Tips")]

    [Tooltip("Heating 切换到 LEVEL 3 时显示。")]
    [TextArea(3, 8)]
    [SerializeField] private string heatingLevel3Tip;

    [Tooltip("Heating 切换到 LEVEL 5 时显示。")]
    [TextArea(3, 8)]
    [SerializeField] private string heatingLevel5Tip;

    [Tooltip("Heating 切换到 OFF 时显示。")]
    [TextArea(3, 8)]
    [SerializeField] private string heatingOffTip;

    [Header("Attempt 2 Window Tips")]

    [Tooltip("Window 切换到 OPEN 时显示。")]
    [TextArea(3, 8)]
    [SerializeField] private string windowOpenTip;

    [Tooltip("Window 切换到 CLOSED 时显示。")]
    [TextArea(3, 8)]
    [SerializeField] private string windowClosedTip;

    [Header("Attempt 2 Blinds Tips")]

    [Tooltip("Blinds 切换到 CLOSED 时显示。")]
    [TextArea(3, 8)]
    [SerializeField] private string blindsClosedTip;

    [Tooltip("Blinds 切换到 OPEN 时显示。")]
    [TextArea(3, 8)]
    [SerializeField] private string blindsOpenTip;

    // 1 = Attempt 1
    // 2 = Attempt 2
    private int currentAttempt = 1;

    // Enter 后锁定 H、W、B
    private bool isLockedByReturn = false;

    /*
     * 与 Scene1_StatusAndComfort 保持相同的初始状态：
     *
     * Heating LEVEL 3
     * Window CLOSED
     * Blinds OPEN
     */
    private int heatingLevel = 3;
    private bool windowOpen = false;
    private bool blindsOpen = true;

    private void Start()
    {
        FindTextTipsIfNecessary();

        currentAttempt = 1;
        isLockedByReturn = false;

        ResetSettingState();
        ShowTip(attempt1InitialTip, "Attempt 1 Initial Tip");
    }

    private void Update()
    {
        /*
         * Attempt 1 锁定后：
         * 按下箭头进入 Attempt 2。
         */
        if (currentAttempt == 1 &&
            isLockedByReturn &&
            Input.GetKeyDown(KeyCode.DownArrow))
        {
            StartAttempt2();
            return;
        }

        /*
         * Enter：
         * 显示当前 Attempt 的最终固定 Tip，
         * 并锁定 H、W、B。
         */
        if (!isLockedByReturn &&
            (Input.GetKeyDown(KeyCode.Return) ||
             Input.GetKeyDown(KeyCode.KeypadEnter)))
        {
            ShowFinalTipAndLock();
            return;
        }

        // 锁定后不响应 H、W、B
        if (isLockedByReturn)
        {
            return;
        }

        // Attempt 1 中按 H、W、B 不改变 Tip
        if (currentAttempt == 1)
        {
            return;
        }

        /*
         * Attempt 2：
         * 每次按键后，显示变化后的档位所对应的 Tip。
         */

        if (Input.GetKeyDown(KeyCode.H))
        {
            ChangeHeatingLevel();
            ShowHeatingTip();
            return;
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            windowOpen = !windowOpen;
            ShowWindowTip();
            return;
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            blindsOpen = !blindsOpen;
            ShowBlindsTip();
        }
    }

    private void StartAttempt2()
    {
        currentAttempt = 2;
        isLockedByReturn = false;

        ResetSettingState();

        // 进入 Attempt 2 时先显示固定的初始说明
        ShowTip(attempt2InitialTip, "Attempt 2 Initial Tip");
    }

    private void ShowFinalTipAndLock()
    {
        if (currentAttempt == 1)
        {
            ShowTip(attempt1FinalTip, "Attempt 1 Final Tip");
        }
        else
        {
            ShowTip(attempt2FinalTip, "Attempt 2 Final Tip");
        }

        isLockedByReturn = true;
    }

    private void ResetSettingState()
    {
        heatingLevel = 3;
        windowOpen = false;
        blindsOpen = true;
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
    }

    private void ShowHeatingTip()
    {
        if (heatingLevel == 3)
        {
            ShowTip(
                heatingLevel3Tip,
                "Heating LEVEL 3 Tip"
            );
        }
        else if (heatingLevel == 5)
        {
            ShowTip(
                heatingLevel5Tip,
                "Heating LEVEL 5 Tip"
            );
        }
        else
        {
            ShowTip(
                heatingOffTip,
                "Heating OFF Tip"
            );
        }
    }

    private void ShowWindowTip()
    {
        if (windowOpen)
        {
            ShowTip(
                windowOpenTip,
                "Window OPEN Tip"
            );
        }
        else
        {
            ShowTip(
                windowClosedTip,
                "Window CLOSED Tip"
            );
        }
    }

    private void ShowBlindsTip()
    {
        if (blindsOpen)
        {
            ShowTip(
                blindsOpenTip,
                "Blinds OPEN Tip"
            );
        }
        else
        {
            ShowTip(
                blindsClosedTip,
                "Blinds CLOSED Tip"
            );
        }
    }

    private void ShowTip(string tip, string tipName)
    {
        if (TextTips == null)
        {
            return;
        }

        if (!string.IsNullOrWhiteSpace(tip))
        {
            TextTips.text = tip;
        }
        else
        {
            TextTips.text = "";

            Debug.LogWarning(
                "Scene1_TipsOnTV：" +
                tipName +
                " 为空，请在 Inspector 中填写。"
            );
        }
    }

    private void FindTextTipsIfNecessary()
    {
        if (TextTips != null)
        {
            return;
        }

        TMP_Text[] textObjects =
            GetComponentsInChildren<TMP_Text>(true);

        foreach (TMP_Text textObject in textObjects)
        {
            if (textObject.gameObject.name == "TextTips")
            {
                TextTips = textObject;
                break;
            }
        }

        if (TextTips == null)
        {
            Debug.LogError(
                "Scene1_TipsOnTV：没有找到名称为 TextTips 的 TextMeshPro 对象。"
            );
        }
    }

    public void ResetTips()
    {
        currentAttempt = 1;
        isLockedByReturn = false;

        ResetSettingState();

        ShowTip(
            attempt1InitialTip,
            "Attempt 1 Initial Tip"
        );
    }
}