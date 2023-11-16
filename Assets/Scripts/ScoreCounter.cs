using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScoreCounter : MonoBehaviour
{
    [SerializeField]
    private int _Score = 0;
    
    public int Score 
    {
        get => _Score;
        set 
        {
            OnScoreChanged.Invoke(value);
            
            _Score = value;
        }
    }

    public UnityEvent<int> OnScoreChanged = new(); 

    public void Add(int count)
    {
        Score += count;
    }

    public void Subtract(int count)
    {
        Score -= count;
    }
}
