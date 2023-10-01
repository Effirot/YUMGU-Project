using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using QuestManager;


[CreateAssetMenu(fileName = "QuestSelector", menuName = "Quest System/QuestSelector", order = 0)]
public class QuestSelector : ScriptableObject
{
    public string StartedQuest
    {
        get => QuestSystem.StartedQuest;
        set => QuestSystem.StartedQuest = value;
    }
}
