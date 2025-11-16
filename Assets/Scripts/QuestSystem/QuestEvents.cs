using UnityEngine;
using System;

public static class QuestEvents
{
    public static Action<QuestSO> OnQuestOfferRequested;
    public static Action<QuestSO> OnQuestTurnInRequested;
    public static Func<QuestSO, bool> IsQuestComplete;
}

