﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanZoom : MonoBehaviour
{
    Vector3 touchStart;
    public float zoomOutMin = 0.4f;
    public float zoomOutMax = 1f;
    public bool CanZoom = true;
    public Slider ZoomSlider;
    public Vector3 direction;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (CanZoom)
        {
            if (Input.GetMouseButtonDown(0))
            {
                touchStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }
            if (Input.touchCount == 2)
            {
                Touch touchZero = Input.GetTouch(0);
                Touch touchOne = Input.GetTouch(1);

                Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
                Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

                float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
                float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

                float difference = currentMagnitude - prevMagnitude;

                zoom(difference * 0.002f);
            }
            else
            {
                if (Input.GetMouseButton(0))
                {
                    direction = touchStart - Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    Camera.main.transform.position += direction;

                }
            }
            zoom(Input.GetAxis("Mouse ScrollWheel"));
            

            Camera.main.transform.position = new Vector3(Mathf.Clamp(Camera.main.transform.position.x, 0.001f + Camera.main.orthographicSize, 2.01f - Camera.main.orthographicSize), Mathf.Clamp(Camera.main.transform.position.y, 0.01f + Camera.main.orthographicSize, 2.01f - Camera.main.orthographicSize), -10f);

        }
    }

    void zoom(float increment)
    {
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - increment, zoomOutMin, zoomOutMax);
        
    }

    public void Zoom()
    {
        Camera.main.orthographicSize = 1 -  ZoomSlider.value;
    }
}
