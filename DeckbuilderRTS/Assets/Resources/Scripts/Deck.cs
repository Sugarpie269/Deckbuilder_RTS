using System.Collections.ObjectModel;
using UnityEngine;

using DeckbuilderRTS;

namespace DeckbuilderRTS
{
    public class Deck
    {
        private Collection<ICard> Cards;

        ICard DrawCard()
        {
            // If the deck is empty, return null.
            if (Cards.Count == 0)
            {
                return null;
            }

            // Return the first card in the deck if the deck is not empty.
            return Cards[0];
        }
    }
}

