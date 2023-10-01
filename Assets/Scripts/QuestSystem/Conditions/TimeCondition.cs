using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using QuestManager;

namespace QuestManager{
    public class TimeCondition : QuestCondition
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