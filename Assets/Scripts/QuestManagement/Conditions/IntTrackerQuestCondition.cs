using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


namespace QuestManagement{
    public class IntTrackerQuestCondition : TrackerQuestCondition
    {
        [SerializeField] protected MonoBehaviour trackingObject;
        [SerializeField] protected string TrackingFieldName;

        [SerializeField] int RequeueValue;
        
        public override bool ValueTrack() { 
            if(trackingObject == null) return false;
            
            int value;

            var field = trackingObject.GetType().GetField(TrackingFieldName);
            if(field != null) {
                value = (int)field.GetValue(trackingObject);

                return RequeueValue == value;
            }

            var Property = trackingObject.GetType().GetProperty(TrackingFieldName);
            if(Property != null) {
                value = (int)Property.GetValue(trackingObject);

                return RequeueValue == value;
            }

            return false;

        }   
    }
}
