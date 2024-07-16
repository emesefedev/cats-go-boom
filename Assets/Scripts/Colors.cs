using UnityEngine;

public static class Colors
{
    /// <summary>
    /// This array needs to have as many elements as the CardType enum
    /// </summary>
    public readonly static Color[] cardColors = new Color[]{
        new Color(0.894f, 0.322f, 0.035f),  // Attack
        new Color(0, 0, 0),                 // Boom
        new Color(0.494f, 0.569f, 0.027f),  // Defuse
        new Color(0.592f, 0.549f, 0.514f),  // Favor
        new Color(0.592f, 0, 0.005f),       // Nope
        new Color(0.808f, 0.078f, 0.337f),  // SeeFuture
        new Color(0.631f, 0.361f, 0.153f),  // Shuffle
        new Color(0.165f, 0.306f, 0.678f)   // Skip
    };
}