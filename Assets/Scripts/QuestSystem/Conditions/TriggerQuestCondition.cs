


using UnityEngine;

namespace QuestManager{
    public class TriggerQuestCondition : QuestCondition {
        public new bool IsCompleted {
            get => base.IsCompleted;
            set {
                if(gameObject.activeInHierarchy)
                    base.IsCompleted = value;
            }
        }

        public void Trig()
        {
            if(quest.questSystem.CurrentQuest != quest) return;
            if(!gameObject.activeInHierarchy) return;

            IsCompleted = true;
            IsCompleted = false;
        }
    }
}