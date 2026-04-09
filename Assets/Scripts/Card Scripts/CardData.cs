using UnityEngine;

namespace AkagirSCG
{
    [System.Serializable]
public class CardData
{
    public CardColor color;
    public CardType type;
    public int number;

    public CardData(CardColor color, CardType type, int number)
    {
        this.color = color;
        this.type = type;
        this.number = number;
    }
}

public enum CardColor
{Red, Yellow, Green, Blue, Wild} //Wild -> Black
    
public enum CardType
{Number, Skip, Reverse, WildColorChange, PlusTwo, WildPlusFour}
}