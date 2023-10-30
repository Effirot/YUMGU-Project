using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


namespace QuestManagement{
    public class DoubleTrackerQuestCondition : TrackerQuestCondition
    {
        [SerializeField] protected MonoBehaviour trackingObject;
        [SerializeField] protected string TrackingFieldName;

        [SerializeField] double RequeueValue;
        
        public override bool ValueTrack() { 
            if(trackingObject == null) return false;
            
            double value;

            var field = trackingObject.GetType().GetField(TrackingFieldName);
            if(field != null) {
                value = (double)field.GetValue(trackingObject);

                return RequeueValue == value;
            }

            var Property = trackingObject.GetType().GetProperty(TrackingFieldName);
            if(Property != null) {
                value = (double)Property.GetValue(trackingObject);

                return RequeueValue == value;
            }

            return false;

        }   
    }
}
