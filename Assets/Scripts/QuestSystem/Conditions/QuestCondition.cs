using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


namespace QuestManager{

    public class QuestCondition : MonoBehaviour{
        [TextArea(1, 3)]
        public string Name = "New Condition";
        public bool Optional = false;
        
        public bool isVisible = true;

        public ConditionCompleteEvent OnConditionChanged = new ConditionCompleteEvent(); 

        public virtual bool IsCompleted { 
            get => _IsCompleted; 
            set {
                _IsCompleted = value;
                OnConditionChanged.Invoke(value);

                if(gameObject.activeInHierarchy)
                    quest.CheckConditions();

            }
        }

        public Quest quest;
        
        bool _IsCompleted = false; 


        // protected virtual void OnValidate() {
        //     if(Name.Length > 0)
        //         gameObject.name = Name;
        //     else gameObject.name = " ";
        // }

    }

    
    [Serializable]
    public class ConditionCompleteEvent : UnityEvent<bool> { }

    public abstract class TrackerQuestCondition : QuestCondition {
        public new bool IsCompleted { 
            get => base.IsCompleted; 
            private set => base.IsCompleted = value; }

        public abstract bool ValueTrack();

        private void Awake() {
            StartCoroutine(CheckParameter());
        }

        private void OnEnable() {
            StopAllCoroutines();
            StartCoroutine(CheckParameter());
        }

        // protected override void OnValidate() {
        //     base.OnValidate(); 
        //     IsCompleted = ValueTrack();
        // }

        IEnumerator<CustomYieldInstruction> CheckParameter(){
            while(true){
                try {
                    IsCompleted = ValueTrack();
                }
                catch (Exception e) { 
                    Debug.LogWarning(e); 
                }
                
                yield return null;
            }
        }

    }

}