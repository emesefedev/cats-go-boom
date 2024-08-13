using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private bool playable;

    [SerializeField] private List<CardSO> hand = new List<CardSO>();
    [SerializeField] private List<HandCard> newHand = new List<HandCard>();
    
    public bool PlayerIsPlayable()
    {
        return playable;
    }

    private void AddHandCardToPlayerHand(HandCard handCard)
    {
        newHand.Add(handCard);
    }

    private void DiscardCardFromPlayerHandAtIndex(int index)
    {
        HandCard removedCard = newHand[index];
        newHand.RemoveAt(index);

        DiscardDeckUI.Instance.AddCardToDiscardDeck(removedCard); // Visual
    }

    private GameObject InstantiateNewCardInPlayerHandPanel()
    {
        return Instantiate(cardPrefab.transform, transform).gameObject;
    }

    private void PlayerDrawDefuseCard()
    {
        CardSO cardSO = CardDatabase.Instance.DrawDefuseCard();
        GameObject cardInstance = InstantiateNewCardInPlayerHandPanel();

        HandCard handCard = new HandCard(cardInstance, cardSO, this);
        AddHandCardToPlayerHand(handCard);
    } 

    public void PlayerDrawCard()
    {
        CardSO cardSO = CardDatabase.Instance.DrawCardSOFromDrawDeck();
        GameObject cardInstance = InstantiateNewCardInPlayerHandPanel();

        HandCard handCard = new HandCard(cardInstance, cardSO, this);
        AddHandCardToPlayerHand(handCard);

        if (cardSO.cardType == CardType.Boom)
        {
            GameManager.Instance.GameOver();
        }
    }

    public void InitializePlayerHand()
    {
        for(int i = 0; i < 7; i++)
        {
            PlayerDrawCard();
        }

        PlayerDrawDefuseCard();
    }

    public bool PlayerHasTwoEqualCardsInHand(CardType cardType, CardSubType cardSubType)
    {
        int totalCopies = 0;

        foreach (HandCard card in newHand)
        {
            if (card.GetCardType() == cardType && card.GetCardSubType() == cardSubType)
            {
                totalCopies++;
                Debug.Log($"Total copies in hand of {cardType}+{cardSubType} = {totalCopies}");
                
                if (totalCopies >= 2)
                {
                    return true;
                }
            }
        }

        return false;   
    }

    public void RemoveCardFirstCopyStartingAtTheEndOfPlayerHand(CardType cardType, CardSubType cardSubType)
    {
        for (int i = hand.Count - 1; i >= 0; i--)
        {
            HandCard handCard = newHand[i];
            if (handCard.GetCardType() == cardType && handCard.GetCardSubType() == cardSubType)
            {
                DiscardCardFromPlayerHandAtIndex(i);
                return;
            }
        }

        Debug.LogError($"There's no copy of {cardType}+{cardSubType} to remove");
    }  

    public bool PlayerPlayCard(int childIndex)
    {
        HandCard playedCard = newHand[childIndex];
        
        bool cardCanBePlayed = playedCard.PlayHandCard();
        
        if (cardCanBePlayed)
        {
            DiscardCardFromPlayerHandAtIndex(childIndex);

            CardType cardType = playedCard.GetCardType();

            if (cardType == CardType.Cat)
            {
                CardSubType cardSubType = playedCard.GetCardSubType();
                RemoveCardFirstCopyStartingAtTheEndOfPlayerHand(cardType, cardSubType);
            }
        }

        return cardCanBePlayed;
    }

    public IEnumerator PlayerPlayTurnAutomatically()
    {
        // We declare the needed variables
        bool playCard = true;
        int randomCardIndex;
        CardType cardType;

        do 
        {
            randomCardIndex =  Random.Range(-1, hand.Count);

            if (randomCardIndex == -1)
            {
                Debug.Log("Doesn't play any card");
                playCard = false;
                break;
            }

            cardType = hand[randomCardIndex].cardType;
        }
        while (cardType == CardType.Defuse || 
               cardType == CardType.Nope || 
               cardType == CardType.Boom);

        yield return new WaitForSeconds(1);

        if (playCard)
        {
            Debug.Log($"Plays a card");

            GameObject cardInstance = transform.GetChild(randomCardIndex).gameObject;    
            
            PlayerPlayCard(randomCardIndex);
        }  

        PlayerDrawCard();
        GameManager.Instance.ChangeTurn();
    } 

    public void ReorderPlayerHand(int playedCardIndex)
    {
        HandCard playedCard = newHand[playedCardIndex];
        int totalPlayerHandCards = newHand.Count;
        for (int i = playedCardIndex; i < totalPlayerHandCards - 1; i++)
        {
            newHand[i] = newHand[i + 1];
        }

        newHand[totalPlayerHandCards - 1] = playedCard;
    }
}
