using System;
using UnityEngine;

public enum Turn {
    Player1,
    Player2
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set;}
    public static event Action OnTurnChange;

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
        currentTurn = Turn.Player1;
    }

    public void ChangeTurn()
    {
        currentTurn = currentTurn == Turn.Player1
        ? Turn.Player2
        : Turn.Player1;

        OnTurnChange?.Invoke();
    }
}
