using System;
using UnityEngine;

public class CardLogic : MonoBehaviour
{
    public Func<bool> PlayCard;
    
    private CardType cardType;
    private CardSubType cardSubType;

    private PlayerHand cardPlayer;

    public void SetCardType(CardType cardType, CardSubType cardSubType)
    {
        this.cardType = cardType;
        this.cardSubType = cardSubType;

        SetPlayCardAction();
    }

    public void SetCardPlayer(PlayerHand cardPlayer)
    {
        this.cardPlayer = cardPlayer;
    }

    public void SetPlayCardAction()
    {
        PlayCard = cardType switch 
        {
            CardType.Attack => PlayAttackCard,
            CardType.Boom => PlayBoomCard,
            CardType.Cat => PlayCatCard,
            CardType.Defuse => PlayDefuseCard,
            CardType.Favor => PlayFavorCard,
            CardType.Nope => PlayNopeCard,
            CardType.SeeFuture => PlaySeeFutureCard,
            CardType.Shuffle => PlayShuffleCard,
            CardType.Skip => PlaySkipCard,
            _ => throw new ArgumentException("The value of cardType does not match any valid CardType", 
                nameof(cardType))
        };
    }

    private bool PlayAttackCard()
    {
        Debug.Log("Attack Card played");
        
        GameManager.Instance.ChangeTurn(true);
        
        return true;
    }

    private bool PlayBoomCard()
    {
        Debug.Log("Boom Card played");
        return true;
    }

    private bool PlayCatCard()
    {
        Debug.Log("Cat Card played");
        Debug.Log($"Card is {cardSubType} subtype");

        if (cardPlayer.PlayerHasSameCardInHand(cardType, cardSubType))
        {
            Debug.Log("Cat card can be played");
            cardPlayer.RemoveFirstCopyStartingAtTheEndOfTheHand(cardType, cardSubType);
            
            return true;
        }
        else
        {
            Debug.Log("Cat card can't be played");
            return false;
        }
    }

    private bool PlayDefuseCard()
    {
        Debug.Log("Defuse Card played");
        return true;
    }

    private bool PlayFavorCard()
    {
        Debug.Log("Favor Card played");
        return true;
    }

    private bool PlayNopeCard()
    {
        Debug.Log("Nope Card played");
        return true;
    }

    private bool PlaySeeFutureCard()
    {
        Debug.Log("See Future Card played");

        // This will work only if Player1 is the only playable player
        if (cardPlayer.IsPlayable()) 
        {
            SeeFuturePanelUI.Instance.ShowSeeFuturePanel();
        }

        return true;
    }

    private bool PlayShuffleCard()
    {
        Debug.Log("Shuffle Card played");
        
        // TODO: Make something visual to tell the player the draw deck has been shuffled
        CardDatabase.Instance.ShuffleDeck();

        return true;
    }

    private bool PlaySkipCard()
    {
        Debug.Log("Skip Card played");
        GameManager.Instance.ChangeTurn();
        return true;
    }
}
