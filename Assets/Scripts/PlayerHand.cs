using System.Collections.Generic;
using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    [SerializeField] private List<GameObject> hand = new List<GameObject>();
    [SerializeField] bool playable;

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

    private void AddCardToPlayerHand(GameObject card) 
    {
        if (card != null)
        {
            hand.Add(card);
            UpdatePlayerDeckUIWithNewCard(card);
        }
    }

    public void PlayerDrawCard()
    {
        GameObject card = CardDatabase.Instance.DrawCard();
        CardType cardType = card.GetComponent<CardUI>().GetCardType();

        if (cardType == CardType.Boom)
        {
            Debug.Log($"{gameObject.name} GAME OVER");
        }


        AddCardToPlayerHand(card);
    }

    private void PlayerDrawDefuseCard()
    {
        GameObject card = CardDatabase.Instance.DrawDefuseCard();
        AddCardToPlayerHand(card);
    }

    private void UpdatePlayerDeckUIWithNewCard(GameObject card)
    {
        Transform cardInstance = Instantiate(card.transform, transform);
        
        CardUI cardUI = cardInstance.GetComponent<CardUI>();
        cardUI.SetCard();
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

            GameObject card = hand[i];
            CardType cardType = card.GetComponent<CardUI>().GetCardType();

            cardUI.SetCard();
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
        GameObject card = null;
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

            card = hand[randomCardIndex];
            cardType = card.GetComponent<CardUI>().GetCardType();
        }
        while (cardType == CardType.Defuse || 
               cardType == CardType.Nope || 
               cardType == CardType.Boom);

        if (playCard)
        {
            Debug.Log($"Plays card {cardType}");
            hand.Remove(card);

            GameObject cardInstance = transform.GetChild(randomCardIndex).gameObject;
            CardUI cardUI = cardInstance.GetComponent<CardUI>();
            cardUI.FaceDownCard(false);
            
            DiscardDeckUI.Instance.AddCardToDiscardDeck(cardInstance);
        }   

        PlayerDrawCard();
        GameManager.Instance.ChangeTurn();
    }
}
