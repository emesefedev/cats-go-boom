using UnityEngine;

/// <summary>
    /// If this enum is modified, totalCardsPerType, cardColors and cardTitles must also be modified
/// </summary>
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

public enum CardSubType {
    None,
    AirCat,
    EarthCat,
    EtherCat,
    FireCat,
    WaterCat
}

[CreateAssetMenu(menuName = "Card")]
public class CardSO : ScriptableObject
{
    [TextArea] public string description;
    public CardType cardType;
    public CardSubType cardSubType;
}
