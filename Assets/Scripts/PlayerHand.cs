using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private bool playable;
    [SerializeField] private List<CardSO> hand = new List<CardSO>();
    

    public bool IsPlayable()
    {
        return playable;
    }
    
    public void InitializePlayerDeck()
    {
        for(int i = 0; i < 7; i++)
        {
            PlayerDrawCard();
        }

        PlayerDrawDefuseCard();
    } 

    private void AddCardToPlayerHand(CardSO card) 
    {
        if (card != null)
        {
            hand.Add(card);
            UpdatePlayerDeckUIWithNewCard(card);
        }
    }

    public void PlayerDrawCard()
    {
        CardSO cardSO = CardDatabase.Instance.DrawCard();
        CardType cardType = cardSO.cardType;

        if (cardType == CardType.Boom)
        {
            Debug.Log($"{gameObject.name} GAME OVER");
        }


        AddCardToPlayerHand(cardSO);
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

        cardUI.SetCard(cardSO);
        cardUI.FaceDownCard(!playable);
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

    public void PlayerPlayCard(GameObject card, int childIndex)
    {
        CardLogic cardLogic = card.GetComponent<CardLogic>();
        cardLogic.PlayCard();
        
        hand.RemoveAt(childIndex);
    }

    public void PlayTurnAutomatically()
    {
        // We declare the needed variables
        bool playCard = true;
        int randomCardIndex;
        CardType cardType = CardType.Boom;

        // We can play a card or not. Finally draw and change turn
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

        if (playCard)
        {
            Debug.Log($"Plays card {cardType}");
            hand.RemoveAt(randomCardIndex);

            GameObject cardInstance = transform.GetChild(randomCardIndex).gameObject;
            CardUI cardUI = cardInstance.GetComponent<CardUI>();
            cardUI.FaceDownCard(false);
            
            DiscardDeckUI.Instance.AddCardToDiscardDeck(cardInstance);
        }   

        PlayerDrawCard();
        GameManager.Instance.ChangeTurn();
    }
}
