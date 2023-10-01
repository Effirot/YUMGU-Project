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

    public class QuestSystem : MonoBehaviour, IEnumerable<Quest>
    {
        public static QuestSystem Current;
        
        public static string StartedQuest { 
            get => SelectedQuestName;
            set {
                SelectedQuestName = value;
            }
        }
        static string SelectedQuestName = "";

        [SerializeField] public string Name;
        [TextArea(3, 20)]
        [SerializeField] public string Description;

        [SerializeField] private int _CurrentQuestIndex = 0;
        [SerializeField] public int NextQuestIndex = 1;

#if UNITY_EDITOR
        [SerializeField] bool PermanentlyStarted = false;
#endif

        public List<Quest> questList = new List<Quest>();
            
        public UnityEvent OnQuestComplete = new(); 
        public UnityEvent<Quest> OnQuestSelected = new(); 
        
        
        public Quest CurrentQuest => questList[_CurrentQuestIndex];
        
        public int CurrentQuestIndex {
            get => _CurrentQuestIndex;
            set {
                Debug.Log($"Set quest to {NextQuestIndex}");

                _CurrentQuestIndex = value;
                CurrentQuest?.OnQuestStart.Invoke();
                CheckIncludes();

                if(Application.isPlaying)
                    OnQuestSelected.Invoke(CurrentQuest);
            }
        }

        public virtual void NextQuest()
        {            
            CurrentQuestIndex = NextQuestIndex;
            NextQuestIndex = SetDefaultNextQuestIndex();
            
            CheckIncludes();
        }

        public virtual int SetDefaultNextQuestIndex(){
            return CurrentQuestIndex + 1;
        }
        
        protected virtual void CheckIncludes(){

            int index = 0;
            while(index < questList.Count){
                questList[index]?.gameObject?.SetActive(_CurrentQuestIndex == index);
                
                if(questList[index] != null) 
                    index++;
                else questList.RemoveAt(index);
            }
        }


        protected virtual void Start() 
        {
            bool isStarted;
#if UNITY_EDITOR
            isStarted = Name == StartedQuest || PermanentlyStarted;
#else
            isStarted = Name == StartedQuest;
#endif
            
            gameObject.SetActive(isStarted);
            if(isStarted){
                Current = this;

                CurrentQuestIndex = 0;

                OnQuestSelected.Invoke(CurrentQuest);
            }
        }
        private void OnValidate() => CheckIncludes();


        public IEnumerator<Quest> GetEnumerator() => questList.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => questList.GetEnumerator();
    
#if UNITY_EDITOR
        [CustomEditor(typeof(QuestSystem), true)]
        public class QuestSystem_Editor : Editor{
            new QuestSystem target => (QuestSystem)base.target; 

            public override void OnInspectorGUI()
            {
                GUILayout.BeginHorizontal();
                if(GUILayout.Button("New Quest"))
                    CreateQuest();
                if(GUILayout.Button("Clear Quests"))
                    ClearQuest();
                GUILayout.EndHorizontal();

                base.OnInspectorGUI();
            }

                
            public void CreateQuest() {
                GameObject obj = new GameObject("New Quest");
                obj.transform.parent = target.transform;
                obj.transform.SetAsLastSibling();
                Quest quest = obj.AddComponent<Quest>();
                target.questList.Add(quest);

                #if UNITY_EDITOR
                    Selection.objects = new UnityEngine.Object[] { obj };
                #endif

                target.CheckIncludes();
            }

            public void ClearQuest() {
                while(target.questList.Any()){
                    
                    if(target.questList[0]?.gameObject != null)
                    if(Application.isPlaying)
                        GameObject.Destroy(target.questList[0]?.gameObject);
                    else 
                        DestroyImmediate(target.questList[0]?.gameObject);

                    target.questList.RemoveAt(0);
                    target.CheckIncludes();
                }
            }
            
        }
#endif
    }
}