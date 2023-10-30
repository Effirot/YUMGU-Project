using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using QuestManagement;

namespace QuestManagement{
    public class TimeCondition : ToggleQuestCondition
    {
        public float Time;

        private void OnEnable() {
            
            StartCoroutine(Timer(Time));
            
        }

        IEnumerator Timer(float seconds){
            yield return new WaitForSeconds(seconds);

            IsCompleted = !IsCompleted;
        }
    }
}