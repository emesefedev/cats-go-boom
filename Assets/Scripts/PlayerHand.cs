using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    public static PlayerHand Instance { get; private set;}

    [SerializeField] private List<GameObject> hand = new List<GameObject>();

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

    private void UpdatePlayerDeckUI(GameObject card)
    {
        Transform cardTransform = Instantiate(card.transform, transform);
        CardUI cardUI = card.GetComponent<CardUI>();
        cardUI.SetCard();
    }   

    public void PlayerDrawCard()
    {
        GameObject card = CardDatabase.Instance.DrawCard();
        hand.Add(card);
        UpdatePlayerDeckUI(card);
    }

    private void PlayerDrawDefuseCard()
    {
        GameObject card = CardDatabase.Instance.DrawDefuseCard();
        hand.Add(card);
        UpdatePlayerDeckUI(card);
    }
}
