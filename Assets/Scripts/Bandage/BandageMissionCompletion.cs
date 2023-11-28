using System.Collections;
using System.Collections.Generic;
using QuestManagement;
using UnityEngine;

public class BandageMissionCompletion : MonoBehaviour
{
    [SerializeField]
    private Bandage bandage;

    [SerializeField]
    private Mission mission;

    private float RequireDistance = 1;

    private void FixedUpdate()
    {
        if (bandage.totalDistance > RequireDistance)
        {
            mission.ForcedComplete();
            enabled = false;
        }
    }
}
