using UnityEngine;
using System.Collections.ObjectModel;

namespace DeckbuilderRTS
{
    public class Inventory : MonoBehaviour
    {
        private ICard CardSlot1;
        private ICard CardSlot2;
        private ICard CardSlot3;
        private Collection<ICard> Deck;
        private Collection<ICard> Discard;

        // Start is called before the first frame update
        void Start()
        {
            this.CardSlot1 = null;
            this.CardSlot2 = null;
            this.CardSlot3 = null;

            this.Deck = new Collection<ICard>();
            this.Discard = new Collection<ICard>();
        }

        public void AddCardSlot1(ICard card)
        {
            this.CardSlot1 = card;
        }

        public void PlayCard1()
        {

            // If there is no card in this slot, do nothing (or draw a card?). ~Jackson
            if (this.CardSlot1 == null)
            {
                // Draw a card from the deck and remove it from the deck. ~Jackson
                if (this.Deck.Count > 0)
                {
                    this.CardSlot1 = this.Deck[0];
                    this.Deck.RemoveAt(0);
                }
                else
                {
                    // if the discard is empty, return. ~Jackson
                    if (this.Discard.Count == 0)
                    {
                        return;
                    }

                    // The deck is empty, so shuffle the discard. ~Jackson
                    for (var discardIndex = 0; discardIndex < this.Discard.Count; discardIndex++)
                    {
                        this.Deck.Add(this.Discard[discardIndex]);
                    }
                    this.Discard.Clear();
                    this.PlayCard1();
                }
                return;
            }
            // Play the card in card slot 1. ~Jackson
            this.CardSlot1.OnCardPlayed(this.gameObject, new Vector2(0.0f, 0.0f));

            // Destroy the card if it shoudl be destroyed, otherwise, add it to the discard. ~Jackson
            if (this.CardSlot1.ShouldBeDestroyed() && this.CardSlot1.CanBeDestroyed())
            {
                this.CardSlot1 = null;
            }
            else
            {
                this.Discard.Add(this.CardSlot1);
                this.CardSlot1 = null;
            }
        }

        public void GainCard(ICard card)
        {
            this.Discard.Add(card);
        }

        // Update is called once per frame
        void Update()
        {
            //Debug.Log("Deck:" + this.Deck.Count.ToString() + ";Discard:" + this.Discard.Count.ToString());
        }
    }
}

