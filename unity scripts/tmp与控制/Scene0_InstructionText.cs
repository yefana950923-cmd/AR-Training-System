using UnityEngine;
using TMPro;
using System.Collections;

public class Scene0_InstructionText : MonoBehaviour
{
    [Header("H / W / B Instruction")]

    [Tooltip("始终显示 H、W、B 操作说明的 TMP")]
    [SerializeField]
    private TextMeshProUGUI instructionText;


    [Header("Continue Instruction Panel")]

    [Tooltip("包含继续提示文字的背景板")]
    [SerializeField]
    private GameObject continuePanel;

    [Tooltip("必须是 Continue Panel 下面的 TMP 子物体")]
    [SerializeField]
    private TextMeshProUGUI continueText;


    [Header("Survey Reference")]

    [Tooltip("拖入显示当前问题和选项的 TMP")]
    [SerializeField]
    private TextMeshProUGUI surveyText;


    [Header("Delay Settings")]

    [Tooltip("等待多少秒后显示 Continue Panel")]
    [Min(0f)]
    [SerializeField]
    private float displayDelay = 300f;


    private bool continuePanelVisible = false;
    private string previousSurveyText = "";


    private void Start()
    {
        ShowHBWInstructions();

        // 场景开始时隐藏整个 Continue Panel
        if (continuePanel != null)
        {
            continuePanel.SetActive(false);
        }

        if (continueText != null)
        {
            continueText.text = "";
        }

        StartCoroutine(
            ShowContinuePanelAfterDelay()
        );
    }


    private void Update()
    {
        if (!continuePanelVisible ||
            surveyText == null)
        {
            return;
        }

        /*
         * 当前问卷内容发生变化时，
         * 更新 Continue Panel 中的提示。
         */
        if (surveyText.text != previousSurveyText)
        {
            previousSurveyText = surveyText.text;

            UpdateContinueInstruction();
        }
    }


    private IEnumerator ShowContinuePanelAfterDelay()
    {
        if (displayDelay > 0f)
        {
            yield return new WaitForSecondsRealtime(
                displayDelay
            );
        }

        continuePanelVisible = true;

        if (surveyText != null)
        {
            previousSurveyText = surveyText.text;
        }

        UpdateContinueInstruction();

        // 文字更新完成后，再显示整个背景板
        if (continuePanel != null)
        {
            continuePanel.SetActive(true);
        }
    }


    private void ShowHBWInstructions()
    {
        if (instructionText == null)
        {
            return;
        }

        instructionText.text =
            "Press H for the heating.\n\n" +
            "Press W for the window.\n\n" +
            "Press B for the blinds.";
    }


    private void UpdateContinueInstruction()
    {
        if (continueText == null)
        {
            return;
        }

        if (HasAtLeastThreeOptions())
        {
            continueText.text =
                "Press the corresponding number to continue.";
        }
        else
        {
            continueText.text =
                "Press the Down Arrow to continue.";
        }
    }


    private bool HasAtLeastThreeOptions()
    {
        if (surveyText == null ||
            string.IsNullOrWhiteSpace(
                surveyText.text
            ))
        {
            return false;
        }

        string[] lines =
            surveyText.text.Split('\n');

        int optionCount = 0;

        for (int i = 0; i < lines.Length; i++)
        {
            string line =
                lines[i].Trim();

            if (line.StartsWith("1.") ||
                line.StartsWith("2.") ||
                line.StartsWith("3.") ||
                line.StartsWith("4.") ||
                line.StartsWith("5.") ||
                line.StartsWith("6.") ||
                line.StartsWith("7.") ||
                line.StartsWith("8.") ||
                line.StartsWith("9."))
            {
                optionCount++;
            }
        }

        return optionCount >= 3;
    }
}