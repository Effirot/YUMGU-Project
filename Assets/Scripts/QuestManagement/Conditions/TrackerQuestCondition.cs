using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


namespace QuestManagement{
        
    [Serializable]
    public abstract class TrackerQuestCondition : ToggleQuestCondition {
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