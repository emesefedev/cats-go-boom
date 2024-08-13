using UnityEngine;

public class HandCard 
{
    private GameObject cardInstance;
    private CardSO cardSO;
    private PlayerHand player;

    private CardUI cardUI;
    private CardLogic cardLogic;

    public HandCard(GameObject cardInstance, CardSO cardSO, PlayerHand player)
    {
        this.cardInstance = cardInstance;
        this.cardSO = cardSO;
        this.player = player;

        SetUpCardLogic();
        SetUpCardVisual();
    }

    public bool PlayHandCard()
    {
        return cardLogic.PlayCard();
    }

    public void FaceDownCard(bool isfaceDown)
    {
        cardUI.FaceDownCard(isfaceDown);
    }

    public void SetParent(Transform transform)
    {
        cardInstance.transform.parent = transform;
    }

    public CardType GetCardType()
    {
        return cardSO.cardType;
    }

    public CardSubType GetCardSubType()
    {
        return cardSO.cardSubType;
    }

    private void SetUpCardVisual()
    {
        cardUI = cardInstance.GetComponent<CardUI>();
        cardUI.SetCard(cardSO, !player.PlayerIsPlayable());
    }

    private void SetUpCardLogic()
    {
        cardLogic = cardInstance.AddComponent<CardLogic>();

        cardLogic.SetCardType(cardSO.cardType, cardSO.cardSubType);
        cardLogic.SetCardPlayer(player);
    }
}
