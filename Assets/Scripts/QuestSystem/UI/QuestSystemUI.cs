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

namespace QuestManager{
    public class QuestSystemUI : MonoBehaviour{

        public QuestUIElement _questInfoUI;

        public GameObject conditionUIElementPrefab;

        List<ConditionUIElement> conditionsInfoUI = new List<ConditionUIElement>();
        QuestSystem questSystem => QuestSystem.Current;


        public void ClearConditions(){
            while(conditionsInfoUI.Any())
            {
                if(Application.isPlaying)
                    GameObject.Destroy(conditionsInfoUI[0]?.gameObject);
                else 
                    GameObject.DestroyImmediate(conditionsInfoUI[0]?.gameObject);

                conditionsInfoUI.RemoveAt(0);
            }
        }

        public void RegenerateConditions(Quest quest){
            
            if(null == quest) return;

            ClearConditions();

            _questInfoUI.NameField.text = questSystem.CurrentQuest.Name;
            _questInfoUI.DescriptionField.text = questSystem.CurrentQuest.Description;

            foreach(var condition in questSystem.CurrentQuest.conditionsList) 
                if(condition.isVisible)
                    generateConditionsInfo(condition);
            

            void generateConditionsInfo(QuestCondition condition){
                var obj = Instantiate(conditionUIElementPrefab, transform).GetComponent<ConditionUIElement>();
                obj.NameField.text = condition.Name;
                obj.name = condition.Name;
                obj.condition = condition;
                obj.isConditionCompleted = condition.IsCompleted;
                
                condition.OnConditionChanged.AddListener(a => obj.isConditionCompleted = a);

                conditionsInfoUI.Add(obj);
            }
        }

        private void Start() {
            RegenerateConditions(QuestSystem.Current.CurrentQuest);
            QuestSystem.Current.OnQuestSelected.AddListener(RegenerateConditions);
        }
    }
}