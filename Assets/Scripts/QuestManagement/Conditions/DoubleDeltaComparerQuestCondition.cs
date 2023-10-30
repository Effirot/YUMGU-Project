using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QuestManagement{

    public class DoubleDeltaComparerQuestCondition : ToggleQuestCondition
    {
        [SerializeField] private double MinDelta = 0;
        
        private double Delta = 0;

        public void SetNewValue(double newValue)
        {
            Delta = newValue - Delta; 

            if(!gameObject.activeInHierarchy)
                IsCompleted = false;
            else
                IsCompleted = Math.Abs(MinDelta) <= Math.Abs(Delta);
        }
    }
}