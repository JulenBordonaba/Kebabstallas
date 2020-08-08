using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EffectIcon : MonoBehaviour
{
    public EffectData effect;
    public Image icon;
    public TextMeshProUGUI durationText;
    public Coroutine durationCoroutine;
    public float currentDuration;

    private void Awake()
    {
        if (!icon)
        {
            icon = GetComponentInChildren<Image>();
        }
        if (!durationText)
        {
            durationText = GetComponent<TextMeshProUGUI>();
        }
    }

    private void Update()
    {
        if (effect.permanent)
        {
            durationText.text = "";
        }
        else
        {
            durationText.text = Mathf.FloorToInt(currentDuration).ToString();
        }

    }


}

