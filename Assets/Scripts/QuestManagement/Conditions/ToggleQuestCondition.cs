using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


namespace QuestManagement{

    public class ToggleQuestCondition : MonoBehaviour{

        public class ConditionCompleteEvent : UnityEvent<ToggleQuestCondition, bool> { }

        [TextArea(1, 3)]
        public string Name = "New Condition";
        public bool Optional = false;
        
        public bool isVisible = true;

        public ConditionCompleteEvent OnConditionChanged = new ConditionCompleteEvent(); 

        public virtual bool IsCompleted { 
            get => _IsCompleted; 
            set {
                _IsCompleted = value;
                OnConditionChanged.Invoke(this, value);

                if(gameObject.activeInHierarchy)
                    mission.CheckConditions();
            }
        }

        [System.NonSerialized]
        public Mission mission;
        
        bool _IsCompleted = false; 


        // protected virtual void OnValidate() {
        //     if(Name.Length > 0)
        //     {
        //         gameObject.name = Name;
        //     }
        //     else 
        //     { 
        //         gameObject.name = " ";
        //     }
        // }

    }
}