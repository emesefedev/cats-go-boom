using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardUI : MonoBehaviour
{
    [SerializeField] private CardSO cardSO;

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
        return cardSO.cardType;
    }

    public void SetCard()
    {
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
}