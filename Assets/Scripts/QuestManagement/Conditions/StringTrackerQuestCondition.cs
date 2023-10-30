using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#if UNITY_EDITOR
    using UnityEditor;
#endif

namespace QuestManagement{
    public class StringTrackerQuestCondition : TrackerQuestCondition
    {
        [SerializeField] protected MonoBehaviour trackingObject;
        [SerializeField] protected string TrackingFieldName;
        
        [SerializeField] string RequeueValue;
        
        public override bool ValueTrack() { 
            if(trackingObject == null)
                return false;

            var property = trackingObject.GetType().GetProperty(TrackingFieldName);
            if(property != null)
                return RequeueValue == (string)property.GetValue(trackingObject);
            
            var field = trackingObject.GetType().GetField(TrackingFieldName);
            if(field != null)
                return RequeueValue == (string)field.GetValue(trackingObject);

            return false;
        }
    }
}