using System.Collections;
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

    public void DisableFirstVisibleChild()
    {
        // There's a bug and totalCardsInDiscardDeck's value is always one less. That's why we sum +1
        int totalCardsInDiscardDeck = transform.childCount + 1;
        Debug.Log($"hulu {totalCardsInDiscardDeck} cards");
    
        if (totalCardsInDiscardDeck > 3)
        {
            Debug.Log("hulu");
            // We disable the fourth child starting from the end
            transform.GetChild(totalCardsInDiscardDeck - 4).gameObject.SetActive(false);
        }
    }
}
