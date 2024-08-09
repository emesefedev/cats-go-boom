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

        SeeFuturePanelUI.Instance.HideSeeFuturePanel();

        foreach (PlayerHand player in players)
        {
            player.InitializePlayerDeck();
        }

        CardDatabase.Instance.CompleteDrawDeck();
    }

    private void AddExtraTurnForNextPlayer()
    {
        extraTurn = true;
    }

    private void NonPlayablePlayerPlaysTurn(Turn currentTurn) 
    {
        bool nonPlayablePlayer = currentTurn != Turn.Player1;
        if (nonPlayablePlayer) 
        {
            StartCoroutine(players[1].PlayTurnAutomatically());
        }
    }

    public void ChangeTurn(bool addExtraTurnToNextPlayer = false)
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
            Debug.Log("It's the extra turn");
            extraTurn = false;
        }
        
        if (addExtraTurnToNextPlayer)
        {
            AddExtraTurnForNextPlayer();
            Debug.Log($"There will be an extra turn for {currentTurn}");
        }
        
        Debug.Log($"It's {currentTurn} turn");

        OnTurnChange?.Invoke(currentTurn);
    }

    public void GameOver()
    {
        Debug.Log($"!!! GAME OVER !!!");
    }

    public Turn GetCurrentTurn()
    {
        return currentTurn;
    }
}
