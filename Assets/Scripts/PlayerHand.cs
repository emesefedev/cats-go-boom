using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private bool playable;

    [SerializeField] private List<CardSO> hand = new List<CardSO>();
    
    private void AddCardToPlayerHand(CardSO card) 
    {
        if (card != null)
        {
            hand.Add(card);
            UpdatePlayerDeckUIWithNewCard(card);
        }
    }

    private void PlayerDrawDefuseCard()
    {
        CardSO card = CardDatabase.Instance.DrawDefuseCard();
        AddCardToPlayerHand(card);
    }

    private void UpdatePlayerDeckUIWithNewCard(CardSO cardSO)
    {
        Transform cardInstance = Instantiate(cardPrefab.transform, transform); 
        CardUI cardUI = cardInstance.GetComponent<CardUI>();

        CardLogic cardLogic = cardInstance.AddComponent<CardLogic>();
        cardLogic.SetCardType(cardSO.cardType, cardSO.cardSubType);
        cardLogic.SetCardPlayer(this);

        cardUI.SetCard(cardSO);
        cardUI.FaceDownCard(!playable);
    }  

    private void RemoveCardFromHandAtIndex(int index)
    {
        hand.RemoveAt(index);
    }

    private void RemoveCardFromHandPanel(GameObject card, int childIndex)
    {
        DiscardDeckUI.Instance.AddCardToDiscardDeck(card); // Visual
        RemoveCardFromHandAtIndex(childIndex); // Logic
    }

    public void UpdatePlayerDeckUI()
    {
        if (transform.childCount != hand.Count)
        {
            Debug.LogError("Card instances do not match the cards in the player's hand");
        }

        for (int i = 0; i < hand.Count; i++)
        {
            Transform cardInstance = transform.GetChild(i);
            CardUI cardUI = cardInstance.GetComponent<CardUI>();

            CardSO cardSO = hand[i];

            cardUI.SetCard(cardSO);
        }
    }  

    public bool PlayerPlayCard(GameObject card, int childIndex)
    {
        CardLogic cardLogic = card.GetComponent<CardLogic>();
        bool cardCanBePlayed = cardLogic.PlayCard();
        
        if (cardCanBePlayed)
        {
            RemoveCardFromHandPanel(card, childIndex);
        }

        return cardCanBePlayed;
    }

    public IEnumerator PlayTurnAutomatically()
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
            CardUI cardUI = cardInstance.GetComponent<CardUI>();
            
            bool cardCanBePlayed = PlayerPlayCard(cardInstance, randomCardIndex);

            cardUI.FaceDownCard(!cardCanBePlayed);
        }  

        PlayerDrawCard();
        GameManager.Instance.ChangeTurn();
    }

    public void PlayerDrawCard()
    {
        CardSO cardSO = CardDatabase.Instance.DrawCard();
        CardType cardType = cardSO.cardType;

        if (cardType == CardType.Boom)
        {
            GameManager.Instance.GameOver();
        }


        AddCardToPlayerHand(cardSO);
    }

    public void InitializePlayerDeck()
    {
        for(int i = 0; i < 7; i++)
        {
            PlayerDrawCard();
        }

        PlayerDrawDefuseCard();
    } 

    public bool PlayerHasSameCardInHand(CardType cardType, CardSubType cardSubType)
    {
        int totalCopies = 0;

        foreach (CardSO cardSO in hand)
        {
            if (cardSO.cardType == cardType && cardSO.cardSubType == cardSubType)
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

    public void RemoveFirstCopyStartingAtTheEndOfTheHand(CardType cardType, CardSubType cardSubType)
    {
        for (int i = hand.Count - 1; i >= 0; i--)
        {
            CardSO handCard = hand[i];
            if (handCard.cardType == cardType && handCard.cardSubType == cardSubType)
            {
                RemoveCardFromHandPanel(transform.GetChild(i).gameObject, i);
                return;
            }
        }

        Debug.LogError($"There's no copy of {cardType}+{cardSubType} to remove");
    }

    public bool IsPlayable()
    {
        return playable;
    }
}
