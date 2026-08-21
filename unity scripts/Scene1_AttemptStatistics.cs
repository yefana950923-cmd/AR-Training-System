using System;
using System.Globalization;
using System.IO;
using System.Text;
using UnityEngine;

public class Scene1_AttemptStatistics : MonoBehaviour
{
    [Header("Participant number")]
    [Tooltip("Optional fixed participant number, for example P004. Leave empty for automatic numbering.")]
    [SerializeField] private string participantNumberOverride = "";

    [Header("Heating cycle")]
    [SerializeField] private string[] heatingStates =
    {
        "Off",
        "Level 3",
        "Level 5"
    };

    [Header("Blinds cycle")]
    [SerializeField] private string[] blindsStates =
    {
        "Closed",
        "Open"
    };

    [Header("Window cycle")]
    [SerializeField] private string[] windowStates =
    {
        "Closed",
        "Open"
    };

    [Header("Attempt 1 initial state")]
    [SerializeField] private int attempt1HeatingStartIndex = 0;
    [SerializeField] private int attempt1BlindsStartIndex = 1;
    [SerializeField] private int attempt1WindowStartIndex = 0;

    [Header("Attempt 2 initial state")]
    [SerializeField] private int attempt2HeatingStartIndex = 0;
    [SerializeField] private int attempt2BlindsStartIndex = 1;
    [SerializeField] private int attempt2WindowStartIndex = 0;

    [Header("CSV settings")]
    [SerializeField] private string dataFolderName = "ExperimentData";
    [SerializeField] private string attempt1FileName = "Attempt1_Results.csv";
    [SerializeField] private string attempt2FileName = "Attempt2_Results.csv";

    private const char CsvSeparator = ';';

    private static Scene1_AttemptStatistics activeInstance;

    private int currentAttempt = 1;
    private bool currentAttemptConfirmed;
    private bool attempt1Saved;
    private bool attempt2Saved;

    private int heatingIndex;
    private int blindsIndex;
    private int windowIndex;
    private int totalKeyCount;

    private string participantNumber;
    private string dataFolderPath;

    private readonly Encoding csvEncoding = new UTF8Encoding(true);

    private void Awake()
    {
        if (activeInstance != null && activeInstance != this)
        {
            Debug.LogWarning(
                "A second Scene1_AttemptStatistics component was found. " +
                "The duplicate component has been disabled."
            );

            enabled = false;
            return;
        }

        activeInstance = this;

        ValidateStateArrays();

        dataFolderPath = Path.Combine(
            Application.persistentDataPath,
            dataFolderName
        );

        Directory.CreateDirectory(dataFolderPath);

        participantNumber = CreateParticipantNumber();

        SetAttemptState(1);

        Debug.Log("Participant number: " + participantNumber);
        Debug.Log("CSV save folder: " + dataFolderPath);
    }

    private void OnDestroy()
    {
        if (activeInstance == this)
        {
            activeInstance = null;
        }
    }

    private void Update()
    {
        if (!currentAttemptConfirmed)
        {
            HandleControlKeys();
        }

        if (Input.GetKeyDown(KeyCode.Return) ||
            Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            ConfirmAndSaveCurrentAttempt();
        }

        if (Input.GetKeyDown(KeyCode.DownArrow) &&
            currentAttempt == 1 &&
            currentAttemptConfirmed &&
            attempt1Saved)
        {
            SetAttemptState(2);
        }
    }

    private void HandleControlKeys()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            totalKeyCount++;

            heatingIndex = GetNextIndex(
                heatingIndex,
                heatingStates.Length
            );
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            totalKeyCount++;

            blindsIndex = GetNextIndex(
                blindsIndex,
                blindsStates.Length
            );
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            totalKeyCount++;

