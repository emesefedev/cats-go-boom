using System.Collections.Generic;
using UnityEngine;

public class SeeFuturePanelUI : MonoBehaviour
{

    public static SeeFuturePanelUI Instance { get; private set;}

    [SerializeField] private CardUI[] seeFutureCardsArray;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("More than one instance");
        }

        Instance = this;
    }

    public void ShowSeeFuturePanel()
    {
        gameObject.SetActive(true);
        
        SetSeeFutureCards();
    }

    public void HideSeeFuturePanel()
    {
        gameObject.SetActive(false);
    }

    private void SetSeeFutureCards()
    {
        List<CardSO> seeFutureCardsSO = CardDatabase.Instance.GetFirstThreeCardsFromDrawDeck();
        for (int i = 0; i < seeFutureCardsArray.Length; i++)
        {
            seeFutureCardsArray[i].SetCard(seeFutureCardsSO[i]);
        }
    }
}
