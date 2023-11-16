using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TMP_IntText : TMP_Text
{
    public new int text 
    {
        get => int.Parse(base.text);
        set
        {
            base.text = value.ToString();    
        }
    }
}
