using UnityEngine;
using System.Collections.Generic;
using AkagirSCG;

public class DropPileManager : MonoBehaviour
{
    public static DropPileManager Instance { get; private set; }

    [SerializeField] 
    private List<CardData> dropPile = new List<CardData>();
    public CardDisplay topCardDisplay;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
    }

    public void InitializeFirstDroppedCard(CardData firstCard)
    {
        dropPile.Clear();
        AddCardToDropPile(firstCard);
    }

    public void AddCardToDropPile(CardData playedCard)
    {
        if (playedCard == null) return;

        dropPile.Add(playedCard);
        topCardDisplay.Setup(playedCard);
    }

    public CardData GetTopCard()
    {
        if (dropPile.Count == 0) return null;
        return dropPile[dropPile.Count - 1];
    }

    public void ChangeTopCardColor(CardColor newColor)
    {
        Debug.Log("ChangeTopCardColor called!");

        CardData topCard = GetTopCard();
        topCard.color = newColor;
        topCardDisplay.Setup(topCard);
    }

    public List<CardData> GetCardsForReshuffle()
    {
        if (dropPile.Count <= 1) return new List<CardData>();

        CardData topCard = GetTopCard();
        List<CardData> cardsToRecycle = dropPile.GetRange(0, dropPile.Count - 1);

        dropPile.Clear();
        dropPile.Add(topCard);

        return cardsToRecycle;
    }
}
