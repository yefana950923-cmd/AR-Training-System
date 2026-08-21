using UnityEngine;

public class Scene0_Survey : MonoBehaviour
{
    public GameObject[] questions;

    private int index = 0;

    void Start()
    {
        ShowQuestion(0);
    }

    void Update()
    {
        // 下一题
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (index < questions.Length - 1)
            {
                index++;
                ShowQuestion(index);
            }
        }

        // 上一题
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (index > 0)
            {
                index--;
                ShowQuestion(index);
            }
        }
    }

    private void ShowQuestion(int questionIndex)
    {
        // 先关闭所有问题
        for (int i = 0; i < questions.Length; i++)
        {
            if (questions[i] != null)
            {
                questions[i].SetActive(false);
            }
        }

        // 再显示当前问题
        if (questionIndex >= 0 &&
            questionIndex < questions.Length &&
            questions[questionIndex] != null)
        {
            questions[questionIndex].SetActive(true);
        }
    }
}