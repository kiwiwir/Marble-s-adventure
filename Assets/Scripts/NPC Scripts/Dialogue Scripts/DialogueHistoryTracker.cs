using System.Collections.Generic;
using UnityEngine;

public class DialogueHistoryTracker : MonoBehaviour
{
    private readonly HashSet<ActorSO> spokenNPCs = new HashSet<ActorSO>();

    public void RecordNPC(ActorSO actorSO)
    {
        spokenNPCs.Add(actorSO);

        Debug.Log($"Just spoke to {actorSO.nameENG}.");
    }

    public bool HasSpokenWith(ActorSO actorSO)
    {
        return spokenNPCs.Contains(actorSO);
    }
}
