using TMPro;
using UnityEngine;

public class DrawDeckUI : MonoBehaviour
{
    public static DrawDeckUI Instance { get; private set;}

    [SerializeField] private TextMeshProUGUI cardsInDrawDeckText;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("More than one instance");
        }
        Instance = this;
    }

    public void UpdateCardsInDrawDeckText(int totalCards)
    {
        cardsInDrawDeckText.text = totalCards.ToString();
    }
}
