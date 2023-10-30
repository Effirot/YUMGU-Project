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
    public class MissionUI : MonoBehaviour     
    {
        public Mission QuestLink; 

        public TMP_Text NameField;
        public TMP_Text DescriptionField;
    }
}