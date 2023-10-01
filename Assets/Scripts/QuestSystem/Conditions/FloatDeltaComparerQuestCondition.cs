using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QuestManager{

    public class FloatDeltaComparerQuestCondition : QuestCondition
    {
        [SerializeField] private float MinDelta = 0;
        
        private float Delta = 0;

        public void SetNewValue(float newValue)
        {
            Delta = newValue - Delta; 

            if(!gameObject.activeInHierarchy)
                IsCompleted = false;
            else
                IsCompleted = Mathf.Abs(MinDelta) <= Mathf.Abs(Delta);
        }
    }
}