using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RandomTipsOnTV : MonoBehaviour
{
    [Header("电视上的 TextMeshPro 文本组件")]
    [SerializeField] private TMP_Text TextTips;

    [Header("Tips 设置")]
    [Tooltip(
        "Element 0：固定初始提示；" +
        "Element 1 到倒数第二个：循环随机显示的 Tips；" +
        "最后一个 Element：按 Return 后显示的最终 Tip。"
    )]
    [TextArea(3, 8)]
    [SerializeField] private string[] tips;

    // 当前一轮随机排列后的 Tip 编号
    private readonly List<int> randomTipOrder = new List<int>();

    // 当前一轮中下一条 Tip 的位置
    private int currentTipPosition = 0;

    // 上一次显示的随机 Tip 编号
    // 用于避免两轮交界处连续出现同一条 Tip
    private int lastShownTipIndex = -1;

    // 已经显示的随机 Tip 总数
    // 也就是用户按下 W、B、H 的有效次数
    private int randomTipCount = 0;

    // 按下 Return 后锁定电视内容
    private bool isLockedByReturn = false;

    private void Start()
    {
        FindTextTipsIfNecessary();

        randomTipCount = 0;
        lastShownTipIndex = -1;
        isLockedByReturn = false;

        CreateRandomTipOrder();
        ShowInitialTip();

        Debug.Log(
            "RandomTipsOnTV started. Total W/B/H presses = 0"
        );
    }

    private void Update()
    {
        // 按下 Return 后，电视内容已经锁定
        if (isLockedByReturn)
        {
            return;
        }

        // 苹果键盘 Return：
        // 显示最后一个 Element，并永久锁定电视内容
        if (Input.GetKeyDown(KeyCode.Return))
        {
            ShowFinalTipAndLock();
            return;
        }

        // W、B、H 中任意一个键：
        // 显示下一条随机 Tip
        if (Input.GetKeyDown(KeyCode.W) ||
            Input.GetKeyDown(KeyCode.B) ||
            Input.GetKeyDown(KeyCode.H))
        {
            ShowNextRandomTip();
        }
    }

    /// <summary>
    /// 如果 Inspector 中没有手动绑定 TextTips，
    /// 自动寻找名称为 TextTips 的 TextMeshPro 对象。
    /// </summary>
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
                "RandomTipsOnTV：没有找到名称为 TextTips 的 TextMeshPro 对象。"
            );
        }
    }

    /// <summary>
    /// 场景开始时固定显示 Element 0。
    /// Element 0 不参与随机。
    /// </summary>
    private void ShowInitialTip()
    {
        if (TextTips == null)
        {
            return;
        }

        if (tips != null &&
            tips.Length > 0 &&
            !string.IsNullOrWhiteSpace(tips[0]))
        {
            TextTips.text = tips[0];
        }
        else
        {
            TextTips.text = "";

            Debug.LogWarning(
                "RandomTipsOnTV：Element 0 为空，请填写固定初始提示。"
            );
        }
    }

    /// <summary>
    /// 创建新一轮随机 Tip 顺序。
    ///
    /// Element 0 不参与随机。
    /// 最后一个 Element 是 Return 最终提示，也不参与随机。
    /// </summary>
    private void CreateRandomTipOrder()
    {
        randomTipOrder.Clear();
        currentTipPosition = 0;

        /*
         * 至少需要三个 Element：
         *
         * Element 0 = 初始提示
         * Element 1 = 至少一条随机 Tip
         * 最后一个 Element = Return 最终 Tip
         */
        if (tips == null || tips.Length < 3)
        {
            Debug.LogWarning(
                "RandomTipsOnTV：Tips 至少需要三个 Element。"
            );

            return;
        }

        int finalTipIndex = tips.Length - 1;

        // 只加入 Element 1 到倒数第二个 Element
        for (int i = 1; i < finalTipIndex; i++)
        {
            if (!string.IsNullOrWhiteSpace(tips[i]))
            {
                randomTipOrder.Add(i);
            }
        }

        // Fisher-Yates 洗牌
        for (int i = randomTipOrder.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);

            int temporaryValue = randomTipOrder[i];
            randomTipOrder[i] = randomTipOrder[randomIndex];
            randomTipOrder[randomIndex] = temporaryValue;
        }

        /*
         * 如果新一轮第一条与上一轮最后一条相同，
         * 并且存在两条以上的随机 Tips，
         * 则与其他位置交换，避免连续重复。
         */
        if (randomTipOrder.Count > 1 &&
            randomTipOrder[0] == lastShownTipIndex)
        {
            int swapIndex =
                Random.Range(1, randomTipOrder.Count);

            int temporaryValue = randomTipOrder[0];
            randomTipOrder[0] =
                randomTipOrder[swapIndex];
            randomTipOrder[swapIndex] =
                temporaryValue;
        }
    }

    /// <summary>
    /// 显示下一条随机 Tip。
    ///
    /// 同一轮中不会重复。
    /// 一轮全部显示后，自动洗牌并开始下一轮。
    /// </summary>
    private void ShowNextRandomTip()
    {
        if (TextTips == null)
        {
            return;
        }

        if (randomTipOrder.Count == 0)
        {
            Debug.LogWarning(
                "RandomTipsOnTV：没有可用的随机 Tips。"
            );

            return;
        }

        // 当前一轮已经显示完成，自动开始新一轮
        if (currentTipPosition >= randomTipOrder.Count)
        {
            CreateRandomTipOrder();
        }

        if (randomTipOrder.Count == 0)
        {
            return;
        }

        int tipIndex =
            randomTipOrder[currentTipPosition];

        TextTips.text = tips[tipIndex];

        lastShownTipIndex = tipIndex;
        currentTipPosition++;

        // 后台统计随机 Tip 出现次数
        randomTipCount++;

        Debug.Log(
            "Random Tip displayed. Total W/B/H presses = "
            + randomTipCount
        );
    }

    /// <summary>
    /// 按下 Return 后显示最后一个 Element，
    /// 并永久锁定电视提示。
    /// </summary>
    private void ShowFinalTipAndLock()
    {
        if (TextTips == null)
        {
            return;
        }

        if (tips == null || tips.Length < 2)
        {
            Debug.LogWarning(
                "RandomTipsOnTV：没有设置最终 Tip。"
            );

            return;
        }

        int finalTipIndex = tips.Length - 1;

        if (string.IsNullOrWhiteSpace(
            tips[finalTipIndex]))
        {
            Debug.LogWarning(
                "RandomTipsOnTV：最后一个 Element 为空，请填写最终 Tip。"
            );

            return;
        }

        TextTips.text = tips[finalTipIndex];
        isLockedByReturn = true;

        Debug.Log(
            "Return pressed. Tips are now locked. " +
            "Total W/B/H presses = "
            + randomTipCount
        );
    }

    /// <summary>
    /// 供其他脚本读取随机 Tip 的累计显示次数。
    /// </summary>
    public int GetRandomTipCount()
    {
        return randomTipCount;
    }

    /// <summary>
    /// 重新开始整个 Tips 流程。
    /// </summary>
    public void ResetTips()
    {
        randomTipCount = 0;
        lastShownTipIndex = -1;
        isLockedByReturn = false;

        CreateRandomTipOrder();
        ShowInitialTip();

        Debug.Log(
            "Tips reset. Total W/B/H presses = 0"
        );
    }
}