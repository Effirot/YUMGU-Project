using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using QuestManager;
using TMPro;
using UnityEngine;

public class RegexTextTrackerQuestCondition : TrackerQuestCondition
{
    [SerializeField] private TMP_Text textField;
    [SerializeField, TextArea(1, 10)] private string Pattern;

    public override bool ValueTrack()
    {
        if(Pattern.Length == 0 || textField.text.Length == 0)
            return false;
        return Regex.IsMatch(textField.text, Pattern);
    }
}