            windowIndex = GetNextIndex(
                windowIndex,
                windowStates.Length
            );
        }
    }

    private void ConfirmAndSaveCurrentAttempt()
    {
        if (currentAttemptConfirmed)
        {
            return;
        }

        if (currentAttempt == 1 && attempt1Saved)
        {
            return;
        }

        if (currentAttempt == 2 && attempt2Saved)
        {
            return;
        }

        currentAttemptConfirmed = true;

        string fileName = currentAttempt == 1
            ? attempt1FileName
            : attempt2FileName;

        string filePath = Path.Combine(
            dataFolderPath,
            fileName
        );

        EnsureCsvHeader(filePath);

        string date = DateTime.Now.ToString(
            "dd.MM.yyyy",
            CultureInfo.GetCultureInfo("de-DE")
        );

        string time = DateTime.Now.ToString(
            "HH:mm:ss",
            CultureInfo.GetCultureInfo("de-DE")
        );

        string[] values =
        {
            participantNumber,
            date,
            time,
            totalKeyCount.ToString(CultureInfo.InvariantCulture),
            CleanValue(blindsStates[blindsIndex]),
            CleanValue(windowStates[windowIndex]),
            CleanValue(heatingStates[heatingIndex])
        };

        AppendCsvLine(filePath, values);

        if (currentAttempt == 1)
        {
            attempt1Saved = true;
        }
        else
        {
            attempt2Saved = true;
        }

        Debug.Log(
            "Attempt " + currentAttempt +
            " saved: " + filePath
        );
    }

    private void SetAttemptState(int attemptNumber)
    {
        currentAttempt = attemptNumber;
        currentAttemptConfirmed = false;
        totalKeyCount = 0;

        if (attemptNumber == 1)
        {
            heatingIndex = ClampIndex(
                attempt1HeatingStartIndex,
                heatingStates.Length
            );

            blindsIndex = ClampIndex(
                attempt1BlindsStartIndex,
                blindsStates.Length
            );

            windowIndex = ClampIndex(
                attempt1WindowStartIndex,
                windowStates.Length
            );
        }
        else
        {
            heatingIndex = ClampIndex(
                attempt2HeatingStartIndex,
                heatingStates.Length
            );

            blindsIndex = ClampIndex(
                attempt2BlindsStartIndex,
                blindsStates.Length
            );

            windowIndex = ClampIndex(
                attempt2WindowStartIndex,
                windowStates.Length
            );

            Debug.Log("Attempt 2 statistics recording started.");
        }
    }

    private string CreateParticipantNumber()
    {
        string cleanedOverride = CleanValue(
            participantNumberOverride
        );

        if (!string.IsNullOrEmpty(cleanedOverride))
        {
            return cleanedOverride;
        }

        string attempt1Path = Path.Combine(
            dataFolderPath,
            attempt1FileName
        );

        int highestParticipantNumber =
            GetHighestParticipantNumber(attempt1Path);

        return "P" +
            (highestParticipantNumber + 1).ToString(
                "D3",
                CultureInfo.InvariantCulture
            );
    }

    private int GetHighestParticipantNumber(string filePath)
    {
        if (!File.Exists(filePath))
        {
            return 0;
        }

        int highestNumber = 0;
        string[] lines = File.ReadAllLines(
            filePath,
            csvEncoding
        );

        for (int i = 1; i < lines.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(lines[i]))
            {
                continue;
            }

            string[] parts = lines[i].Split(CsvSeparator);

            if (parts.Length < 1)
            {
                continue;
            }

            string participant = CleanValue(
                parts[0].Trim('"')
            );

            if (participant.StartsWith(
                "P",
                StringComparison.OrdinalIgnoreCase))
            {
                participant = participant.Substring(1);
            }

            int parsedNumber;

            if (int.TryParse(
                participant,
                NumberStyles.Integer,
                CultureInfo.InvariantCulture,
                out parsedNumber))
            {
                highestNumber = Mathf.Max(
                    highestNumber,
                    parsedNumber
                );
            }
        }

        return highestNumber;
    }

    private void EnsureCsvHeader(string filePath)
    {
        if (File.Exists(filePath) &&
            new FileInfo(filePath).Length > 0)
        {
            return;
        }

        string[] headers =
        {
            "Participant",
            "Date",
            "Time",
            "TotalKeyCount",
            "FinalBlinds",
            "FinalWindow",
            "FinalHeating"
        };

        WriteCsvLine(filePath, headers);
    }

    private void WriteCsvLine(
        string filePath,
        string[] values)
    {
        string line =
            BuildCsvLine(values) +
            Environment.NewLine;

        File.WriteAllText(
            filePath,
            line,
            csvEncoding
        );
    }

    private void AppendCsvLine(
        string filePath,
        string[] values)
    {
        string prefix = FileNeedsLineBreak(filePath)
            ? Environment.NewLine
            : "";

        string line =
            prefix +
            BuildCsvLine(values) +
            Environment.NewLine;

        File.AppendAllText(
            filePath,
            line,
            csvEncoding
        );
    }

    private bool FileNeedsLineBreak(string filePath)
    {
        if (!File.Exists(filePath))
        {
            return false;
        }

        FileInfo fileInfo = new FileInfo(filePath);

        if (fileInfo.Length == 0)
        {
            return false;
        }

        using (FileStream stream = new FileStream(
            filePath,
            FileMode.Open,
            FileAccess.Read,
            FileShare.ReadWrite))
        {
            stream.Seek(-1, SeekOrigin.End);
            int lastByte = stream.ReadByte();

            return lastByte != '\n' &&
                   lastByte != '\r';
        }
    }

    private string BuildCsvLine(string[] values)
    {
        StringBuilder builder = new StringBuilder();

        for (int i = 0; i < values.Length; i++)
        {
            if (i > 0)
            {
                builder.Append(CsvSeparator);
            }

            builder.Append(
                EscapeCsvValue(values[i])
            );
        }

        return builder.ToString();
    }

    private string EscapeCsvValue(string value)
    {
        value = CleanValue(value);

        bool requiresQuotes =
            value.IndexOf(CsvSeparator) >= 0 ||
            value.Contains("\"") ||
            value.Contains("\n") ||
            value.Contains("\r");

        string escaped = value.Replace(
            "\"",
            "\"\""
        );

        return requiresQuotes
            ? "\"" + escaped + "\""
            : escaped;
    }

    private string CleanValue(string value)
    {
        return value == null
            ? ""
            : value.Trim();
    }

    private int GetNextIndex(
        int currentIndex,
        int arrayLength)
    {
        if (arrayLength <= 0)
        {
            return 0;
        }

        return (currentIndex + 1) % arrayLength;
    }

    private int ClampIndex(
        int index,
        int arrayLength)
    {
        if (arrayLength <= 0)
        {
            return 0;
        }

        return Mathf.Clamp(
            index,
            0,
            arrayLength - 1
        );
    }

    private void ValidateStateArrays()
    {
        if (heatingStates == null ||
            heatingStates.Length == 0)
        {
            heatingStates = new string[]
            {
                "Off"
            };
        }

        if (blindsStates == null ||
            blindsStates.Length == 0)
        {
            blindsStates = new string[]
            {
                "Closed"
            };
        }

        if (windowStates == null ||
            windowStates.Length == 0)
        {
            windowStates = new string[]
            {
                "Closed"
            };
        }
    }
}
