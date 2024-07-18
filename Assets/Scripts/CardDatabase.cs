using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class CardDatabase : MonoBehaviour
{
    public static CardDatabase Instance { get; private set;}

    [SerializeField] private CardSO[] cardsSO;

    /// <summary>
    /// This array needs to have as many elements as the CardSO array
    /// </summary>
    private int[] totalCardsPerType = new int[]{
        4,   // Attack
        1,   // Boom
        4,   // Cat (AirCat)
        4,   // Cat (EarthCat)
        4,   // Cat (EtherCat)
        4,   // Cat (FireCat)
        4,   // Cat (WaterCat)
        6,   // Defuse
        4,   // Favor
        5,   // Nope
        5,   // SeeFuture
        4,   // Shuffle
        4    // Skip
    };
    private int totalDefuseCardsDrawn;


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
        ShuffleDeck();
    }

    private void InitializeDrawDeck()
    {
        for (int i = 0; i < cardsSO.Length; i++)
        {
            if (cardsSO[i].cardType == CardType.Boom || cardsSO[i].cardType == CardType.Defuse) continue;

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

    public CardSO DrawDefuseCard()
    {
        // TODO: Get CardSO with type Defuse
        totalDefuseCardsDrawn ++;
        return cardsSO[7];
    }

    public void CompleteDrawDeck()
    {
        // TODO: Get CardSO with type Boom
        drawDeck.Add(cardsSO[2]); // Boom

        // TODO: Get CardSO with type Defuse
        int totalDefuseCardsLeft = totalCardsPerType[7] - totalDefuseCardsDrawn;
        for (int j = 0; j < totalDefuseCardsLeft; j++)
        {
            drawDeck.Add(cardsSO[7]);
        }

        DrawDeckUI.Instance.UpdateCardsInDrawDeckText(drawDeck.Count);
        ShuffleDeck();
    }
}