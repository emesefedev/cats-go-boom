using UnityEngine;
using UnityEngine.EventSystems;

public class DropCardZone : MonoBehaviour, IDropHandler, IPointerEnterHandler
{
    public const string DISCARD_DECK_TAG = "Discard Deck";

    public void OnDrop(PointerEventData eventData)
    {
        if (gameObject.CompareTag(DISCARD_DECK_TAG))
        {
            GameObject card = eventData.pointerDrag;
            
            if (card != null) 
            {
                DragCard dragCard = card.GetComponent<DragCard>();
                Transform player = dragCard.GetOriginalParent();
                int childIndex = dragCard.GetChildIndex();
                
                bool cardCanBePlayed = player.GetComponent<PlayerHand>().PlayerPlayCard(card, childIndex);

                if (!cardCanBePlayed)
                {
                    dragCard.ChangeParent(player);
                }
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
