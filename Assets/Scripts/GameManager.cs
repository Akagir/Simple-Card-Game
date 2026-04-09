using UnityEngine;
using AkagirSCG;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public HandManager[] allPlayerHands;
    public int startingHandSize = 7;
    
    public CardColor currentActiveColor;
    private bool isGameOver = false;

    [Header("End Game UI")]
    public GameObject victoryScreenPanel;
    public TMP_Text victoryWinnerText;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);        
    }

    void Start()
    {
        TurnManager.Instance.OnTurnStarted += HandleTurnStart;
        TurnManager.Instance.OnTurnEnded += HandleTurnEnd;
        StartMatch();
    }


    // ==========================================
    // CORE FUNCTIONS
    // ==========================================

    private void StartMatch()
    {
        // Start dealing hands to all players
        foreach (HandManager hand in allPlayerHands)
        {
            for (int i = 0; i < startingHandSize; i++)
            {
                CardData drawnCard = DrawPileManager.Instance.DrawCardFromPile();
                if (drawnCard != null) {
                    hand.AddCardToHand(drawnCard);
                }
            }
        }

        // Flip the first card onto the discard pile
        CardData firstDrop = DrawPileManager.Instance.DrawCardFromPile();
        DropPileManager.Instance.InitializeFirstDroppedCard(firstDrop);
        currentActiveColor = firstDrop.color;

        // 3. Tell the TurnManager to begin!
        TurnManager.Instance.InitializeMatch(allPlayerHands.Length);
    }

    public void TryDrawCard(int attempterPlayerIndex)
    {
        Debug.Log("Drawing card for player: " + attempterPlayerIndex);
        if (isGameOver) return;

        if (attempterPlayerIndex != TurnManager.Instance.currentPlayerIndex) return;

        // Get a card from the deck
        CardData drawnCard = DrawPileManager.Instance.DrawCardFromPile();

        // Give it to the active player
        if (drawnCard != null)
        {
            HandManager activeHand = GetHandByIndex(attempterPlayerIndex);
            if(activeHand != null)
            {
                Debug.Log("Active hand has get!");
                activeHand.AddCardToHand(drawnCard);
            }
        }
        else
            Debug.Log("Drawn card is NULL!");
    }

    public void TryPlayCard(CardData playedCardData, int attempterPlayerIndex)
    {
        if (isGameOver) return;

        if(attempterPlayerIndex != TurnManager.Instance.currentPlayerIndex)
        {
            Debug.LogWarning("Not your turn!");
            return;
        }

        CardData topCardData = DropPileManager.Instance.GetTopCard();
        if (IsValidUnoPlay(playedCardData, topCardData))
        {
            HandManager activeHand = GetHandByIndex(attempterPlayerIndex);
            activeHand.RemoveCardFromHand(playedCardData);

            DropPileManager.Instance.AddCardToDropPile(playedCardData);
            currentActiveColor = playedCardData.color;
            // SHOULD DIFFER WHEN A WILD CARD IS DROPPED
            if (activeHand.GetCardCount() == 0)
            {
                DeclareWinner(attempterPlayerIndex);
                return; // Stop here so the turn doesn't pass
            }
            TurnManager.Instance.PassTurn();
        }
    }

    // ==========================================
    // HELPER FUNCTIONS
    // ==========================================
    private HandManager GetHandByIndex(int index)
    {
        // Safety check to prevent out-of-bounds errors
        if (index < 0 || index >= allPlayerHands.Length) return null;
        return allPlayerHands[index];
    }

    public int GetHandIndex(HandManager hand)
    {
        for (int i = 0; i < allPlayerHands.Length; i++)
        {
            if (allPlayerHands[i] == hand)
            {
                return i;
            }
        }
        
        Debug.LogWarning("HandManager not found in allPlayerHands!");
        return -1;
    }
    
    private bool IsValidUnoPlay(CardData playedCardData, CardData topCardData)
    {
        if(playedCardData.type == CardType.WildColorChange
                || playedCardData.type == CardType.WildPlusFour) return true;

        if(playedCardData.color == currentActiveColor) return true;

        if(playedCardData.type == CardType.Number
                && playedCardData.number == topCardData.number) return true;

        return false;
    }

    private void DeclareWinner(int winningPlayerIndex)
    {
        isGameOver = true;
        
        if (victoryScreenPanel != null)
        {
            victoryScreenPanel.SetActive(true);
            
            if (winningPlayerIndex == 0) {
                victoryWinnerText.text = "You Win!";
            } else {
                victoryWinnerText.text = $"AI {winningPlayerIndex} Wins!";
            }
        }
    }

    // ==========================================
    // TURN EVENTS
    // ==========================================

    private void HandleTurnStart(int playerIndex)
    {
        if (isGameOver) return;

        Debug.Log($"--- Player {playerIndex}'s Turn Started ---");

        // If it's an AI's turn (index > 0), you would trigger their logic here.
        // Example: if (playerIndex > 0) GetComponent<AIManager>().TakeTurn(playerIndex);
    }

    private void HandleTurnEnd(int playerIndex)
    {
        // Useful for resetting any turn-specific visual timers or effects
    }
}