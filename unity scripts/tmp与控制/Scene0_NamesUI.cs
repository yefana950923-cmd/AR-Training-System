using TMPro;
using UnityEngine;

public class Scene0_NamesUI : MonoBehaviour
{
    [Header("Scene0 State Controller")]
    [SerializeField] private Scene0_Interaction scene0Interaction;

    [Header("Status Texts")]
    [SerializeField] private TMP_Text windowText;
    [SerializeField] private TMP_Text blindsText;
    [SerializeField] private TMP_Text heatingText;

    private void Start()
    {
        RefreshUI();
    }

    /// <summary>
    /// 根据 Scene0_Interaction 中的真实状态刷新UI。
    /// 此脚本不监听 H、W、B，也不单独保存状态。
    /// </summary>
    public void RefreshUI()
    {
        if (scene0Interaction == null)
        {
            Debug.LogWarning(
                "Scene0_NamesUI: Scene0 Interaction has not been assigned."
            );
            return;
        }

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

        int heatingLevel = scene0Interaction.HeatingLevel;

        if (heatingLevel == 0)
        {
            heatingText.text =
                "Thermostatic Heating ▼\nOff";
        }
        else
        {
            heatingText.text =
                "Thermostatic Heating ▼\nLevel " + heatingLevel;
        }
    }

    private void UpdateWindowText()
    {
        if (windowText == null)
        {
            return;
        }

        windowText.text =
            scene0Interaction.IsWindowOpen
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
            scene0Interaction.AreBlindsOpen
                ? "◀ Blinds\nOpen"
                : "◀ Blinds\nClosed";
    }
}