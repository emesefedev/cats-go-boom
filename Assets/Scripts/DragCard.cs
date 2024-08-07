using UnityEngine;
using UnityEngine.EventSystems;

public class DragCard : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private CanvasGroup canvasGroup;
    private Transform parentToReturnTo = null;
    private Transform originalParent = null;
    private int childIndex = -1;

    public void OnBeginDrag(PointerEventData eventData)
    {
        childIndex = transform.GetSiblingIndex();

        originalParent = transform.parent;
        parentToReturnTo = transform.parent;

        transform.SetParent(transform.parent.parent);

        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // TODO: Si ha vuelto a la hand, reordenar la hand para que la carta movida pase a estar en último lugar, porque si no se quedan desordenadas y se depende de dicho orden
        transform.SetParent(parentToReturnTo);
        canvasGroup.blocksRaycasts = true;
    }   

    public void ChangeParent(Transform newParent)
    {
        parentToReturnTo = newParent;
    }

    public Transform GetOriginalParent()
    {
        return originalParent;
    }

    public int GetChildIndex()
    {
        return childIndex;
    }
}
