using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AkagirSCG;
using UnityEngine.SocialPlatforms.Impl;

public class AIHandController : MonoBehaviour
{
    public int aiPlayerIndex;
    public float thinkDelay = 1.5f;
    private HandManager aiHand;
    private CardData topCard;
    [SerializeField]
    private CardColor mostFreqColor;

    // Debugging variable
    public int[] scores;

    void Start()
    {
        aiHand = GetComponent<HandManager>();
        // Subscribe to the turn system
        TurnManager.Instance.OnTurnStarted += HandleTurnStarted;
    }

    private void OnDestroy()
    {
        if (TurnManager.Instance != null)
            TurnManager.Instance.OnTurnStarted -= HandleTurnStarted;
    }

    private void HandleTurnStarted(int playerIndex)
    {
        // If it is this specific AI's turn, begin the decision process
        if (playerIndex == aiPlayerIndex)
            StartCoroutine(PlayTurnRoutine());
    }

    // ==========================================
    // CORE FUNCTIONS
    // ==========================================

    private IEnumerator PlayTurnRoutine()
    {
        List<CardData> handCards = aiHand.GetCardsInHand();
        topCard = DropPileManager.Instance.GetTopCard();
        mostFreqColor = GetMostFrequentColor(handCards);

        CardData bestCard = null;
        int bestScore = -1;
        // Debugging
        scores = new int[handCards.Count];
        int i = 0;

        yield return new WaitForSeconds(thinkDelay/2.0f);
        foreach (CardData card in handCards)
        {
            if( GameManager.Instance.IsValidPlay(card,topCard))
            {
                int score = CalculateCardScore(card);
                if (score > bestScore)
                {
                    bestScore = score;
                    bestCard = card;
                }
                scores[i] = score;
            }
            else
                scores[i] = 0;
            i++;
        }

        yield return new WaitForSeconds(thinkDelay/2.0f);

        if (bestCard != null && bestScore > 0)
        {
            //Debug.Log($"AI {aiPlayerIndex} playing card with score {bestScore}");
            GameManager.Instance.TryPlayCard(bestCard, aiPlayerIndex);
            
            // If the card was a Wild, the AI needs to pick a color
            if (GameManager.Instance.isWaiting)
            {
                yield return new WaitForSeconds(thinkDelay/3.0f);
                EffectProcessor.Instance.OnColorSelected(mostFreqColor);
            }
        }
        else
        {
            // bestScore was 0, meaning absolutely no cards are valid. Force a draw.
            //Debug.Log($"AI {aiPlayerIndex} found no valid moves. Drawing card.");
            if (EffectProcessor.Instance.pendingDraws == 0)
            {
                GameManager.Instance.TryDrawCard(aiPlayerIndex);
                yield return StartCoroutine(PlayTurnRoutine());
            }
            else
            {
                GameManager.Instance.TryDrawCard(aiPlayerIndex);
            }
        }

    }

    private int CalculateCardScore(CardData givenCard)
    {
        bool freqColor = (givenCard.color == mostFreqColor);

        bool sameType = (givenCard.type == topCard.type);
        bool actionType = (givenCard.type == CardType.Reverse) 
                            || (givenCard.type == CardType.Skip)
                            || (givenCard.type == CardType.PlusTwo);
        bool numberType = (givenCard.type == CardType.Number);
        bool wildType = (givenCard.type == CardType.WildColorChange)
                            || (givenCard.type == CardType.WildPlusFour);

        if(topCard.type == CardType.WildPlusFour)
        {
            if(actionType)
                return 4;
            else if(numberType)
                return 3;
            else if(wildType)
                return 2;
        }
        else if(topCard.type == CardType.WildColorChange)
        {
            if(numberType)
                return 4;
            else if(actionType)
                return 3;
            else if(sameType)
                return 2;
            else if(wildType)
                return 1;                
        }
        else
        {
            // Applies for all of Reverse, Skip and Number
            if(sameType && freqColor)
                return 4;
            else if(numberType)
                return 3;
            else if(sameType)
                return 3;            
            else if(actionType)
                return 2;
            else if(wildType)
                return 1;
        }
        return 0;
    }

    private CardColor GetMostFrequentColor(List<CardData> hand)
    {
        // Simple logic to pick the color the AI has the most of
        var counts = new Dictionary<CardColor, int> { {CardColor.Red,0}, {CardColor.Blue,0}, {CardColor.Green,0}, {CardColor.Yellow,0} };
        foreach(var c in hand) 
            if(c.color != CardColor.Wild) 
                counts[c.color]++;
        
        CardColor best = CardColor.Wild;
        int max = -1;
        foreach(var kvp in counts) { if(kvp.Value > max) { max = kvp.Value; best = kvp.Key; } }
        return best;
    }



    // ==========================================
    // HELPER FUNCTIONS
    // ==========================================

    private CardColor DetermineBestColor(List<CardData> hand)
    {
        int r = 0, b = 0, g = 0, y = 0;

        foreach (CardData card in hand)
        {
            if (card.color == CardColor.Red) r++;
            else if (card.color == CardColor.Blue) b++;
            else if (card.color == CardColor.Green) g++;
            else if (card.color == CardColor.Yellow) y++;
        }

        int max = Mathf.Max(r, Mathf.Max(b, Mathf.Max(g, y)));
        if (max == r) return CardColor.Red;
        if (max == b) return CardColor.Blue;
        if (max == g) return CardColor.Green;
        return CardColor.Yellow;
    }
}
