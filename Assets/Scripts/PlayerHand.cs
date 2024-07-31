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
            UpdatePlayerDeckUI(card);
        }
    }

    public void PlayerDrawCard()
    {
        GameObject card = CardDatabase.Instance.DrawCard();
        AddCardToPlayerHand(card);
    }

    private void PlayerDrawDefuseCard()
    {
        GameObject card = CardDatabase.Instance.DrawDefuseCard();
        AddCardToPlayerHand(card);
    }

    private void UpdatePlayerDeckUI(GameObject card)
    {
        Transform cardInstance = Instantiate(card.transform, transform);
        CardUI cardUI = cardInstance.GetComponent<CardUI>();
        cardUI.SetCard();
        cardUI.FaceDownCard(!playable);
    }  
}
