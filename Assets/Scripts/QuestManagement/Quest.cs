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

    public class Quest : MonoBehaviour, IEnumerable<Mission>
    {
        
#if UNITY_EDITOR
        [CustomEditor(typeof(Quest), true)]
        public class QuestSystem_Editor : Editor{
            new Quest target => (Quest)base.target; 

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
                Mission quest = obj.AddComponent<Mission>();
                target.Missions.Add(quest);

                #if UNITY_EDITOR
                    Selection.objects = new UnityEngine.Object[] { obj };
                #endif

                target.CheckContents();
            }

            public void ClearQuest() {
                while(target.Missions.Any()){
                    
                    if(target.Missions[0]?.gameObject != null)
                    if(Application.isPlaying)
                        GameObject.Destroy(target.Missions[0]?.gameObject);
                    else 
                        DestroyImmediate(target.Missions[0]?.gameObject);

                    target.Missions.RemoveAt(0);
                    target.CheckContents();
                }
            }
            
        }
#endif

        public delegate void QuestSelectedAction(Quest quest);
        public delegate void MissionSelectedAction(Mission quest);


        public static Quest Singleton;

        public static event QuestSelectedAction OnQuestSelected; 

        public static string StartedQuest { 
            get => SelectedQuestName;
            set {
                SelectedQuestName = value;
            }
        }

        private static string SelectedQuestName = "";
        private static List<Quest> allQuestSingletons = new(); 



#if UNITY_EDITOR
        [SerializeField] bool SelectQuestOnStart = false;
#endif

        [Space]
        [SerializeField] 
        public string Name;
        [SerializeField, TextArea(3, 20)] 
        public string Description;
        
        [Space]
        [SerializeField] 
        private int _CurrentMissionIndex = 0;
        [NonSerialized] 
        public int NextMissionIndex = 1;
        
        [Space]    
        [SerializeField] 
        public UnityEvent OnMissionComplete = new(); 
        [SerializeField] 
        public UnityEvent<Mission> OnMissionSelected = new(); 
        
        [Space]
        [SerializeField] 
        public List<Mission> Missions = new List<Mission>();
        
        public Mission SelectedMission => 
                            Missions[_CurrentMissionIndex];

        public int CurrentQuestIndex {
            get => _CurrentMissionIndex;
            set {
                _CurrentMissionIndex = value;

                SelectMissionByIndex(value);
            }
        }

        public virtual void NextMission()
        {            
            CurrentQuestIndex = NextMissionIndex;
            NextMissionIndex = GetNextQuestIndex();
            
            CheckContents();
        }
        public virtual int GetNextQuestIndex()
        {
            return CurrentQuestIndex + 1;
        }
                        
        protected virtual void CheckContents(){
            int index = 0;
            while(index < Missions.Count){

                if(Missions[index] != null)
                {
                    Missions[index]?.gameObject?.SetActive(_CurrentMissionIndex == index);
                    Missions[index].Quest = this;
                    index++;
                }
                else 
                {
                    Missions.RemoveAt(index);
                }
            }
        }

        protected virtual void Awake() 
        {
            allQuestSingletons.Add(this);

            bool isStarted;
#if UNITY_EDITOR
            isStarted = Name == StartedQuest || SelectQuestOnStart;
#else
            isStarted = Name == StartedQuest;
#endif
            
            gameObject.SetActive(isStarted);
            if(isStarted){
                SeletQuestAsSingleton();
                SelectMissionByIndex(_CurrentMissionIndex);
            }
        }
        protected virtual void OnValidate() 
        {
            CheckContents();
        }
        protected virtual void OnDestroy()
        {
            allQuestSingletons.Remove(this);
        }


        private void SelectMissionByIndex(int Index)
        {
            Debug.Log($"Set quest to {Index}");

            // foreach(var condition in SelectedMission?.conditionsList)
            // {
            //     condition.OnConditionChanged.RemoveListener(ConditionChangedEventHandler);
            // }

            _CurrentMissionIndex = Index;

            // foreach(var condition in SelectedMission?.conditionsList)
            // {
            //     condition.OnConditionChanged.AddListener(ConditionChangedEventHandler);
            // }

            SelectedMission.CheckIncludes();
            SelectedMission.OnMissionStart.Invoke();
            CheckContents();

            if(Application.isPlaying)
            {
                if(SelectedMission != null)
                {
                    SelectedMission.Quest = this;
                    OnMissionSelected.Invoke(SelectedMission);
                }
            }
        }

        // private void ConditionChangedEventHandler(ToggleQuestCondition condition, bool value)
        // {

        // }

        private void SeletQuestAsSingleton()
        {
            if(Singleton != null) return;

            Singleton = this;

            CurrentQuestIndex = 0;

            OnQuestSelected?.Invoke(this);
            OnMissionSelected.Invoke(SelectedMission);
        }


        IEnumerator<Mission> IEnumerable<Mission>.GetEnumerator() => Missions.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => Missions.GetEnumerator();
    
    }
}