using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class FloatingText : MonoBehaviour
{
    private TMP_Text text;

    private void Awake()
    {
        text = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        var color = text.color;

        color.a /= 1.08f;

        text.color = color;

        if (color.a <= 0.01f)
        {
            Destroy(gameObject);
        }
    }
}
