using UnityEngine;

public class size : MonoBehaviour
{
    void Start()
    {
        Debug.Log(GetComponent<Renderer>().bounds.size);
    }
}