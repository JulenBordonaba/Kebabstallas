using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonBlock : MonoBehaviour
{
    public int myNum;
    // Start is called before the first frame update
    void Awake()
    {
        int state = GameController.ld.levels[myNum - 1];
        if (state >= 1)
        {
            transform.GetComponent<Button>().interactable = true;
            if (state == 2)
            {
                transform.GetComponent<Image>().color = Color.blue;
            }
        }
        else if (state == 0)
        {
            transform.GetComponent<Button>().interactable = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
