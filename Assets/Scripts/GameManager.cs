using System;
using UnityEngine;

public enum Turn {
    Player1,
    Player2,
    Player3,
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set;}
    public static event Action<Turn> OnTurnChange;

    // The game is intended for two players, but more players may be added in the future
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

    private void OnEnable() 
    {
        OnTurnChange += NonPlayablePlayerPlaysTurn;
    }

    private void OnDisable() 
    {
        OnTurnChange -= NonPlayablePlayerPlaysTurn;
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

    private void NonPlayablePlayerPlaysTurn(Turn currentTurn) 
    {
        bool nonPlayablePlayer = currentTurn != Turn.Player1;
        if (nonPlayablePlayer) 
        {
            players[1].PlayTurnAutomatically();
        }
    }
}
