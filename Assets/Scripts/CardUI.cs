using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardUI : MonoBehaviour
{
    private CardSO cardSO;

    [SerializeField] private Image cardBorder;
    [SerializeField] private Image cardIllustration;
    
    [SerializeField] private GameObject cardBack;

    [SerializeField] private TextMeshProUGUI titleTMP;
    [SerializeField] private TextMeshProUGUI descriptionTMP;

    private bool isFaceDown;

    private int typeIndex;
    private Color cardColor;

    /// <summary>
    /// This array needs to have as many elements as the CardType enum
    /// </summary>
    private readonly static string[] cardTitles = new string[]{
        "Attaaack",        // Attack
        "Cat went boom",   // Boom
        "Cat Card",        // Cat
        "Defuse",          // Defuse
        "Do me a favor",   // Favor
        "Nope",            // Nope
        "See the future",  // SeeFuture
        "Shuffle",         // Shuffle
        "Skip"             // Skip
    };

    public CardType GetCardType() {
        if (cardSO == null)
        {
            Debug.LogError("No cardSO set");
        }
        return cardSO.cardType;
    }

    public CardSubType GetCardSubType() {
        if (cardSO == null)
        {
            Debug.LogError("No cardSO set");
        }
        return cardSO.cardSubType;
    }

    public void SetCard(CardSO cardSO, bool isFaceDown)
    {
        SetCardSO(cardSO);

        FaceDownCard(isFaceDown);

        typeIndex = (int)cardSO.cardType;
        cardColor = Colors.cardColors[typeIndex];

        cardBorder.color = cardColor;
        cardIllustration.color = cardColor;

        titleTMP.text = GetCardType() == CardType.Cat 
        ? cardSO.cardSubType.ToString()
        : cardTitles[typeIndex];
        
        descriptionTMP.text = cardSO.description;
    }

    public void FaceDownCard(bool isFaceDown)
    {
        cardBack.SetActive(isFaceDown);
        this.isFaceDown = isFaceDown;
    }

    private void SetCardSO(CardSO cardSO)
    {
        this.cardSO = cardSO;
    }
}