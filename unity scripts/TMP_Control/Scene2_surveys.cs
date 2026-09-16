using UnityEngine;
using TMPro;
using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

public class Scene2_surveys : MonoBehaviour
{
    [Serializable]
    public class SurveyElement
    {
        [TextArea(2, 6)]
        public string question;

        [TextArea(1, 3)]
        public string[] options;
    }

    [Header("Survey Elements")]
    public SurveyElement[] elements;

    [Header("UI References")]
    public TextMeshProUGUI questionText;
    public TextMeshProUGUI optionsText;
    public GameObject optionsPanel;

    private int index;
    private int[] answers;
    private bool csvSaved;
    private string folderPath;

    private void OnValidate()
    {
        if (elements == null)
        {
            return;
        }

        string[] default5 =
        {
            "Strongly disagree",
            "Disagree",
            "Neutral",
            "Agree",
            "Strongly agree"
        };

        string[] default7 =
        {
            "Totally disagree",
            "Disagree",
            "Slightly disagree",
            "Indifferent",
            "Slightly agree",
            "Agree",
            "Totally agree"
        };

        foreach (SurveyElement element in elements)
        {
            if (element?.options == null)
            {
                continue;
            }

            if (element.options.Length == 5)
            {
                FillEmptyOptions(element.options, default5);
            }
            else if (element.options.Length == 7)
            {
                FillEmptyOptions(element.options, default7);
            }
        }
    }

    private void FillEmptyOptions(
        string[] target,
        string[] defaults)
    {
        for (int i = 0;
             i < Mathf.Min(target.Length, defaults.Length);
             i++)
        {
            if (string.IsNullOrWhiteSpace(target[i]))
            {
                target[i] = defaults[i];
            }
        }
    }

    private void Start()
    {
        folderPath = Path.Combine(
            Application.persistentDataPath,
            "ExperimentData"
        );

        if (questionText == null ||
            optionsText == null ||
            optionsPanel == null)
        {
            Debug.LogError(
                "Scene2_surveys: UI references are missing.",
                gameObject
            );

            enabled = false;
            return;
        }

        if (elements == null ||
            elements.Length == 0)
        {
            Debug.LogError(
                "Scene2_surveys: No survey elements found.",
                gameObject
            );

            enabled = false;
            return;
        }

        SetUpText(questionText);
        SetUpText(optionsText);

        index = 0;
        answers = new int[elements.Length];

        ShowElement();
    }

    private void SetUpText(
        TextMeshProUGUI textComponent)
    {
        textComponent.richText = true;
        textComponent.enableWordWrapping = true;
        textComponent.overflowMode =
            TextOverflowModes.Overflow;
    }

    private void Update()
    {
        List<string> options =
            GetValidOptions(elements[index]);

        if (options.Count >= 3)
        {
            int maximum =
                Mathf.Min(options.Count, 9);

            for (int number = 1;
                 number <= maximum;
                 number++)
            {
                KeyCode normalKey =
                    (KeyCode)((int)KeyCode.Alpha0 + number);

                KeyCode keypadKey =
                    (KeyCode)((int)KeyCode.Keypad0 + number);

                if (Input.GetKeyDown(normalKey) ||
                    Input.GetKeyDown(keypadKey))
                {
                    answers[index] = number;
                    Next();
                    return;
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Next();
        }
    }

    private void ShowElement()
    {
        SurveyElement current =
            elements[index];

        List<string> validOptions =
            GetValidOptions(current);

        questionText.text =
            current?.question ?? "";

        bool showOptions =
            validOptions.Count >= 3;

        optionsPanel.SetActive(showOptions);
        optionsText.gameObject.SetActive(showOptions);

        if (showOptions)
        {
            StringBuilder text =
                new StringBuilder();

            for (int i = 0;
                 i < validOptions.Count;
                 i++)
            {
                text.Append(i + 1);
                text.Append(". ");
                text.Append(validOptions[i]);

                if (i < validOptions.Count - 1)
                {
                    text.Append("\n");
                }
            }

            optionsText.text = text.ToString();
        }
        else
        {
            optionsText.text = "";
        }

        if (index == elements.Length - 1 &&
            !csvSaved)
        {
            csvSaved = true;
            SaveCSV();
        }
    }

    private void Next()
    {
        if (index >= elements.Length - 1)
        {
            return;
        }

        index++;
        ShowElement();
    }

    private List<string> GetValidOptions(
        SurveyElement element)
    {
        List<string> result =
            new List<string>();

        if (element?.options == null)
        {
            return result;
        }

        foreach (string option in element.options)
        {
            if (!string.IsNullOrWhiteSpace(option))
            {
                result.Add(option.Trim());
            }
        }

        return result;
    }

    private void SaveCSV()
    {
        try
        {
            Directory.CreateDirectory(folderPath);

            string csvPath =
                Path.Combine(
                    folderPath,
                    "SurveyResults2.csv"
                );

            int participantID =
                GetNextParticipantID(csvPath);

            string participantCode =
                "P" + participantID.ToString("D3");

            bool writeHeader =
                !File.Exists(csvPath) ||
                new FileInfo(csvPath).Length == 0;

            string date =
                DateTime.Now.ToString("dd.MM.yyyy");

            string time =
                DateTime.Now.ToString("HH:mm:ss");

            using (StreamWriter writer =
                   new StreamWriter(
                       csvPath,
                       true,
                       new UTF8Encoding(true)))
            {
                if (writeHeader)
                {
                    writer.WriteLine(
                        "Participant;Date;Time;Question;Answer"
                    );
                }

                for (int i = 0;
                     i < elements.Length;
                     i++)
                {
                    if (GetValidOptions(elements[i]).Count < 3)
                    {
                        continue;
                    }

                    writer.WriteLine(
                        participantCode + ";" +
                        date + ";" +
                        time + ";" +
                        (i + 1) + ";" +
                        answers[i]
                    );
                }
            }

            Debug.Log(
                "Scene 2 CSV saved: " +
                csvPath +
                " | Participant: " +
                participantCode
            );
        }
        catch (Exception exception)
        {
            Debug.LogError(
                "Failed to save Scene 2 CSV: " +
                exception.Message
            );
        }
    }

    private int GetNextParticipantID(
        string csvPath)
    {
        if (!File.Exists(csvPath))
        {
            return 1;
        }

        string[] lines =
            File.ReadAllLines(csvPath);

        for (int i = lines.Length - 1;
             i >= 1;
             i--)
        {
            if (string.IsNullOrWhiteSpace(lines[i]))
            {
                continue;
            }

            string[] parts =
                lines[i].Split(';');

            if (parts.Length > 0 &&
                parts[0].StartsWith("P") &&
                int.TryParse(
                    parts[0].Substring(1),
                    out int lastID))
            {
                return lastID + 1;
            }
        }

        return 1;
    }
}