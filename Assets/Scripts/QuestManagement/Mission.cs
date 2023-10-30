using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Reflection;


#if UNITY_EDITOR
    using UnityEditor;
    using UnityEditor.UIElements;
#endif 

namespace QuestManagement{

    public class Mission : MonoBehaviour, IEnumerable<ToggleQuestCondition> {
        
        public string Name = "New Quest";
        [TextArea(3, 10)]
        public string Description;
        public bool OnlyExternalComplete = false;

        [Space(10)]
        public UnityEvent OnMissionStart = new UnityEvent();
        public UnityEvent OnMissionComplete = new UnityEvent();

        [SerializeReference]
        public List<ToggleQuestCondition> conditionsList = new List<ToggleQuestCondition>();
        
        [System.NonSerialized]
        public Quest Quest;

        public void ForcedComplete(){
            if(!gameObject.activeInHierarchy) return;

            Quest.NextMission();
            OnMissionComplete.Invoke();
        }

        public void CheckConditions(){

            CheckIncludes();

            if(!conditionsList.Any()) return;
            if(!conditionsList.All(a=>a.IsCompleted)) return; 
            // if(questSystem.CurrentQuest != this) return;
            if(OnlyExternalComplete) return;

            Quest.NextMission();
            OnMissionComplete.Invoke();
        }
        public void CheckIncludes(){
            int index = 0;
            while(index < conditionsList.Count){

                if(conditionsList[index] != null) 
                {
                    conditionsList[index].mission = this;
                    index++;
                }
                else 
                {
                    conditionsList.RemoveAt(index);
                }
            }
        }
        
        private void OnValidate() {
            gameObject.name = Name;
            CheckConditions();
        }

        public IEnumerator<ToggleQuestCondition> GetEnumerator() => conditionsList.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => conditionsList.GetEnumerator();
    }



#if UNITY_EDITOR

    [CustomEditor(typeof(Mission)), CanEditMultipleObjects]
    public class Quest_Editor : Editor{
        new Mission target => (Mission)base.target; 

        Type[] conditionSubTypes;

        private void OnEnable() {
            
            conditionSubTypes = typeof(ToggleQuestCondition).Assembly.GetTypes().Where(type =>  (type == typeof(ToggleQuestCondition) || type.IsSubclassOf(typeof(ToggleQuestCondition))) && !type.IsAbstract).ToArray();
        }

        public override void OnInspectorGUI()
        {
            if(Application.isPlaying && target.gameObject.activeInHierarchy)
            {
                if(GUILayout.Button("Force complete"))
                {
                    target.ForcedComplete();
                }
            }

            GUILayout.BeginHorizontal();
            
            var selectedTypeIndex = EditorGUILayout.Popup(0, new string[] { "Add Condition" }.Union(conditionSubTypes.Select(a=>a.Name)).ToArray());
            if(selectedTypeIndex > 0)
            {
                GameObject obj = new GameObject("New Quest");
                obj.transform.parent = target.transform;
                obj.transform.SetAsLastSibling();
                ToggleQuestCondition quest = (ToggleQuestCondition)obj.AddComponent(conditionSubTypes[selectedTypeIndex - 1]);
                target.conditionsList.Add(quest);
                quest.mission = target;

                Selection.objects = new UnityEngine.Object[] { obj };
            }
            if(GUILayout.Button("Clear Conditions"))
                ClearConditions();
            GUILayout.EndHorizontal();


            base.OnInspectorGUI();
        }

        private void ClearConditions() {
            while(target.conditionsList.Any()){
                
                if(Application.isPlaying)
                    GameObject.Destroy(target.conditionsList[0].gameObject);
                else 
                    DestroyImmediate(target.conditionsList[0].gameObject);

                target.conditionsList.RemoveAt(0);
            }
        }

    }

#endif 

}


