using System;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance { get; private set; }

    [Header("Turn State")]
    public int totalPlayers { get; private set; }
    public int currentPlayerIndex { get; private set; }
    public int currentRound { get; private set; }
    
    [SerializeField] 
    private bool playDirectionClockwise;

    // --- EVENTS ---
    public event Action<int> OnTurnStarted;
    public event Action<int> OnTurnEnded;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
    }

    // Called by the GameManager to initailize turn mechanism 
    public void InitializeMatch(int playerCount)
    {
        if (playerCount <= 0)
        {
            Debug.LogError("Cannot start with 0 players!");
            return;
        }

        totalPlayers = playerCount;
        currentPlayerIndex = 0;
        playDirectionClockwise = true;

        // Announce the first turn
        OnTurnStarted?.Invoke(currentPlayerIndex);
    }

    public void PassTurn()
    {
        OnTurnEnded?.Invoke(currentPlayerIndex);

        currentPlayerIndex = GetNextIndex();

        //Debug.Log($"Turn passed to Player {currentPlayerIndex}");
        OnTurnStarted?.Invoke(currentPlayerIndex);
    }

    public int GetNextIndex()
    {
        int directionVar;
        if(playDirectionClockwise)
            directionVar = 1;
        else
            directionVar = -1;
        
        // (Current + Direction + Total) % Total handles player loop
        return (currentPlayerIndex + directionVar + totalPlayers) % totalPlayers;
    }

    public void SetPlayerIndex(int index)
    { currentPlayerIndex = index % totalPlayers; }

    // ==========================================
    // EFFECT PROCESSOR HELPERS
    // ==========================================

    public void ReverseDirection()
    {
        playDirectionClockwise = !playDirectionClockwise;
        //Debug.Log("Play direction reversed");
    }

    public void SkipNextPlayer()
    {
        currentPlayerIndex = GetNextIndex();
        //Debug.Log($"Player skipped!");
    }
}