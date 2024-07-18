using System.Collections.Generic;
using UnityEngine;

public class CardDatabase : MonoBehaviour
{
    public static CardDatabase Instance { get; private set;}

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

    [SerializeField] private List<CardSO> drawDeck = new List<CardSO>();
    [SerializeField] private List<CardSO> discardDeck = new List<CardSO>();

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("More than one instance");
        }
        Instance = this;

        InitializeDrawDeck();
        //ShuffleDeck();
    }

    private void InitializeDrawDeck()
    {
        for (int i = 0; i < totalCardsPerType.Length; i++)
        {
            for (int j = 0; j < totalCardsPerType[i]; j++)
            {
                drawDeck.Add(cardsSO[i]);
            }
        }
    }

    private void ShuffleDeck()
    {
        for(int i = 0; i < drawDeck.Count; i++)
        {
            int randomCardIdx = Random.Range(i, drawDeck.Count);
            CardSO randomCard = drawDeck[randomCardIdx];

            drawDeck[randomCardIdx] = drawDeck[i];

            drawDeck[i] = randomCard;
        }
    }

    public CardSO DrawCard()
    {
        CardSO card = drawDeck[0];
        drawDeck.RemoveAt(0);

        DrawDeckUI.Instance.UpdateCardsInDrawDeckText(drawDeck.Count);

        return card;
    }
}