using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scene2_Survey : MonoBehaviour
{
    public GameObject[] questions;
    int index;

    // Start is called before the first frame update
    void Start()
    {
        index = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (index >= 37)
             index = 37;

    
        if (index < 0)
            index = 0;

        if(index == 0)
        {
            questions[0].gameObject.SetActive(true);
        }
        
       
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                index += 1;

                for (int i = 0; i < questions.Length; i++)
                {
                    questions[i].gameObject.SetActive(false);
                    questions[index].gameObject.SetActive(true);
                }
                Debug.Log(index);
         
        }
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                index -= 1;

                for (int i = 0; i < questions.Length; i++)
                {
                    questions[i].gameObject.SetActive(false);
                    questions[index].gameObject.SetActive(true);
                }
                Debug.Log(index);
         
            }
    }

    
}
