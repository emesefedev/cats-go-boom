using UnityEngine;

public class DiscardDeckUI : MonoBehaviour
{
    public static DiscardDeckUI Instance {get; private set;}

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("More than one instance");
        }
        Instance = this;
    }

    public void AddCardToDiscardDeck(GameObject card)
    {
        CardDatabase.Instance.UpdateDiscardDeck(card);
        card.transform.SetParent(transform);
        DisableFirstVisibleChild();
    }

    public void DisableFirstVisibleChild()
    {
        int totalCardsInDiscardDeck = transform.childCount;
    
        if (totalCardsInDiscardDeck > 3)
        {
            // We disable the fourth child starting from the end
            transform.GetChild(totalCardsInDiscardDeck - 4).gameObject.SetActive(false);
        }
    }
}
