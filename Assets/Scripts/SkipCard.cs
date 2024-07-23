using UnityEngine;

public class SkipCard : CardLogic
{
    public override void PlayCard()
    {
        GameManager.Instance.ChangeTurn();
    }
}
