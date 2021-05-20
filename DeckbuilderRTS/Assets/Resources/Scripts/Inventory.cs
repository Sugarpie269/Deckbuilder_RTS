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

        // Flags. ~Liam
        private bool ErrorCardSlot1;
        private bool ErrorCardSlot2;
        private bool ErrorCardSlot3;

        // Start is called before the first frame update
        void Start()
        {
            this.CardSlot1 = null;
            this.CardSlot2 = null;
            this.CardSlot3 = null;

            this.ErrorCardSlot1 = false;
            this.ErrorCardSlot2 = false;
            this.ErrorCardSlot3 = false;

            this.Deck = new Collection<ICard>();
            this.Discard = new Collection<ICard>();
        }

        public void AddCardSlot1(ICard card)
        {
            this.CardSlot1 = card;
        }

        public void AddCardSlot2(ICard card)
        {
            this.CardSlot2 = card;
        }

        public void AddCardSlot3(ICard card)
        {
            this.CardSlot3 = card;
        }

        public void DrawCard()
        {
            // If the deck is empty, then put the discard back in instead.
            if (this.Deck.Count == 0)
            {
                // if the discard is empty, return. ~Jackson
                if (this.Discard.Count == 0)
                {
                    return;
                }

                // The deck is empty but the discard is not, so shuffle the discard. ~Jackson
                for (var discardIndex = 0; discardIndex < this.Discard.Count; discardIndex++)
                {
                    this.Deck.Add(this.Discard[discardIndex]);
                }

                this.Discard.Clear();
                return;
            }
            Debug.Log("I ran");
            // If the deck is not empty, add a card to the first available slot, if possible. ~Jackson
            if (this.CardSlot1 == null)
            {
                this.CardSlot1 = this.Deck[0];
                this.Deck.RemoveAt(0);
            }
            else if (this.CardSlot2 == null)
            {
                this.CardSlot2 = this.Deck[0];
                this.Deck.RemoveAt(0);
            }
            else if (this.CardSlot3 == null)
            {
                this.CardSlot3 = this.Deck[0];
                this.Deck.RemoveAt(0);
            }
        }

        public void PlayCard1()
        {

            // If there is no card in this slot, do nothing (or draw a card?). ~Jackson
            if (this.CardSlot1 == null)
            {
                // Set the ErrorCardSlot1 flag to true and return. ~Liam
                this.ErrorCardSlot1 = true;
                return;
                /*// Draw a card from the deck and remove it from the deck. ~Jackson
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
                return;*/
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

        // Getter & setter functions for the 3 card slot error flags. ~Liam
        public bool GetErrorCardSlot1()
        {
            return this.ErrorCardSlot1;
        }
        public bool GetErrorCardSlot2()
        {
            return this.ErrorCardSlot2;
        }
        public bool GetErrorCardSlot3()
        {
            return this.ErrorCardSlot3;
        }

        public void SetErrorCardSlot1(bool value)
        {
            this.ErrorCardSlot1 = value;
        }
        public void SetErrorCardSlot2(bool value)
        {
            this.ErrorCardSlot2 = value;
        }
        public void SetErrorCardSlot3(bool value)
        {
            this.ErrorCardSlot3 = value;
        }
        // Update is called once per frame
        void Update()
        {
            Debug.Log("Deck:" + this.Deck.Count.ToString() + ";Discard:" + this.Discard.Count.ToString());
        }
    }
}

