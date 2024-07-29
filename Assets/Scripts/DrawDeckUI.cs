using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DrawDeckUI : MonoBehaviour
{
    public static DrawDeckUI Instance { get; private set;}

    [SerializeField] private TextMeshProUGUI cardsInDrawDeckText;

    [SerializeField] private Button drawButton;

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
        drawButton.onClick.AddListener(PlayerHand.Instance.PlayerDrawCard);
    }

    private void OnEnable() 
    {
        GameManager.OnTurnChange += GameManager_OnTurnChange;
    }

    private void OnDisable() 
    {
        GameManager.OnTurnChange -= GameManager_OnTurnChange;
    }

    private void GameManager_OnTurnChange(Turn currentTurn) 
    {
        EnableOrDisableDrawButtonInteractable(currentTurn);
    }

    public void UpdateCardsInDrawDeckText(int totalCards)
    {
        cardsInDrawDeckText.text = totalCards.ToString();
    }

    private void EnableOrDisableDrawButtonInteractable(Turn currentTurn) 
    {
        bool interactable = currentTurn == Turn.Player1;
        drawButton.interactable = interactable;
    }
}
