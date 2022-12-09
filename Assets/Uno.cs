using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Uno : MonoBehaviour
{
// The deck of cards in the game
    [SerializeField]
    public Stack<string> _deck;

    // The player's hand of cards
    public List<string> _hand;

    // The card that is currently at the top of the discard pile
    public string _currentCard;

    void Start()
    {
        // Initialize the deck and shuffle the cards
        _deck = new Stack<string>();
        ShuffleDeck();
        Debug.Log((_deck));

        // Deal the initial hand to the player
        _hand = new List<string>();
        DealHand();
    }

    void Update()
    {
        // Check if the player has played a valid card
        var playedCard = GetPlayedCard();
        if (!IsValidPlay(playedCard)) return;
        // Remove the played card from the player's hand
        _hand.Remove(playedCard);

        // Add the played card to the top of the discard pile
        _currentCard = playedCard;

        // Check if the player has won the game
        if (_hand.Count == 0)
        {
            Debug.Log("Congratulations, you won the game!");
        }
    }

    // Shuffles the deck of cards
    public void ShuffleDeck()
    {
        // Create a list of all the cards in the deck
        var cards = new List<string>();
        foreach (var color in new string[] { "Red", "Yellow", "Green", "Blue" })
        {
            for (var i = 0; i < 10; i++)
            {
                cards.Add(color + " " + i);
            }
            for (var i = 1; i <= 2; i++)
            {
                cards.Add(color + " Skip");
                cards.Add(color + " Reverse");
                cards.Add(color + " Draw Two");
            }
            cards.Add(color + " Wild");
            cards.Add(color + " Wild Draw Four");
        }

        // Shuffle the list of cards using the Fisher-Yates shuffle algorithm
        for (var i = cards.Count - 1; i > 0; i--)
        {
            var j = Random.Range(0, i + 1);
            (cards[i], cards[j]) = (cards[j], cards[i]);
        }

        // Add the shuffled cards to the deck
        _deck = new Stack<string>(cards);
    }

    // Deals the initial hand to the player
    public void DealHand()
    {
        // Deal 7 cards to the player
        for (var i = 0; i < 7; i++)
        {
            _hand.Add(_deck.Pop());
        }

        // Set the top card of the discard pile to the first card in the deck
        _currentCard = _deck.Pop();
    }

    // Returns the card that the player has played, or null if no card was played
    public string GetPlayedCard()
    {
        // Check if the player has clicked on a card in their hand
        if (Input.GetMouseButtonDown(0))
        {
            // Check if the mouse cursor is over a card in the player's hand
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit))
            {
                // Return the card that the player clicked on
                return hit.collider.gameObject.name;
            }
        }

        // Return null if no card was played
        return null;
    }

    // Returns true if the played card is a valid play, or false if it is not
    public bool IsValidPlay(string playedCard)
    {
        // Check if the played card is in the player's hand
        if (!_hand.Contains(playedCard))
        {
            return false;
        }

        // Check if the played card has the same color or number as the current card
        string[] playedCardParts = playedCard.Split(' ');
        string[] currentCardParts = _currentCard.Split(' ');
        if (playedCardParts[0] == currentCardParts[0] || playedCardParts[1] == currentCardParts[1])
        {
            return true;
        }

        // Check if the played card is a Wild card
        if (playedCardParts[1] == "Wild" || playedCardParts[1] == "Wild Draw Four")
        {
            return true;
        }

        // Return false if the played card is not a valid play
        return false;
    }
}
