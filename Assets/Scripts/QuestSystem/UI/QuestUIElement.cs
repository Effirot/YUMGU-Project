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


namespace QuestManager{
    public class QuestUIElement : MonoBehaviour     
    {
        public Quest QuestLink; 

        public TMP_Text NameField;
        public TMP_Text DescriptionField;
    }
}