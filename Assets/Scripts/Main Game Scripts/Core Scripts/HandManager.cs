using UnityEngine;
using System;
using System.Collections.Generic;
using AkagirSCG;

[RequireComponent(typeof(CanvasGroup))]
public class HandManager : MonoBehaviour
{
    [SerializeField]
    private List<CardData> cardsInHand = new List<CardData>();
    private Transform handTransform;
    public float horizontalOffset = 0;

    public float verticalOffset = -18;
    public float cardSpacing = 100f;
    public GameObject cardPrefab;
    public bool isFaceDownHand = false;


    // For highlighing hand in turn
    public GameObject handHighlighter;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        handTransform = this.transform;

        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup != null)
        {
            canvasGroup.interactable = !isFaceDownHand;
            canvasGroup.blocksRaycasts = !isFaceDownHand;
        }
    }

    public void AddCardToHand(CardData inData,Transform originTransform,Action onComplete)
    {
        if(inData == null)
            Debug.Log("Add Card is null!!!");

        cardsInHand.Add(inData);
        UpdateHandVisual();
        
        if (originTransform != null && handTransform.childCount > 0)
        {
            Transform newCardObj = handTransform.GetChild(handTransform.childCount - 1);
            CardDisplay newCardDisplay = newCardObj.GetComponent<CardDisplay>();

            if (newCardDisplay != null)
            {
                newCardDisplay.SetFaceUp(false);
                newCardDisplay.ActivateDrawAnimation(originTransform, () =>
                {
                    if (!isFaceDownHand)
                        newCardDisplay.SetFaceUp(true);
                    onComplete?.Invoke();
                });
            }
        }
        onComplete?.Invoke();        
    }

    public void RemoveCardFromHand(CardData inData)
    {
        if(cardsInHand.Contains(inData))
            cardsInHand.Remove(inData);
        UpdateHandVisual();
    }

    public void UpdateHandVisual()
    {
        int cardCount = GetCardCount();
        //Debug.Log("Current cardCount: "+cardCount);

        foreach (Transform child in handTransform)
        {
            if (child.GetComponent<CardDisplay>() != null)
                Destroy(child.gameObject);
        }

        List<GameObject> cardObjects = new List<GameObject>();
        

        for(int i=0; i < cardCount ; i++)
        {
            GameObject newCard = Instantiate(cardPrefab, 
                                        handTransform.position,
                                        handTransform.rotation,
                                        handTransform);

            newCard.GetComponent<CardDisplay>().Setup(cardsInHand[i]);
            newCard.GetComponent<CardDisplay>().SetFaceUp(!isFaceDownHand);
            cardObjects.Add(newCard);
        }
        
        if(cardCount < 19)
        {
            for(int i=0; i < cardCount ; i++)
            {
                horizontalOffset = cardSpacing * (i - (cardCount - 1) / 2f);
                cardObjects[i].transform.localPosition = new Vector3(horizontalOffset, verticalOffset, 0f);
            }
        }
        else
        {
            for(int i=0; i < cardCount ; i++)
            {
                horizontalOffset = (cardSpacing - 16) * (i - (cardCount - 1) / 2f);
                cardObjects[i].transform.localPosition = new Vector3(horizontalOffset, verticalOffset, 0f);
            }
        }
    }

    public int GetCardCount()
    {
        return cardsInHand.Count;
    }

    public List<CardData> GetCardsInHand()
    {
        return cardsInHand;
    }

    public void activateHighlight()
    {
        handHighlighter.SetActive(true);
    }

    public void deactivateHighlight()
    {
        handHighlighter.SetActive(false);
    }

    public GameObject ExtractCardObject(CardData targetData)
    {
        GameObject extractObject = null;
        foreach (Transform child in handTransform)
        {
            if(child.GetComponent<CardDisplay>() != null)
            {
                CardDisplay display = child.GetComponent<CardDisplay>();
                if(display.cardData == targetData)
                {
                    display.SetFaceUp(true);
                    child.SetParent(handTransform.parent);
                    extractObject = child.gameObject;
                }
            }
            
        }
        return extractObject;
    }
}
