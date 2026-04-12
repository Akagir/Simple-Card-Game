using UnityEngine;
using System.Collections.Generic;
using AkagirSCG;

public class HandManager : MonoBehaviour
{
    [SerializeField]
    private List<CardData> cardsInHand = new List<CardData>();
    private Transform handTransform;
    public float horizontalOffset = 0;

    public float verticalOffset = -18;
    public float cardSpacing = 100f;
    public GameObject cardPrefab;

    // For highlighing hand in turn
    public GameObject handHighlighter;

    private void Awake()
    {
        handTransform = this.transform; 
    }

    public void AddCardToHand(CardData inData)
    {
        if(inData == null)
        {
            Debug.Log("Add Card is null!!!");
            return;
        }
        cardsInHand.Add(inData);
        UpdateHandVisual();
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

    public void activateHighlight()
    {
        handHighlighter.SetActive(true);
    }

    public void deactivateHighlight()
    {
        handHighlighter.SetActive(false);
    }

}
