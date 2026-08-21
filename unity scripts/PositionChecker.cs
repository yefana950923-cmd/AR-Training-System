using UnityEngine;

public class PositionChecker : MonoBehaviour
{
    void Start()
    {
        Debug.Log("Object = " + gameObject.name);
        Debug.Log("Local Position = " + transform.localPosition);
        Debug.Log("World Position = " + transform.position);

        Transform p = transform.parent;
        while (p != null)
        {
            Debug.Log("Parent: " + p.name 
                      + " | local = " + p.localPosition
                      + " | world = " + p.position
                      + " | scale = " + p.lossyScale);
            p = p.parent;
        }

        Renderer r = GetComponent<Renderer>();
        if (r != null)
        {
            Debug.Log("Renderer Bounds Center = " + r.bounds.center);
            Debug.Log("Renderer Bounds Size = " + r.bounds.size);
        }
    }
}