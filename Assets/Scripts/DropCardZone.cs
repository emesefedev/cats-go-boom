using UnityEngine;
using UnityEngine.EventSystems;

public class DropCardZone : MonoBehaviour, IDropHandler, IPointerEnterHandler
{
    public const string DISCARD_DECK_TAG = "Discard Deck";

    public void OnDrop(PointerEventData eventData)
    {
        if (gameObject.CompareTag(DISCARD_DECK_TAG))
        {
            CardLogic cardLogic = eventData.pointerDrag.GetComponent<CardLogic>();
            if (cardLogic != null) 
            {
                cardLogic.PlayCard();
                DiscardDeckUI.Instance.DisableFirstVisibleChild();
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GameObject draggedGameObject = eventData.pointerDrag;
        if (draggedGameObject != null)
        {
            draggedGameObject.GetComponent<DragCard>().ChangeParent(transform);
        }
    }
}
