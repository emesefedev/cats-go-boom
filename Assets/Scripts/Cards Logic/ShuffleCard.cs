public class ShuffleCard : CardLogic
{
    public override void PlayCard()
    {
        // TODO: Make something visual to tell the player the draw deck has been shuffled
        CardDatabase.Instance.ShuffleDeck();
    }
}
