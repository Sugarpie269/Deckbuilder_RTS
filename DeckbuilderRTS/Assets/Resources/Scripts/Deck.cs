using System.Collections.ObjectModel;
using UnityEngine;

using DeckbuilderRTS;

namespace DeckbuilderRTS
{
    public class Deck
    {
        private Collection<ICard> Cards;
        private float DrawCardCoolDown = 0.0f;
        private float DRAW_CARD_COOL_DOWN_BASE = 5.0f;
        private int CurrentCooldownShown = 0;
        
        void Update()
        {
            
        }

        ICard DrawCard()
        {
            // If the deck is empty, return null.
            if (this.Cards.Count == 0)
            {
                return null;
            }

            this.DrawCardCoolDown = this.DRAW_CARD_COOL_DOWN_BASE;

            // Return the first card in the deck if the deck is not empty.
            return this.Cards[0];
        }
    }
}

