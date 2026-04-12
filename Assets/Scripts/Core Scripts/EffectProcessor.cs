using AkagirSCG;
using UnityEngine;

public class EffectProcessor : MonoBehaviour
{
    public static EffectProcessor Instance;
    public int pendingDraws = 0;
    public GameObject colorSelectionPanel;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);        
    }

    public void ProcessCardEffect(CardData drawnCard)
    {
        switch(drawnCard.type)
        {
            case CardType.Skip:
                TurnManager.Instance.SkipNextPlayer();
            break;

            case CardType.Reverse:
                TurnManager.Instance.ReverseDirection();
                if (TurnManager.Instance.totalPlayers == 2)
                    TurnManager.Instance.SkipNextPlayer();
            break;

            case CardType.PlusTwo:
                pendingDraws += 2;
            break;

            case CardType.WildPlusFour:
                pendingDraws += 4; 
                TriggerColorSelection();
            break;

            case CardType.WildColorChange:
                TriggerColorSelection();
            break;

            default:
            break;
        }
    }

    public void TriggerColorSelection()
    {
        if (colorSelectionPanel != null)
        {
            GameManager.Instance.isWaiting = true;
            colorSelectionPanel.SetActive(true);
        }
        else
            Debug.LogWarning("Color Selection Panel isn't assigned!");
    }

    public void OnColorSelected(CardColor chosenColor) 
    {
        
        GameManager.Instance.currentActiveColor = chosenColor;
        DropPileManager.Instance.ChangeTopCardColor(chosenColor);

        colorSelectionPanel.SetActive(false);
        TurnManager.Instance.PassTurn();
        GameManager.Instance.isWaiting = false;
    }

    public void ConsumePendingDraws(int targetPlayerIndex)
    {
        if (pendingDraws > 0)
        {
            for (int i = 0; i < pendingDraws; i++)
                GameManager.Instance.ForceDrawCard(targetPlayerIndex);
            pendingDraws = 0;
        }
    }

}
