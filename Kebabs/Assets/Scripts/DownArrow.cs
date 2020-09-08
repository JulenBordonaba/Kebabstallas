using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownArrow : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator Select(GameObject soldier)
    {
        transform.parent = soldier.transform;
        transform.position = soldier.transform.position + Vector3.up * 0.1f;
        yield return new WaitForSeconds(1);
        Destroy(this.gameObject);
    }
}
