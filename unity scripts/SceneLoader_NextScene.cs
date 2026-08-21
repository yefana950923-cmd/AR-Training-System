using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader_NextScene : MonoBehaviour
{
    void Update()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            int nextSceneIndex = currentSceneIndex + 1;

            if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
            {
                SceneManager.LoadScene(nextSceneIndex);
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            int previousSceneIndex = currentSceneIndex - 1;

            if (previousSceneIndex >= 0)
            {
                SceneManager.LoadScene(previousSceneIndex);
            }
        }
    }
}