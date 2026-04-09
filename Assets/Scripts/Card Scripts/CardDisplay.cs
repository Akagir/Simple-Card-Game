using UnityEngine;
using UnityEngine.UI;
using AkagirSCG;
using TMPro;

public class CardDisplay : MonoBehaviour
{
    public Image backgroundSprite;
    public TMP_Text topLeftText;
    public TMP_Text botRightText;
    public CardData cardData;

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
        backgroundSprite.color = TranslateColor(cardData.color);
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
}
