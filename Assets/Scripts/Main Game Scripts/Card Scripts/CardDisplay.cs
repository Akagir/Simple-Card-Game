using UnityEngine;
using UnityEngine.UI;
using AkagirSCG;
using TMPro;
using System;
using System.Collections;
using Vector3 = UnityEngine.Vector3;
using Quaternion = UnityEngine.Quaternion;

public class CardDisplay : MonoBehaviour
{
    public GameObject cardBackSide;
    public Image backgroundImage;
    public TMP_Text topLeftText;
    public TMP_Text botRightText;
    public CardData cardData;
    public float dropDuration = 0.45f;
    public float peakScaleMult = 1.8f;

    public float drawDuration = 0.35f;

    void Update()
    {
        UpdateCardVisual();
    }

    public CardData getCardData()
    {
        return cardData;
    }
    public void Setup(CardData inData)
    {
        cardData = inData;
        UpdateCardVisual();
    }

    public void SetFaceUp(bool isFaceUp)
    {
        if(cardBackSide != null)
            cardBackSide.SetActive(!isFaceUp);
    }

    public void UpdateCardVisual()
    {
        // Texts to Update
        string displayText;
        switch (cardData.type)
        {
            case CardType.Number:
                displayText = cardData.number.ToString();
                break;
            case CardType.Skip:
                displayText = "->";
                break;
            case CardType.Reverse:
                displayText = "<<";
                break;
            case CardType.WildColorChange:
                displayText = "C";
                break;
            case CardType.PlusTwo:
                displayText = "+2";
                break;
            case CardType.WildPlusFour:
                displayText = "+4";
                break;
            default:
                displayText = "-1";
                break;
        }
        topLeftText.text = displayText;
        botRightText.text = displayText;
        
        // Color to Update
        backgroundImage.color = TranslateColor(cardData.color);
    }
    
    private Color TranslateColor(CardColor color)
    {
        return color switch
        {
            CardColor.Red => Color.red,
            CardColor.Yellow => Color.yellow,
            CardColor.Green => Color.green,
            CardColor.Blue => Color.blue,
            _ => Color.gray3
        };
    }

    public void ActivateDropAnimation(Transform targetTransform, Action onComplete)
    {
        StartCoroutine(
            AnimateDropRoutine(targetTransform, onComplete));
    }

    public void ActivateDrawAnimation(Transform targetTransform, Action onComplete)
    {
        StartCoroutine(AnimateDrawRoutine(targetTransform,onComplete));
    }

    private IEnumerator AnimateDropRoutine(Transform targetTransform, Action onComplete)
    {
        Vector3 startPos = transform.position;
        Quaternion startRot = transform.rotation;
        Vector3 startScale = transform.localScale;

        Vector3 endPos = targetTransform.position;
        Quaternion endRot = targetTransform.rotation;
        Vector3 peakScale = startScale * peakScaleMult;

        float elapsed = 0f;        
        while (elapsed < dropDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / dropDuration;
            t = t*t * (3f - (2f*t));
            
            transform.position = Vector3.Lerp(startPos, endPos, t);
            transform.rotation = Quaternion.Lerp(startRot, endRot, t);
            
            if(elapsed < (dropDuration/2f))
                transform.localScale = Vector3.Lerp(startScale, peakScale, t);
            else
                transform.localScale = Vector3.Lerp(peakScale, startScale, t);
            
            yield return null;
        }
        //UnityEngine.Debug.Log("Drop animation is done!");
        onComplete?.Invoke();
    }

    private IEnumerator AnimateDrawRoutine(Transform targetTransform, Action onComplete)
    {
        Vector3 startPos = targetTransform.position;
        Quaternion startRot = targetTransform.rotation;

        Vector3 endPos = transform.position;
        Quaternion endRot = transform.rotation;

        float elapsed = 0f;        
        while (elapsed < drawDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / drawDuration;
            t = t*t * (3f - (2f*t));

            transform.position = Vector3.Lerp(startPos, endPos, t);
            transform.rotation = Quaternion.Lerp(startRot, endRot, t);
            yield return null;
        }
        onComplete?.Invoke();
    }
}
