using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "ActorSO", menuName = "Dialogue/NPC")]
public class ActorSO : ScriptableObject
{
    public string actorName;
    public Sprite portrait;

    public float typingSpeed = 0.05f; // Speed at which text is displayed
    public AudioClip voiceSound; // Sound played when the actor speaks
    [Header("Voice Settings")]
    [Range(0.1f, 3f)] public float basePitch = 1.0f;        // Bazowy pitch
    [Range(0f, 1f)] public float pitchVariation = 0.1f;     // Zakres losowej zmiany pitcha


    [Header("Expressions")]
    public List<Expression> expressions;

    public Sprite GetExpressionPortrait(string expressionName)
    {
        foreach (var expr in expressions)
        {
            if (expr.expressionName == expressionName)
                return expr.portrait;
        }

        return portrait; // jeśli brak, zwróć domyślny
    }
}

[System.Serializable]
public class Expression
{
    public string expressionName;
    public Sprite portrait;
}