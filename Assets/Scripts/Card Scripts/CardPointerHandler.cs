using UnityEngine;
using UnityEngine.EventSystems;

public class CardPointerHandler : MonoBehaviour,
IPointerEnterHandler,IPointerExitHandler,
IPointerUpHandler,IPointerDownHandler
{
    private CardDisplay cardDisplay;
    public float hoverOffset = 60f;
    private Vector3 originalPosition;

    public void Awake()
    {
        cardDisplay = GetComponent<CardDisplay>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        originalPosition = transform.localPosition;
        transform.localPosition += new Vector3(0, hoverOffset, 0);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localPosition = originalPosition;        
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        //Debug.Log("OnPointerUp");
        int handIndex = GameManager.Instance.GetHandIndex(
                                GetComponentInParent<HandManager>());

        transform.localPosition = originalPosition;
        GameManager.Instance.TryPlayCard(cardDisplay.getCardData(),handIndex);
    }

    public void OnPointerDown(PointerEventData eventData){}

    
}
