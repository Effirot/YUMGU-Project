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

namespace QuestManager{
    public class Quest : MonoBehaviour, IEnumerable<QuestCondition> {
        
        public string Name = "New Quest";
        [TextArea(3, 10)]
        public string Description;
        public bool OnlyExternalComplete = false;

        [Space(10)]
        public UnityEvent OnQuestStart = new UnityEvent();
        public UnityEvent OnQuestComplete = new UnityEvent();
        [SerializeReference]
        public List<QuestCondition> conditionsList = new List<QuestCondition>();

        public QuestSystem questSystem => GetComponentInParent<QuestSystem>();

        public void ForcedComplete(){
            if(!gameObject.activeInHierarchy) return;

            questSystem.NextQuest();
            OnQuestComplete.Invoke();
        }

        public void CheckConditions(){

            // CheckIncludes();

            if(!conditionsList.Any()) return;
            if(!conditionsList.All(a=>a.IsCompleted)) return; 
            // if(questSystem.CurrentQuest != this) return;
            if(OnlyExternalComplete) return;

            questSystem.NextQuest();
            OnQuestComplete.Invoke();
        }
        public void ClearConditions() {
            while(conditionsList.Any()){
                
                if(Application.isPlaying)
                    GameObject.Destroy(conditionsList[0].gameObject);
                else 
                    DestroyImmediate(conditionsList[0].gameObject);

                conditionsList.RemoveAt(0);
            }
        }
        private void CheckIncludes(){
            int index = 0;
            while(index < conditionsList.Count){
                if(conditionsList[index] != null) index++;
                else conditionsList.RemoveAt(index);
            }
        }
        
        // private void OnValidate() {
        //     gameObject.name = Name;
        //     CheckConditions();
        //     conditionsList.ForEach(a=>a.quest = this);
        // }

        public IEnumerator<QuestCondition> GetEnumerator() => conditionsList.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => conditionsList.GetEnumerator();
    }



#if UNITY_EDITOR

    [CustomEditor(typeof(Quest)), CanEditMultipleObjects]
    public class Quest_Editor : Editor{
        new Quest target => (Quest)base.target; 

        Type[] conditionSubTypes;

        private void OnEnable() {
            
            conditionSubTypes = typeof(QuestCondition).Assembly.GetTypes().Where(type =>  (type == typeof(QuestCondition) || type.IsSubclassOf(typeof(QuestCondition))) && !type.IsAbstract).ToArray();
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
                QuestCondition quest = (QuestCondition)obj.AddComponent(conditionSubTypes[selectedTypeIndex - 1]);
                target.conditionsList.Add(quest);
                quest.quest = target;

                Selection.objects = new UnityEngine.Object[] { obj };
            }
            if(GUILayout.Button("Clear Conditions"))
                target.ClearConditions();
            GUILayout.EndHorizontal();


            base.OnInspectorGUI();
        }

    }

#endif 

}


