using UnityEngine;
using UnityEngine.EventSystems;

public class DropCardZone : MonoBehaviour, IDropHandler, IPointerEnterHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        // TODO: PlayCard
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
