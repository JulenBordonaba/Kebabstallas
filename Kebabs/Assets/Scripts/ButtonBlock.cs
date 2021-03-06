﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonBlock : MonoBehaviour
{
    public int myNum;
    // Start is called before the first frame update
    void Awake()
    {
        if (this.name == "BossButton" || this.name == "PartidaPersonalizada")
        {
            if (GameController.ld.levels[49] == 1)
            {
                transform.GetComponent<Button>().interactable = true;
            }
            else
            {
                transform.GetComponent<Button>().interactable = false;
            }
        }
        else
        {
            //myNum += 50;
            //this.name = "Level Button (" + myNum + ")";
            
            //transform.Find("Text").GetComponent<Text>().text = myNum + 50 + "";
            int state = GameController.ld.levels[myNum - 1];
            if (state >= 1)
            {
                transform.GetComponent<Button>().interactable = true;
                if (state == 2)
                {
                    transform.GetComponent<Image>().color = Color.yellow;
                }
            }
            else if (state == 0)
            {
                transform.GetComponent<Button>().interactable = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
