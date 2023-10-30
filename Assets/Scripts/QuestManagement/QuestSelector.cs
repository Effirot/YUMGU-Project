using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using QuestManagement;

[CreateAssetMenu(fileName = "QuestSelector", menuName = "Quest System/QuestSelector", order = 0)]
public class QuestSelector : ScriptableObject
{
    public string StartedQuest
    {
        get => Quest.StartedQuest;
        set => Quest.StartedQuest = value;
    }
}
