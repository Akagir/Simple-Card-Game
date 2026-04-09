using UnityEngine;
using UnityEngine.EventSystems;

public class DrawPilePointerHandler : MonoBehaviour, 
IPointerEnterHandler,IPointerExitHandler,IPointerClickHandler
{
    public GameObject highlightObject;
    [SerializeField] 
    private bool isInteractable = true;

    void Start()
    {
        if (highlightObject != null)
            highlightObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("OnPointerEnter");
        if(isInteractable)
            highlightObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log("OnPointerExit");
        highlightObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Drawing card tried!!!");
        if (GameManager.Instance == null)
            Debug.LogError("GameManager Instance is missing from the scene!");

        if(isInteractable)
            GameManager.Instance.TryDrawCard(TurnManager.Instance.currentPlayerIndex);
        //Player Index 0 is the local player
    }


}