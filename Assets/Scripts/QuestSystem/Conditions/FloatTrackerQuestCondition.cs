using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


namespace QuestManager{
    public class FloatTrackerQuestCondition : TrackerQuestCondition
    {
        [SerializeField] protected MonoBehaviour trackingObject;
        [SerializeField] protected string TrackingFieldName;

        [SerializeField] float RequeueValue;
        
        public override bool ValueTrack() { 
            if(trackingObject == null) return false;
            
            float value;

            var field = trackingObject.GetType().GetField(TrackingFieldName);
            if(field != null) {
                value = (float)field.GetValue(trackingObject);

                return RequeueValue == value;
            }

            var Property = trackingObject.GetType().GetProperty(TrackingFieldName);
            if(Property != null) {
                value = (float)Property.GetValue(trackingObject);

                return RequeueValue == value;
            }

            return false;

        }   
    }
}
