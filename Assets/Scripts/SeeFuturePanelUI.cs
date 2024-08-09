using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SeeFuturePanelUI : MonoBehaviour
{
    public static SeeFuturePanelUI Instance { get; private set;}

    [SerializeField] private CardUI[] seeFutureCardsArray;
    [SerializeField] private Button closeButton;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("More than one instance");
        }

        Instance = this;

        closeButton.onClick.AddListener(HideSeeFuturePanel);
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
