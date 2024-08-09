using System;
using UnityEngine;

public class CardLogic : MonoBehaviour
{
    public Action PlayCard;
    
    private CardType cardType;
    private CardSubType cardSubType;

    public void SetCardType(CardType cardType, CardSubType cardSubType)
    {
        this.cardType = cardType;
        this.cardSubType = cardSubType;

        SetPlayCardAction();
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

    private void PlayAttackCard()
    {
        Debug.Log("Attack Card played");
        GameManager.Instance.ChangeTurn(true);
    }

    private void PlayBoomCard()
    {
        Debug.Log("Boom Card played");
    }

    private void PlayCatCard()
    {
        Debug.Log("Cat Card played");
    }

    private void PlayDefuseCard()
    {
        Debug.Log("Defuse Card played");
    }

    private void PlayFavorCard()
    {
        Debug.Log("Favor Card played");
    }

    private void PlayNopeCard()
    {
        Debug.Log("Nope Card played");
    }

    private void PlaySeeFutureCard()
    {
        Debug.Log("See Future Card played");

        Turn currentTurn = GameManager.Instance.GetCurrentTurn();

        if (currentTurn == Turn.Player1)
        {
            SeeFuturePanelUI.Instance.ShowSeeFuturePanel();
        }
    }

    private void PlayShuffleCard()
    {
        Debug.Log("Shuffle Card played");
        
        // TODO: Make something visual to tell the player the draw deck has been shuffled
        CardDatabase.Instance.ShuffleDeck();
    }

    private void PlaySkipCard()
    {
        Debug.Log("Skip Card played");
        GameManager.Instance.ChangeTurn();
    }
}
