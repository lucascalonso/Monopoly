using System;
using System.Collections.Generic;

namespace Monopoly.Cards
{
    public class Deck
    {
        private List<Card> cards;
        private Random rng = new Random();

        public Deck(List<Card> cards)
        {
            this.cards = new List<Card>(cards);
            Shuffle();
        }

        public void Shuffle()
        {
            int n = cards.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                var value = cards[k];
                cards[k] = cards[n];
                cards[n] = value;
            }
        }

        public Card Draw()
        {
            if (cards.Count == 0) return null;
            Card card = cards[0];
            cards.RemoveAt(0);
            return card;
        }
    }
}