using System.Collections.Generic;
using UnityEngine;

public class CardDatabase : MonoBehaviour
{
    [SerializeField] private CardSO[] cardsSO;

    /// <summary>
    /// This array needs to have as many elements as the CardType enum
    /// </summary>
    private int[] totalCardsPerType = new int[]{
        4,   // Attack
        1,   // Boom
        20,  // Cat
        6,   // Defuse
        4,   // Favor
        5,   // Nope
        5,   // SeeFuture
        4,   // Shuffle
        4    // Skip
    };

    public List<CardSO> deck;

    private void Awake()
    {
        for (int i = 0; i < totalCardsPerType.Length; i++)
        {
            for (int j = 0; j < totalCardsPerType[i]; j++)
            {
                deck.Add(cardsSO[i]);
            }
        }
    }
}