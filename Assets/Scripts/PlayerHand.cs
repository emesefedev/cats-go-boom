using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandCard 
{
    private GameObject cardInstance;
    private CardSO cardSO;

    private CardUI cardUI;
    private CardLogic cardLogic;

    public HandCard(GameObject cardInstance, CardSO cardSO)
    {
        this.cardInstance = cardInstance;
        this.cardSO = cardSO;

        this.cardUI = cardInstance.GetComponent<CardUI>();
        this.cardLogic = cardInstance.GetComponent<CardLogic>();

        // Call set up card UI
        // Call set up card Logic
    }

    // Declare Set Up Card UI
    // Declare Set Up Card Logic
}

public class PlayerHand : MonoBehaviour
{
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private bool playable;

    [SerializeField] private List<CardSO> hand = new List<CardSO>();
    [SerializeField] private List<HandCard> newHand = new List<HandCard>();
    

    private void AddHandCardToPlayerHand(HandCard handCard)
    {
        newHand.Add(handCard);
    }

    private GameObject InstantiateNewCardInPlayerHandPanel()
    {
        return Instantiate(cardPrefab.transform, transform).gameObject;
    }





    private void AddCardToPlayerHandPanel(CardSO cardSO)
    {
        if (cardSO != null)
        {
            AddCardToPlayerHand(cardSO); // Logic
            UpdatePlayerHandPanelWithNewCard(cardSO); // Visual
        }
        
    }

    private void RemoveCardFromPlayerHandPanel(GameObject card, int childIndex)
    {
        RemoveCardFromPlayerHandAtIndex(childIndex); // Logic
        DiscardDeckUI.Instance.AddCardToDiscardDeck(card); // Visual
    }

    private void AddCardToPlayerHand(CardSO cardSO) 
    {
        hand.Add(cardSO);
    }

    private void RemoveCardFromPlayerHandAtIndex(int index)
    {
        hand.RemoveAt(index);
    }

    private void PlayerDrawDefuseCard()
    {
        CardSO cardSO = CardDatabase.Instance.DrawDefuseCard();
        AddCardToPlayerHandPanel(cardSO);
    }

    private Transform InstantiateNewCardTransformInPlayerHandPanel()
    {
        return Instantiate(cardPrefab.transform, transform);
    }

    private void SetUpCardInPlayerHandPanel(GameObject cardInstance, CardSO cardSO)
    {
        // Card's Logic
        SetUpCardLogic(cardInstance, cardSO);

        // Card's Visual
        SetUpCardVisual(cardInstance, cardSO);
    }

    private void SetUpCardLogic(GameObject cardInstance, CardSO cardSO)
    {
        if (!cardInstance.TryGetComponent<CardLogic>(out CardLogic cardLogic))
        {
            cardLogic = cardInstance.AddComponent<CardLogic>();
        }

        cardLogic.SetCardType(cardSO.cardType, cardSO.cardSubType);
        cardLogic.SetCardPlayer(this);
    }

    private void SetUpCardVisual(GameObject cardInstance, CardSO cardSO)
    {
        CardUI cardUI = cardInstance.GetComponent<CardUI>();
        cardUI.SetCard(cardSO, !playable);
    }

    private void UpdatePlayerHandPanelWithNewCard(CardSO cardSO)
    {
        Transform cardInstance = InstantiateNewCardTransformInPlayerHandPanel();
        SetUpCardInPlayerHandPanel(cardInstance.gameObject, cardSO);
    }  

    public void UpdatePlayerHandPanel()
    {
        if (transform.childCount != hand.Count)
        {
            Debug.LogError("Card instances do not match the cards in the player's hand");
        }

        for (int i = 0; i < hand.Count; i++)
        {
            Transform cardInstance = transform.GetChild(i);
            CardSO cardSO = hand[i];

            SetUpCardInPlayerHandPanel(cardInstance.gameObject, cardSO);
        }
    }  

    public bool PlayerPlayCard(GameObject card, int childIndex)
    {
        CardLogic cardLogic = card.GetComponent<CardLogic>();
        bool cardCanBePlayed = cardLogic.PlayCard();
        
        if (cardCanBePlayed)
        {
            RemoveCardFromPlayerHandPanel(card, childIndex);

            CardUI cardUI = card.GetComponent<CardUI>();
            CardType cardType = cardUI.GetCardType();

            if (cardType == CardType.Cat)
            {
                RemoveFirstCopyStartingAtTheEndOfTheHand(cardType, cardUI.GetCardSubType());
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
            
            PlayerPlayCard(cardInstance, randomCardIndex);
        }  

        PlayerDrawCard();
        GameManager.Instance.ChangeTurn();
    }

    public void PlayerDrawCard()
    {
        CardSO cardSO = CardDatabase.Instance.DrawCardSOFromDrawDeck();
        CardType cardType = cardSO.cardType;

        if (cardType == CardType.Boom)
        {
            GameManager.Instance.GameOver();
        }


        AddCardToPlayerHandPanel(cardSO);
    }

    public void InitializePlayerHand()
    {
        for(int i = 0; i < 7; i++)
        {
            CardSO cardSO = CardDatabase.Instance.DrawCardSOFromDrawDeck();
            GameObject cardInstance = InstantiateNewCardInPlayerHandPanel();

            HandCard handCard = new HandCard(cardInstance, cardSO);
            AddHandCardToPlayerHand(handCard);
        }

        PlayerDrawDefuseCard();
    } 

    public bool PlayerHasTwoEqualCardsInHand(CardType cardType, CardSubType cardSubType)
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
                RemoveCardFromPlayerHandPanel(transform.GetChild(i).gameObject, i);
                return;
            }
        }

        Debug.LogError($"There's no copy of {cardType}+{cardSubType} to remove");
    }

    public bool PlayerIsPlayable()
    {
        return playable;
    }
}
