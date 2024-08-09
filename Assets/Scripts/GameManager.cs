using System;
using UnityEngine;

public enum Turn {
    Player1,
    Player2,
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set;}
    public static event Action<Turn> OnTurnChange;

    private bool extraTurn;

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
        Debug.Log($"{currentTurn} starts");

        foreach (PlayerHand player in players)
        {
            player.InitializePlayerDeck();
        }

        CardDatabase.Instance.CompleteDrawDeck();
    }

    public void AddExtraTurnForNextPlayer()
    {
        extraTurn = true;
    }

    public void ChangeTurn()
    { 
        Debug.Log("Change Turn has been called");
        if (!extraTurn)
        {
            currentTurn = currentTurn == Turn.Player1
            ? Turn.Player2
            : Turn.Player1;
        }
        else 
        {
            Debug.Log("There will be an extra turn");
            extraTurn = false;
        }
        
        
        Debug.Log($"It's {currentTurn} turn");

        OnTurnChange?.Invoke(currentTurn);
    }

    private void NonPlayablePlayerPlaysTurn(Turn currentTurn) 
    {
        bool nonPlayablePlayer = currentTurn != Turn.Player1;
        if (nonPlayablePlayer) 
        {
            StartCoroutine(players[1].PlayTurnAutomatically());
        }
    }
}
