using System.Collections.Generic;
using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    [SerializeField] private List<GameObject> hand = new List<GameObject>();
    [SerializeField] bool playable;

    public void InitializePlayerDeck()
    {
        for(int i = 0; i < 7; i++)
        {
            PlayerDrawCard();
        }

        PlayerDrawDefuseCard();
    } 

    public void PlayerDrawCard()
    {
        GameObject card = CardDatabase.Instance.DrawCard();
        if (card != null)
        {
            hand.Add(card);
            UpdatePlayerDeckUI(card);
        }
    }

    private void PlayerDrawDefuseCard()
    {
        GameObject card = CardDatabase.Instance.DrawDefuseCard();
        hand.Add(card);
        UpdatePlayerDeckUI(card);
    }

    private void UpdatePlayerDeckUI(GameObject card)
    {
        Instantiate(card.transform, transform);
        CardUI cardUI = card.GetComponent<CardUI>();
        cardUI.SetCard();
        cardUI.FaceDownCard(!playable);
    }  
}
