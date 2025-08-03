using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Localization.Settings;

[CreateAssetMenu(fileName = "ActorSO", menuName = "Dialogue/NPC")]
public class ActorSO : ScriptableObject
{
    //public string actorName;
    [Header("Localized Names")]
    public string nameENG;
    public string namePL;

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

    public string GetLocalizedName()
    {
        var locale = LocalizationSettings.SelectedLocale;
        if (locale == null)
        {
            Debug.LogWarning("No locale selected, falling back to English.");
            return nameENG;
        }

        var code = locale.Identifier.Code.ToLower();

        switch (code)
        {
            case "pl":
                return namePL;
            case "en":
            default:
                return nameENG;
        }
    }
}

[System.Serializable]
public class Expression
{
    public string expressionName;
    public Sprite portrait;
}