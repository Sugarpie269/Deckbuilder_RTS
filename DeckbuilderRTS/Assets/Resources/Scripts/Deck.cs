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
        
        void Update()
        {
            // If the deck is on cooldown, update it.
            if (this.DrawCardCoolDown > 0.0f)
            {
                this.DrawCardCoolDown -= Time.deltaTime;
            }
            // If updating the cooldown set it to less than zero, fix it back to zero.
            if (this.DrawCardCoolDown < 0.0f)
            {
                this.DrawCardCoolDown = 0.0f;
            }
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

