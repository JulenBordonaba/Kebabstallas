using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectorLine : MonoBehaviour
{
    protected LineRenderer line;
    protected GameObject SelectorDeCasillas;
    // Start is called before the first frame update
    void Start()
    {
        line = transform.GetComponent<LineRenderer>();
        SelectorDeCasillas = GameObject.Find("Selector");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0) && GameController.Selected != null)
        {
            Vector3 v2 = new Vector3(GameController.Selected.transform.position.x, GameController.Selected.transform.position.y, 0.001f);
            line.SetPosition(0, v2);
            //Vector3 v3 = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0.001f));
            line.SetPosition(1, new Vector3(SelectorDeCasillas.transform.position.x, SelectorDeCasillas.transform.position.y, 0.001f));
        }
        else
        {
            line.SetPositions(new Vector3[] { Vector3.zero, Vector3.zero });
        }
    }
}
