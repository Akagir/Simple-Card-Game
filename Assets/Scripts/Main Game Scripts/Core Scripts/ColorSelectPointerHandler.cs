using AkagirSCG;
using UnityEngine;
using UnityEngine.EventSystems;

public class ColorSelectPointerHandler : MonoBehaviour,
    IPointerUpHandler,IPointerDownHandler
{
    public CardColor assignedColor;
    public void OnPointerDown(PointerEventData eventData){}

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log($"Color selected: {assignedColor}");
        EffectProcessor.Instance.OnColorSelected(assignedColor);        
    }
    
}
