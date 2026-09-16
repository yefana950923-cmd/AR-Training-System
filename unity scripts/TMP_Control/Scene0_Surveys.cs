using UnityEngine;
using TMPro;
using System.IO;
using System.Text;

public class Scene0_Surveys : MonoBehaviour
{
    [System.Serializable]
    public class SurveyElement
    {
        [TextArea(2, 6)]
        public string question;

        [TextArea(1, 3)]
        public string[] options;
    }

    [Header("Survey")]
    public SurveyElement[] elements;
    public TextMeshProUGUI displayText;

    [Header("Scene0 Interaction")]
    [SerializeField] private Scene0_Interaction scene0Interaction;

    [Tooltip("从第几个Element开始锁定交互。填写2表示第二个Element。")]
    [SerializeField] private int lockFromElement = 2;

    private int index = 0;
    private int[] answers;
    private bool csvSaved = false;

    // 避免每进入一个新Element都重新执行状态重置
    private bool interactionHasBeenLocked = false;

    private string folderPath;

    private void Start()
    {
        // 与Attempt 1和Attempt 2的统计文件保存在同一个文件夹
        folderPath = Path.Combine(
            Application.persistentDataPath,
            "ExperimentData"
        );

        Debug.Log("Scene 0 CSV save folder: " + folderPath);

        if (elements == null || elements.Length == 0)
        {
            Debug.LogWarning(
                "Scene0_Surveys: No survey elements have been assigned."
            );
            return;
        }

        if (displayText == null)
        {
            Debug.LogError(
                "Scene0_Surveys: Display Text has not been assigned."
            );
            return;
        }

        answers = new int[elements.Length];

        ShowElement();
    }

    private void Update()
    {
        if (elements == null || elements.Length == 0)
        {
            return;
        }

        if (index < 0 || index >= elements.Length)
        {
            return;
        }

        SurveyElement current = elements[index];

        int optionCount =
            current.options == null
                ? 0
                : current.options.Length;

        /*
         * 有5个或更多选项的问题：
         * 使用数字键作答。
         */
        if (optionCount >= 5)
        {
            for (int i = 1; i <= optionCount; i++)
            {
                KeyCode numberKey =
                    (KeyCode)((int)KeyCode.Alpha0 + i);

                KeyCode keypadKey =
                    (KeyCode)((int)KeyCode.Keypad0 + i);

                if (Input.GetKeyDown(numberKey) ||
                    Input.GetKeyDown(keypadKey))
                {
                    answers[index] = i;

                    Next();
                    return;
                }
            }
        }
        else
        {
            /*
             * 少于5个选项的Element：
             * 按下箭头继续。
             */
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                Next();
            }
        }
    }

    private void ShowElement()
    {
        if (index < 0 || index >= elements.Length)
        {
            return;
        }

        SurveyElement current = elements[index];

        string questionText =
            current.question == null
                ? string.Empty
                : current.question;

        string text =
            "<align=\"center\">" +
            questionText +
            "</align>";

        if (current.options != null &&
            current.options.Length > 1)
        {
            text += "\n\n<align=\"left\">";

            for (int i = 0; i < current.options.Length; i++)
            {
                text +=
                    (i + 1) +
                    ". " +
                    current.options[i] +
                    "\n";
            }

            text += "</align>";
        }

        displayText.text = text;

        /*
         * 每次显示Element时检查是否已经到达锁定位置。
         */
        UpdateInteractionLock();

        /*
         * 到达最后一个Element时自动保存CSV。
         */
        if (index == elements.Length - 1 &&
            !csvSaved)
        {
            csvSaved = true;

            PrintResults();
            SaveCSV();
        }
    }

    private void UpdateInteractionLock()
    {
        if (scene0Interaction == null)
        {
            return;
        }

        /*
         * Inspector中的Element编号从1开始理解，
         * 代码中的index从0开始。
         *
         * lockFromElement = 2
         * 对应index = 1。
         */
        int lockIndex =
            Mathf.Max(0, lockFromElement - 1);

        /*
         * 只在第一次进入锁定范围时执行一次。
         *
         * 从第二个Element开始：
         * 1. 恢复初始状态
         * 2. 锁定H/W/B
         * 3. 隐藏Howtouse
         * 4. NamesUI保持显示
         */
        if (index >= lockIndex &&
            !interactionHasBeenLocked)
        {
            interactionHasBeenLocked = true;

            scene0Interaction.EnterLockedInitialState();
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

    private void PrintResults()
    {
        Debug.Log("===== Survey Results =====");

        for (int i = 0; i < elements.Length; i++)
        {
            if (elements[i].options != null &&
                elements[i].options.Length >= 5)
            {
                Debug.Log(
                    "Question " +
                    (i + 1) +
                    " Answer = " +
                    answers[i]
                );
            }
        }
    }

    private void SaveCSV()
    {
        try
        {
            Directory.CreateDirectory(folderPath);

            string csvPath = Path.Combine(
                folderPath,
                "SurveyResults0.csv"
            );

            int participantID = 1;

            if (File.Exists(csvPath))
            {
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

                    // CSV使用德国格式的分号分隔符
                    string[] parts =
                        lines[i].Split(';');

                    if (parts.Length > 0)
                    {
                        string idText =
                            parts[0].Trim();

                        if (idText.StartsWith("P") &&
                            int.TryParse(
                                idText.Substring(1),
                                out int lastID))
                        {
                            participantID =
                                lastID + 1;

                            break;
                        }
                    }
                }
            }

            // 编号格式：P001、P002、P003……
            string participantCode =
                "P" +
                participantID.ToString("D3");

            bool writeHeader =
                !File.Exists(csvPath) ||
                new FileInfo(csvPath).Length == 0;

            string date =
                System.DateTime.Now.ToString(
                    "dd.MM.yyyy"
                );

            string time =
                System.DateTime.Now.ToString(
                    "HH:mm:ss"
                );

            // UTF-8 with BOM，方便德国Excel正确识别
            Encoding csvEncoding =
                new UTF8Encoding(true);

            using (StreamWriter writer =
                   new StreamWriter(
                       csvPath,
                       true,
                       csvEncoding))
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
                    if (elements[i].options != null &&
                        elements[i].options.Length >= 5)
                    {
                        writer.WriteLine(
                            participantCode + ";" +
                            date + ";" +
                            time + ";" +
                            (i + 1) + ";" +
                            answers[i]
                        );
                    }
                }
            }

            Debug.Log(
                "CSV saved: " +
                csvPath +
                " | Participant: " +
                participantCode
            );
        }
        catch (System.Exception e)
        {
            Debug.LogError(
                "Failed to save Scene 0 CSV: " +
                e.Message
            );
        }
    }
}