using UnityEngine;

public class MultiDisplay : MonoBehaviour
{
    private static bool displaysActivated = false;

    void Start()
    {
        if (displaysActivated)
        {
            return;
        }

        displaysActivated = true;

        Debug.Log("Displays connected: " + Display.displays.Length);

        // Activate Display 2, Display 3, and Display 4
        for (int i = 1; i <= 3 && i < Display.displays.Length; i++)
        {
            Display.displays[i].Activate();
        }
    }
}
