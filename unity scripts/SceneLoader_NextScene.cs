using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SceneLoader_NextScene : MonoBehaviour
{
    [Header("Scene Change Condition")]

    [Tooltip("Scene 0 拖入问卷 TMP；Scene 1 拖入显示 Tips 的 TMP")]
    [SerializeField] private TMP_Text conditionText;

    [Tooltip("填写完成当前场景时，TMP 中一定会出现的独特文字")]
    [TextArea(2, 5)]
    [SerializeField] private string completionMarker;

    private bool conditionWasMetLastFrame = false;
    private bool isLoadingNextScene = false;

    private void Update()
    {
        if (isLoadingNextScene)
        {
            return;
        }

        bool conditionIsMetNow = IsCompletionConditionMet();

        /*
         * 条件必须连续至少两帧成立。
         *
         * 这样在 Scene 0 中按下箭头切换到最后一个 element 时，
         * 不会在同一次按键中直接跳到下一个场景。
         */
        if (Input.GetKeyDown(KeyCode.DownArrow) &&
            conditionWasMetLastFrame &&
            conditionIsMetNow)
        {
            LoadNextScene();
            return;
        }

        conditionWasMetLastFrame = conditionIsMetNow;
    }

    private bool IsCompletionConditionMet()
    {
        if (conditionText == null)
        {
            return false;
        }

        if (string.IsNullOrWhiteSpace(completionMarker))
        {
            return false;
        }

        string currentText = conditionText.text;

        if (string.IsNullOrWhiteSpace(currentText))
        {
            return false;
        }

        return currentText.IndexOf(
            completionMarker.Trim(),
            StringComparison.OrdinalIgnoreCase
        ) >= 0;
    }

    private void LoadNextScene()
    {
        int currentSceneIndex =
            SceneManager.GetActiveScene().buildIndex;

        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex >=
            SceneManager.sceneCountInBuildSettings)
        {
            Debug.LogWarning(
                "SceneLoader_NextScene: No next scene exists in Build Settings."
            );

            return;
        }

        isLoadingNextScene = true;

        SceneManager.LoadScene(nextSceneIndex);
    }
}