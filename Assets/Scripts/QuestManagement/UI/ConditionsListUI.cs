using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


#if UNITY_EDITOR
    using UnityEditor;
    using UnityEditor.UIElements;
#endif 

namespace QuestManagement{
    public class QuestUI : MonoBehaviour{

        public MissionUI _questInfoUI;

        public GameObject conditionUIElementPrefab;

        private List<ConditionUIElement> conditionsInfoUI = new List<ConditionUIElement>();
        private Quest SubscribedQuest;


        private void OnEnable()
        {
            SubscribeToSingletonQuest(Quest.Singleton);

            Quest.OnQuestSelected += SubscribeToSingletonQuest;
        }

        private void OnDisable()
        {
            ClearConditions();

            Quest.OnQuestSelected -= SubscribeToSingletonQuest;
        }

        private void SubscribeToSingletonQuest(Quest targetQuest)
        {
            SubscribedQuest?.OnMissionSelected.RemoveListener(RegenerateConditions);

            SubscribedQuest = targetQuest;

            if(SubscribedQuest == null)
            {
                Debug.LogWarning("Quest system is not assigned now");
            }
            else
            {
                RegenerateConditions(SubscribedQuest.SelectedMission);
                SubscribedQuest.OnMissionSelected.AddListener(RegenerateConditions);
            }
        }

        private void ClearConditions()
        {
            while(conditionsInfoUI.Any())
            {
                if(Application.isPlaying)
                    GameObject.Destroy(conditionsInfoUI[0]?.gameObject);
                else 
                    GameObject.DestroyImmediate(conditionsInfoUI[0]?.gameObject);

                conditionsInfoUI.RemoveAt(0);
            }
        }
        private void RegenerateConditions(Mission mission)
        {
            if(null == mission) return;

            ClearConditions();

            _questInfoUI.NameField.text = mission.Name;
            _questInfoUI.DescriptionField.text = mission.Description;

            foreach(var condition in mission.conditionsList) 
            {
                if(condition.isVisible)
                {
                    GenerateConditionsInfo(condition);
                }
            }
        }
        private void GenerateConditionsInfo(ToggleQuestCondition condition){
            if(condition == null) return;

            var obj = Instantiate(conditionUIElementPrefab, transform).GetComponent<ConditionUIElement>();
            conditionsInfoUI.Add(obj);

            obj.NameField.text = condition.Name;
            obj.name = condition.Name;
            obj.condition = condition;
            obj.isConditionCompleted = condition.IsCompleted;
            
            condition.OnConditionChanged.AddListener((sender, value) => obj.isConditionCompleted = value);
        }

    }
}