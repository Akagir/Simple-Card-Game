using System.Collections.Generic;
using AkagirSCG;
using UnityEngine;

public class DrawPileManager : MonoBehaviour
{
    public static DrawPileManager Instance;
    private List<CardData> drawPile = new List<CardData>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);

        InitializeDrawPile();
        Debug.Log("Draw Pile initialize with " + drawPile.Count + " cards!");
    }

    void Start()
    {
        
    }

    public CardData DrawCardFromPile()
    {
        if(drawPile.Count <= 1) return null;

        CardData drawnCard;
        int topCardIndex = drawPile.Count-1;

        drawnCard = drawPile[topCardIndex];
        //Debug.Log("Drawn card in pile: " + drawnCard.number);

        drawPile.RemoveAt(topCardIndex);

        return drawnCard;
    }

    public void InitializeDrawPile()
    {
        drawPile.Clear();

        drawPile.Add(new CardData(CardColor.Red,CardType.Number,0));
        drawPile.Add(new CardData(CardColor.Yellow,CardType.Number,0));
        drawPile.Add(new CardData(CardColor.Green,CardType.Number,0));
        drawPile.Add(new CardData(CardColor.Blue,CardType.Number,0));

        for(int i=0;i<2;i++)
        {
            foreach (CardColor selectColor in System.Enum.GetValues(typeof(CardColor)))
            {
                if(selectColor != CardColor.Wild)
                {
                    for(int j=1; j < 10; j++)
                        drawPile.Add(new CardData(selectColor,CardType.Number,j));

                    drawPile.Add(new CardData(selectColor,CardType.Reverse,-1));
                    drawPile.Add(new CardData(selectColor,CardType.Skip,-1));
                    drawPile.Add(new CardData(selectColor,CardType.PlusTwo,-1));
                }
                else
                {
                    drawPile.Add(new CardData(selectColor,CardType.WildColorChange,-1));
                    drawPile.Add(new CardData(selectColor,CardType.WildColorChange,-1));
                    drawPile.Add(new CardData(selectColor,CardType.WildPlusFour,-1));
                    drawPile.Add(new CardData(selectColor,CardType.WildPlusFour,-1));
                }
            }
        }

        ShuffleDrawPile();
    }

    private void ShuffleDrawPile()
    {
        for (int i = 0; i < drawPile.Count; i++)
        {
            CardData temp = drawPile[i];
            int rnd = Random.Range(i, drawPile.Count);
            drawPile[i] = drawPile[rnd];
            drawPile[rnd] = temp;
        }
    }


}
