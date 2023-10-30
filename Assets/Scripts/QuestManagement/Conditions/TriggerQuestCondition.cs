


using UnityEngine;

namespace QuestManagement{
    public class TriggerQuestCondition : ToggleQuestCondition {
        public new bool IsCompleted {
            get => base.IsCompleted;
            set {
                if(gameObject.activeInHierarchy)
                    base.IsCompleted = value;
            }
        }

        public void Trig()
        {
            if(mission.Quest.SelectedMission != mission) return;
            if(!gameObject.activeInHierarchy) return;

            IsCompleted = true;
            IsCompleted = false;
        }
    }
}