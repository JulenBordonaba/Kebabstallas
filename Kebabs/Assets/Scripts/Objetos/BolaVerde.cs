using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BolaVerde : MonoBehaviour
{

    float elapsedTime = 0;
    float waitTime = 1f;
    Vector3 Gotoposition = new Vector3(0f, 2.4f, 0f);

    Vector3 currentPos;
    // Start is called before the first frame update
    void Awake()
    {
        currentPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime < waitTime)
        {
            transform.position = Vector3.Lerp(currentPos, Gotoposition, (elapsedTime / waitTime));
        }
        else if (elapsedTime > 1.7f)
        {
            Destroy(this.gameObject);
            transform.position = Gotoposition;
        }
        
        
    }
}
