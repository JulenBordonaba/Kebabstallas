using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishSound : MonoBehaviour
{
    private void Awake()
    {
        StartCoroutine("Suicide");
    }

    private IEnumerator Suicide()
    {
        yield return new WaitForSeconds(5f);
        Destroy(this.gameObject);
    }
}
