using UnityEngine;

public enum CardType {
    Attack,
    Boom,
    Cat,
    Defuse,
    Favor, 
    Nope,
    SeeFuture,
    Shuffle,
    Skip
}

[CreateAssetMenu(menuName = "Card")]
public class CardSO : ScriptableObject
{
    [TextArea] public string description;
    public CardType cardType;
}
