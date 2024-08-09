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
        transform.SetParent(parentToReturnTo);
        
        if (parentToReturnTo == originalParent)
        {
            PlayerHand playerHand = originalParent.GetComponent<PlayerHand>();
            if (playerHand == null)
            {
                Debug.LogError("playerHand should not be null");
            }

            playerHand.UpdatePlayerDeckUI();
        }

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
