using System.Collections;
using System.Reflection;
using TMPro;
using UnityEngine;

public class Scene2_InstructionText : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Scene2_surveys survey;

    [Tooltip("整个提示背景板，Instruction Text 应放在该对象下面")]
    [SerializeField] private GameObject instructionPanel;

    [SerializeField] private TMP_Text instructionText;

    [Header("Display Delay")]
    [Min(0f)]
    [SerializeField] private float initialDisplayDelay = 60f;

    [Header("Instruction Text")]
    [SerializeField] private string arrowText =
        "Press the Down Arrow to continue.";

    [SerializeField] private string numberText =
        "Press the corresponding number to continue.";

    private FieldInfo indexField;
    private int lastIndex = -1;
    private bool delayFinished;

    private void Start()
    {
        if (survey == null)
        {
            survey = FindObjectOfType<Scene2_surveys>();
        }

        if (instructionText == null)
        {
            instructionText = GetComponentInChildren<TMP_Text>(true);
        }

        if (instructionPanel == null &&
            instructionText != null)
        {
            instructionPanel =
                instructionText.transform.parent.gameObject;
        }

        indexField = typeof(Scene2_surveys).GetField(
            "index",
            BindingFlags.Instance |
            BindingFlags.NonPublic
        );

        if (survey == null ||
            instructionText == null ||
            instructionPanel == null ||
            indexField == null)
        {
            Debug.LogError(
                "Scene2_InstructionText: References are missing.",
                gameObject
            );

            return;
        }

        if (instructionPanel == gameObject)
        {
            Debug.LogError(
                "Scene2_InstructionText must not be attached to the Instruction Panel.",
                gameObject
            );

            return;
        }

        instructionText.text = "";
        instructionPanel.SetActive(false);

        StartCoroutine(ShowAfterDelay());
    }

    private IEnumerator ShowAfterDelay()
    {
        if (initialDisplayDelay > 0f)
        {
            yield return new WaitForSecondsRealtime(
                initialDisplayDelay
            );
        }

        delayFinished = true;
        UpdateInstruction();
    }

    private void Update()
    {
        if (!delayFinished ||
            survey == null ||
            indexField == null)
        {
            return;
        }

        int currentIndex = GetCurrentIndex();

        if (currentIndex != lastIndex)
        {
            UpdateInstruction();
        }
    }

    private void UpdateInstruction()
    {
        if (survey.elements == null ||
            survey.elements.Length == 0)
        {
            HideInstruction();
            return;
        }

        int currentIndex = GetCurrentIndex();
        lastIndex = currentIndex;

        if (currentIndex < 0 ||
            currentIndex >= survey.elements.Length)
        {
            HideInstruction();
            return;
        }

        // 最后一个Element隐藏整个提示板
        if (currentIndex ==
            survey.elements.Length - 1)
        {
            HideInstruction();
            return;
        }

        instructionPanel.SetActive(true);

        // 第一个Element
        if (currentIndex == 0)
        {
            instructionText.text = arrowText;
            return;
        }

        Scene2_surveys.SurveyElement current =
            survey.elements[currentIndex];

        if (GetValidOptionCount(current) >= 3)
        {
            int remaining =
                CountRemainingQuestions(
                    currentIndex
                );

            string counterText =
                remaining == 1
                    ? "1 question remaining."
                    : remaining +
                      " questions remaining.";

            instructionText.text =
                numberText +
                "\n\n" +
                counterText;
        }
        else
        {
            instructionText.text = arrowText;
        }
    }

    private void HideInstruction()
    {
        instructionText.text = "";
        instructionPanel.SetActive(false);
    }

    private int GetCurrentIndex()
    {
        object value =
            indexField.GetValue(survey);

        return value is int
            ? (int)value
            : -1;
    }

    private int CountRemainingQuestions(
        int currentIndex)
    {
        int count = 0;

        for (int i = currentIndex;
             i < survey.elements.Length;
             i++)
        {
            if (GetValidOptionCount(
                    survey.elements[i]) >= 3)
            {
                count++;
            }
        }

        return count;
    }

    private int GetValidOptionCount(
        Scene2_surveys.SurveyElement element)
    {
        if (element == null ||
            element.options == null)
        {
            return 0;
        }

        int count = 0;

        foreach (string option in element.options)
        {
            if (!string.IsNullOrWhiteSpace(option))
            {
                count++;
            }
        }

        return count;
    }
}