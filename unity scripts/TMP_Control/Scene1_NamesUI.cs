using TMPro;
using UnityEngine;

public class Scene1_NamesUI : MonoBehaviour
{
    [Header("Status Texts")]
    [SerializeField] private TMP_Text windowText;
    [SerializeField] private TMP_Text blindsText;
    [SerializeField] private TMP_Text heatingText;

    // 保存主交互脚本传来的最新状态
    private int currentHeatingLevel = 3;
    private bool currentWindowOpen = false;
    private bool currentBlindsOpen = true;

    private bool hasReceivedState = false;

    /*
     * 勾选此组件时：
     * 显示三个 TMP，并恢复最新状态。
     */
    private void OnEnable()
    {
        SetTextsVisible(true);

        if (hasReceivedState)
        {
            ApplyUI();
        }
    }

    /*
     * 取消此组件的勾选时：
     * 隐藏三个 TMP。
     */
    private void OnDisable()
    {
        SetTextsVisible(false);
    }

    /*
     * 由 Scene1_StatusAndComfort 主动调用。
     * 本脚本不读取键盘，也不单独控制状态。
     */
    public void UpdateUI(
        int heatingLevel,
        bool windowOpen,
        bool blindsOpen)
    {
        // 即使组件当前被取消勾选，也保存最新状态
        currentHeatingLevel = heatingLevel;
        currentWindowOpen = windowOpen;
        currentBlindsOpen = blindsOpen;

        hasReceivedState = true;

        // 取消勾选时只保存，不更新显示
        if (!isActiveAndEnabled)
        {
            return;
        }

        ApplyUI();
    }

    private void ApplyUI()
    {
        UpdateHeatingText();
        UpdateWindowText();
        UpdateBlindsText();
    }

    private void UpdateHeatingText()
    {
        if (heatingText == null)
        {
            return;
        }

        if (currentHeatingLevel == 0)
        {
            heatingText.text =
                "Thermostatic Heating ▼\nOff";
        }
        else
        {
            heatingText.text =
                "Thermostatic Heating ▼\nLevel " +
                currentHeatingLevel;
        }
    }

    private void UpdateWindowText()
    {
        if (windowText == null)
        {
            return;
        }

        windowText.text =
            currentWindowOpen
                ? "Window ▶\nOpen"
                : "Window ▶\nClosed";
    }

    private void UpdateBlindsText()
    {
        if (blindsText == null)
        {
            return;
        }

        blindsText.text =
            currentBlindsOpen
                ? "◀ Blinds\nOpen"
                : "◀ Blinds\nClosed";
    }

    private void SetTextsVisible(bool visible)
    {
        if (heatingText != null)
        {
            heatingText.enabled = visible;
        }

        if (windowText != null)
        {
            windowText.enabled = visible;
        }

        if (blindsText != null)
        {
            blindsText.enabled = visible;
        }
    }
}