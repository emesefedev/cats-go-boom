using System;
using UnityEngine;

public enum Turn {
    Player1,
    Player2
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set;}
    public static event Action<Turn> OnTurnChange;

    public PlayerHand[] players;

    private Turn currentTurn;

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
        StartGame();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            players[0].PlayTurnAutomatically();
        }
    }

    private void StartGame()
    {
        currentTurn = Turn.Player1;

        foreach (PlayerHand player in players)
        {
            player.InitializePlayerDeck();
        }

        CardDatabase.Instance.CompleteDrawDeck();
    }

    public void ChangeTurn()
    {
        currentTurn = currentTurn == Turn.Player1
        ? Turn.Player2
        : Turn.Player1;

        OnTurnChange?.Invoke(currentTurn);
    }
}
