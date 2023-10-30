using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

using TMPro;

#if UNITY_EDITOR
    using UnityEditor;
    using UnityEditor.UIElements;
#endif 

namespace QuestManagement{
    public class ConditionUIElement : MonoBehaviour     
    {
        public ToggleQuestCondition condition; 

        public TMP_Text NameField;

        public bool isConditionCompleted {
            get => _isConditionCompleted;
            set {
                if(value)   NameField.text = " - <color=#6D6D6D><s>" + condition.name + "</s></color> (Выполнено!)";
                else        NameField.text = " - " + condition.name;
                
                _isConditionCompleted = value;
            }
        }

        bool _isConditionCompleted = false;
    }
}