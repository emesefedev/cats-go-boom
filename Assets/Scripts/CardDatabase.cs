using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;
using System.Linq;

public class CardDatabase : MonoBehaviour
{
    public static CardDatabase Instance { get; private set;}
    
    [SerializeField] private CardSO[] cardsSO;

    /// <summary>
    /// This array needs to have as many elements as the cardsSO array
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
    [SerializeField] private List<HandCard> discardDeck = new List<HandCard>();

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("More than one instance");
        }
        Instance = this;

        InitializeDrawDeck();
        ShuffleDrawDeck();
    }

    private int GetCardIndexGivenType(CardType desiredCardType)
    {
        for (int i = 0; i < cardsSO.Length; i++)
        {
            CardType cardType = cardsSO[i].cardType;
            if (cardType == desiredCardType)
            {
                return i;
            }
        }

        Debug.LogError("The card type passed by parameter does not exist");
        return -1;
    }

    public CardSO DrawDefuseCard()
    {
        int defuseCardIndex = GetCardIndexGivenType(CardType.Defuse);
        totalDefuseCardsDrawn ++;

        return cardsSO[defuseCardIndex];
    }

    private void InitializeDrawDeck()
    {
        for (int i = 0; i < cardsSO.Length; i++)
        {
            CardType cardType = cardsSO[i].cardType;
            if (cardType == CardType.Boom || cardType == CardType.Defuse) 
            {
                continue;
            }

            for (int j = 0; j < totalCardsPerType[i]; j++)
            {
                drawDeck.Add(cardsSO[i]);
            }
        }
    }

    public void CompleteDrawDeck()
    {
        int boomCardIndex = GetCardIndexGivenType(CardType.Boom);
        drawDeck.Add(cardsSO[boomCardIndex]); 

        int defuseCardIndex = GetCardIndexGivenType(CardType.Defuse);
        int totalDefuseCardsLeft = totalCardsPerType[defuseCardIndex] - totalDefuseCardsDrawn;
        for (int j = 0; j < totalDefuseCardsLeft; j++)
        {
            drawDeck.Add(cardsSO[defuseCardIndex]);
        }

        DrawDeckUI.Instance.UpdateCardsInDrawDeckText(drawDeck.Count);
        ShuffleDrawDeck();
    }

    public void ShuffleDrawDeck()
    {
        for(int i = 0; i < drawDeck.Count; i++)
        {
            int randomCardIdx = Random.Range(i, drawDeck.Count);
            CardSO randomCardSO = drawDeck[randomCardIdx];

            drawDeck[randomCardIdx] = drawDeck[i];

            drawDeck[i] = randomCardSO;
        }
    }

    public CardSO DrawCardSOFromDrawDeck()
    {
        if (drawDeck.Count <= 0)
        {
            Debug.LogError("There are no more cards to draw");
        }

        CardSO card = drawDeck[0];
        drawDeck.RemoveAt(0);

        DrawDeckUI.Instance.UpdateCardsInDrawDeckText(drawDeck.Count);

        return card;
    }

    public List<CardSO> GetFirstThreeCardsFromDrawDeck()
    {
        return drawDeck.GetRange(0, 3);
    }

    public void AddCardToDiscardDeck(HandCard handCard)
    {
        discardDeck.Add(handCard);
    }
}