using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardUI : MonoBehaviour
{
    [SerializeField] private CardSO cardSO;

    [SerializeField] private Image cardBorder;
    [SerializeField] private Image cardIllustration;

    [SerializeField] private TextMeshProUGUI titleTMP;
    [SerializeField] private TextMeshProUGUI descriptionTMP;

    private int typeIndex;
    private Color cardColor;

    /// <summary>
    /// This array needs to have as many elements as the CardType enum
    /// </summary>
    private readonly static string[] cardTitles = new string[]{
        "Attaaack",        // Attack
        "Cat went boom",   // Boom
        "Defuse",          // Defuse
        "Do me a favor",   // Favor
        "Nope",            // Nope
        "See the future",  // SeeFuture
        "Shuffle",         // Shuffle
        "Skip"             // Skip
    };

    private void Awake()
    {
        typeIndex = (int)cardSO.cardType;
        cardColor = Colors.cardColors[typeIndex];

        cardBorder.color = cardColor;
        cardIllustration.color = cardColor;

        titleTMP.text = cardTitles[typeIndex];
        descriptionTMP.text = cardSO.description;
    }
}