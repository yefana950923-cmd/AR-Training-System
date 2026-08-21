using UnityEngine;
using TMPro;

public class Scene1_InstructionText : MonoBehaviour
{
    [Header("H / W / B Instruction TMP")]
    [Tooltip("显示 Heating、Window 和 Blinds 操作说明的 TMP")]
    [SerializeField] private TextMeshProUGUI instructionText;

    [Header("Continue Instruction Panel")]
    [Tooltip("包含 Return / Down Arrow 提示文字的背景板")]
    [SerializeField] private GameObject continuePanel;

    private TextMeshProUGUI continueText;

    // false：等待用户按 Return
    // true：等待用户按 Down Arrow
    private bool waitingForDownArrow = false;

    // 1 = Attempt 1
    // 2 = Attempt 2
    private int currentAttempt = 1;

    // 第二次尝试结束后，停止更新提示
    private bool leavingScene = false;

    private void Start()
    {
        waitingForDownArrow = false;
        currentAttempt = 1;
        leavingScene = false;

        // 自动获取 Continue Panel 子物体中的 TMP
        if (continuePanel != null)
        {
            continueText =
                continuePanel.GetComponentInChildren<TextMeshProUGUI>(true);

            continuePanel.SetActive(true);
        }

        UpdateInstructionUI();
    }

    private void Update()
    {
        if (leavingScene)
        {
            return;
        }

        bool returnPressed =
            Input.GetKeyDown(KeyCode.Return) ||
            Input.GetKeyDown(KeyCode.KeypadEnter);

        // 按 Return 后切换为 Down Arrow 提示
        if (!waitingForDownArrow && returnPressed)
        {
            waitingForDownArrow = true;
            UpdateInstructionUI();
            return;
        }

        // 确认设置后按下箭头
        if (waitingForDownArrow &&
            Input.GetKeyDown(KeyCode.DownArrow))
        {
            // Attempt 1 结束，进入 Attempt 2
            if (currentAttempt == 1)
            {
                currentAttempt = 2;
                waitingForDownArrow = false;

                UpdateInstructionUI();
                return;
            }

            // Attempt 2 结束，准备跳转到下一个场景
            if (currentAttempt == 2)
            {
                leavingScene = true;

                // 立即隐藏 Continue Panel
                if (continuePanel != null)
                {
                    continuePanel.SetActive(false);
                }

                // 同时清空 H / W / B 提示，避免跳转时闪现
                if (instructionText != null)
                {
                    instructionText.text = "";
                }
            }
        }
    }

    private void UpdateInstructionUI()
    {
        if (leavingScene)
        {
            return;
        }

        // 原 TMP 始终只显示 H / W / B
        if (instructionText != null)
        {
            instructionText.text =
                "Press H for the heating.\n\n" +
                "Press W for the window.\n\n" +
                "Press B for the blinds.";
        }

        // Continue Panel 内的 TMP 根据当前阶段切换文字
        if (continueText != null)
        {
            if (waitingForDownArrow)
            {
                continueText.text =
                    "Press the Down Arrow to continue.";
            }
            else
            {
                continueText.text =
                    "Press Return to confirm the settings.";
            }
        }
    }
}