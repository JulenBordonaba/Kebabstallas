using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selectable : MonoBehaviour {
    
    private SpriteRenderer sr;
    public int myNumber;
    public GameObject GC;

    // Use this for initialization
    void Start () {
        sr = transform.GetComponent<SpriteRenderer>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    /*
    private void OnMouseEnter()
    {
        transform.localScale *= 1.3f;
    }

    private void OnMouseExit()
    {
        transform.localScale /= 1.3f;
    }
    */

    IEnumerator MoveToSpot()
    {
        Vector3 Gotoposition = new Vector3(0f,-3f,0f);
        float elapsedTime = 0;
        float waitTime = 1f;
        Vector3 currentPos = transform.position;

        while (elapsedTime < waitTime)
        {
            transform.position = Vector3.Lerp(currentPos, Gotoposition, (elapsedTime / waitTime));
            elapsedTime += Time.deltaTime;

            // Yield here
            yield return null;
        }
        // Make sure we got there
        transform.position = Gotoposition;
        GC.GetComponent<GameController>().ShowConfirmation(myNumber);
        yield return null;
    }

    private void OnMouseDown()
    {

        switch (tag)
        {
            case "Lider":
                {
                    
                    GameController.LiderSelected(this.gameObject);
                    StartCoroutine("MoveToSpot");
                    break;
                }
            case "Player":
                {
                    sr.color = new Color32(255, 83, 83, 255);
                    tag = "Enemy";
                    break;
                }
            case "Selectable":
                {
                    sr.color = new Color32(129, 255, 133, 255);
                    tag = "Player";
                    break;
                }
            default:
                {
                    sr.color = Color.white;
                    tag = "Selectable";
                    break;
                }
        }
    }
}
