using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    public static PlayerHand Instance { get; private set;}

    [SerializeField] private List<CardSO> hand = new List<CardSO>();

    [SerializeField] private Transform cardPrefab;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("More than one instance");
        }
        Instance = this;
    }

    private void Start()
    {
        InitializePlayerDeck();
        CardDatabase.Instance.CompleteDrawDeck();
    }

    private void InitializePlayerDeck()
    {
        for(int i = 0; i < 7; i++)
        {
            PlayerDrawCard();
        }

        PlayerDrawDefuseCard();
    }

    private void UpdatePlayerDeckUI(CardSO cardSO)
    {
        Transform card = Instantiate(cardPrefab, transform);
        CardUI cardUI = card.GetComponent<CardUI>();
        cardUI.SetCard(cardSO);
    }   

    public void PlayerDrawCard()
    {
        CardSO card = CardDatabase.Instance.DrawCard();
        hand.Add(card);
        UpdatePlayerDeckUI(card);
    }

    private void PlayerDrawDefuseCard()
    {
        CardSO card = CardDatabase.Instance.DrawDefuseCard();
        hand.Add(card);
        UpdatePlayerDeckUI(card);
    }
}
