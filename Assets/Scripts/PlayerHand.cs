using System.Collections.Generic;
using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    [SerializeField] private List<CardSO> hand = new List<CardSO>();

    [SerializeField] private Transform cardPrefab;

    private void Start()
    {
        InitializePlayerDeck();
    }

    private void InitializePlayerDeck()
    {
        for(int i = 0; i < 7; i++)
        {
            hand.Add(CardDatabase.Instance.DrawCard());
        }

        UpdatePlayerDeckUI();
    }

    private void UpdatePlayerDeckUI()
    {
        foreach(CardSO cardSO in hand)
        {
            Transform card = Instantiate(cardPrefab, transform);
            CardUI cardUI = card.GetComponent<CardUI>();
            cardUI.SetCard(cardSO);
        }
    }
}
