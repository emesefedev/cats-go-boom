using UnityEngine;

public class SkipCard : CardLogic
{
    public override void PlayCard()
    {
        Debug.Log("Skip Card played");
        GameManager.Instance.ChangeTurn();
    }
}
