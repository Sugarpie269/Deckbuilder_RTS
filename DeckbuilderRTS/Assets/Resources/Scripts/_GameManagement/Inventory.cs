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
        private bool CardSlot1Updated;
        private bool CardSlot2Updated;
        private bool CardSlot3Updated;
        private bool DiscardSlotUpdated;
        private bool DrawSlotUpdated;
        private bool DrawError;

        // Audio for replacing the draw deck. ~Liam
        private AudioSource ReplaceDrawSound;

        // Start is called before the first frame update
        void Start()
        {
            this.CardSlot1 = null;
            this.CardSlot2 = null;
            this.CardSlot3 = null;

            this.ErrorCardSlot1 = false;
            this.ErrorCardSlot2 = false;
            this.ErrorCardSlot3 = false;

            this.CardSlot1Updated = true;
            this.CardSlot2Updated = true;
            this.CardSlot3Updated = true;
            this.DiscardSlotUpdated = true;
            this.DrawSlotUpdated = true;
            this.DrawError = false;

            this.Deck = new Collection<ICard>();
            this.Discard = new Collection<ICard>();
        }
        public void AddCardToDeck(ICard card) {
            this.Deck.Add(card);
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

        public bool IsHandFull()
        {
            return this.CardSlot1 != null && this.CardSlot2 != null && this.CardSlot3 != null;
        }

        public ICard GetDiscardTop()
        {
            if (this.Discard.Count <= 0)
            {
                return null;
            }
            return this.Discard[this.Discard.Count-1];
        }

        public void DestroyDiscardTop()
        { 
            if (this.Discard.Count <= 0)
            {
                return;
            }
            this.DiscardSlotUpdated = true;
            this.Discard.RemoveAt(this.Discard.Count-1);
        }

        // Returns true if the draw pile has no cards in it, false otherwise. ~Liam
        public bool IsDeckEmpty()
        {
            return (this.Deck.Count == 0);
        }

        // Returns true if the discard pile has no cards in it, false otherwise. ~Liam
        public bool IsDiscardEmpty()
        {
            return (this.Discard.Count == 0);
        }

        public bool DrawCard()
        {
            // If the deck is empty, then put the discard back in instead.
            if (this.Deck.Count == 0)
            {
                // if the discard is empty, return. ~Jackson
                if (this.Discard.Count == 0)
                {
                    this.DrawError = true;
                    return false;
                }

                // The deck is empty but the discard is not, so shuffle the discard. ~Jackson
                for (var discardIndex = 0; discardIndex < this.Discard.Count; discardIndex++)
                {
                    this.Deck.Add(this.Discard[discardIndex]);
                }

                this.DrawSlotUpdated = true;

                this.Discard.Clear();
                this.DiscardSlotUpdated = true;
                return false;
            }
            // If the deck is not empty, add a card to the first available slot, if possible. ~Jackson
            var cardAdded = false;
            if (this.CardSlot1 == null)
            {
                this.CardSlot1 = this.Deck[0];
                this.Deck.RemoveAt(0);
                this.CardSlot1Updated = true;
                cardAdded = true;
            }
            else if (this.CardSlot2 == null)
            {
                this.CardSlot2 = this.Deck[0];
                this.Deck.RemoveAt(0);
                this.CardSlot2Updated = true;
                cardAdded = true;
            }
            else if (this.CardSlot3 == null)
            {
                this.CardSlot3 = this.Deck[0];
                this.Deck.RemoveAt(0);
                this.CardSlot3Updated = true;
                cardAdded = true;
            }

            // If the deck is now empty, set the flag as needed. ~Liam
            if (this.Deck.Count == 0)
            {
                this.DrawSlotUpdated = true;
            }

            // Give the player an error if they tried to draw a card but their hand was full. ~Liam
            if (!cardAdded)
            {
                this.DrawError = true;
            }
            return cardAdded;
        }

        // Play the card in slot 1. ~Liam
        public void PlayCard1()
        {

            // If there is no card in this slot, do nothing (or draw a card?). ~Jackson
            if (this.CardSlot1 == null)
            {
                // Set the ErrorCardSlot1 flag to true and return. ~Liam
                this.ErrorCardSlot1 = true;
                return;
            }
            
            // Play the sound corresponding to the card in slot 1. If the card is Lightning Strike or Laser Beam, do not play sound (they will be played later). ~Liam
            if (!(this.CardSlot1.GetCardInfo().CardName == "Lightning Strike" || this.CardSlot1.GetCardInfo().CardName == "Laser Beam"))
            {
                this.CardSlot1.GetCardInfo().PlaySound.Play();
            }

            // Play the card in card slot 1. ~Jackson
            this.CardSlot1.OnCardPlayed(this.gameObject, new Vector2(0.0f, 0.0f));

            // Destroy the card if it should be destroyed, otherwise, add it to the discard. ~Jackson
            if (this.CardSlot1.ShouldBeDestroyed() && this.CardSlot1.CanBeDestroyed())
            {
                this.CardSlot1 = null;
            }
            else
            {
                this.Discard.Add(this.CardSlot1);
                this.DiscardSlotUpdated = true;
                this.CardSlot1 = null;
            }

            // Update the CardSlot1Updated flag, to signal that the UI should update the card image. ~Liam
            this.CardSlot1Updated = true;
        }

        // Play the card in slot 2. ~Liam
        public void PlayCard2()
        {

            // If there is no card in this slot, do nothing (or draw a card?). ~Jackson
            if (this.CardSlot2 == null)
            {
                // Set the ErrorCardSlot2 flag to true and return. ~Liam
                this.ErrorCardSlot2 = true;
                return;
            }

            // Play the sound corresponding to the card in slot 2. ~Liam
            if (!(this.CardSlot2.GetCardInfo().CardName == "Lightning Strike" || this.CardSlot2.GetCardInfo().CardName == "Laser Beam"))
            {
                this.CardSlot2.GetCardInfo().PlaySound.Play();
            }
            
            // Play the card in card slot 2. ~Jackson
            this.CardSlot2.OnCardPlayed(this.gameObject, new Vector2(0.0f, 0.0f));

            // Destroy the card if it should be destroyed, otherwise, add it to the discard. ~Jackson
            if (this.CardSlot2.ShouldBeDestroyed() && this.CardSlot2.CanBeDestroyed())
            {
                this.CardSlot2 = null;
            }
            else
            {
                this.Discard.Add(this.CardSlot2);
                this.DiscardSlotUpdated = true;
                this.CardSlot2 = null;
            }

            // Update the CardSlot2Updated flag, to signal that the UI should update the card image. ~Liam
            this.CardSlot2Updated = true;
        }

        // Play the card in slot 3. ~Liam
        public void PlayCard3()
        {

            // If there is no card in this slot, do nothing (or draw a card?). ~Jackson
            if (this.CardSlot3 == null)
            {
                // Set the ErrorCardSlot3 flag to true and return. ~Liam
                this.ErrorCardSlot3 = true;
                return;
            }

            // Play the sound corresponding to the card in slot 3. ~Liam
            if (!(this.CardSlot3.GetCardInfo().CardName == "Lightning Strike" || this.CardSlot3.GetCardInfo().CardName == "Laser Beam"))
            {
                this.CardSlot3.GetCardInfo().PlaySound.Play();
            }
            
            // Play the card in card slot 3. ~Jackson
            this.CardSlot3.OnCardPlayed(this.gameObject, new Vector2(0.0f, 0.0f));

            // Destroy the card if it should be destroyed, otherwise, add it to the discard. ~Jackson
            if (this.CardSlot3.ShouldBeDestroyed() && this.CardSlot3.CanBeDestroyed())
            {
                this.CardSlot3 = null;
            }
            else
            {
                this.Discard.Add(this.CardSlot3);
                this.DiscardSlotUpdated = true;
                this.CardSlot3 = null;
            }

            // Update the CardSlot3Updated flag, to signal that the UI should update the card image. ~Liam
            this.CardSlot3Updated = true;
        }

        public void GainCard(ICard card)
        {
            this.Discard.Add(card);
            this.DiscardSlotUpdated = true;
        }

        // Getter functions for the sprites corresponding to the cards in the 3 slots (and the discard pile). ~Liam
        public bool GetCardSlot1Info(ref CardInfo cInfo)
        {
            // Check if the slot is empty, and return the blank slot sprite if so. Otherwise, return the card's associated sprite. ~Liam
            if (this.CardSlot1 != null)
            {
                cInfo = this.CardSlot1.GetCardInfo();
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool GetCardSlot2Info(ref CardInfo cInfo)
        {
            // Check if the slot is empty, and return the blank slot sprite if so. Otherwise, return the card's associated sprite. ~Liam
            if (this.CardSlot2 != null)
            {
                cInfo = this.CardSlot2.GetCardInfo();
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool GetCardSlot3Info(ref CardInfo cInfo)
        {
            // Check if the slot is empty, and return the blank slot sprite if so. Otherwise, return the card's associated sprite. ~Liam
            if (this.CardSlot3 != null)
            {
                cInfo = this.CardSlot3.GetCardInfo();
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool GetDiscardSlotImageInfo(ref CardInfo cInfo)
        {
            // Check if the slot is empty, and return the blank slot sprite if so. Otherwise, return the last card in the pile's associated sprite. ~Liam
            if (this.Discard.Count > 0)
            {
                cInfo = this.Discard[Discard.Count - 1].GetCardInfo();
                return true;
            }
            else
            {
                return false;
            }
        }

        // Getter & setter functions for the various flags. ~Liam
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

        public bool GetCardSlot1Updated()
        {
            return this.CardSlot1Updated;
        }
        public bool GetCardSlot2Updated()
        {
            return this.CardSlot2Updated;
        }
        public bool GetCardSlot3Updated()
        {
            return this.CardSlot3Updated;
        }

        public void SetCardSlot1Updated(bool value)
        {
            this.CardSlot1Updated = value;
        }
        public void SetCardSlot2Updated(bool value)
        {
            this.CardSlot2Updated = value;
        }
        public void SetCardSlot3Updated(bool value)
        {
            this.CardSlot3Updated = value;
        }

        public bool GetDrawSlotUpdated()
        {
            return this.DrawSlotUpdated;
        }
        public void SetDrawSlotUpdated(bool value)
        {
            this.DrawSlotUpdated = value;
        }

        public bool GetDiscardSlotUpdated()
        {
            return this.DiscardSlotUpdated;
        }
        public void SetDiscardSlotUpdated(bool value)
        {
            this.DiscardSlotUpdated = value;
        }

        public bool GetDrawError()
        {
            return this.DrawError;
        }
        public void SetDrawError(bool value)
        {
            this.DrawError = value;
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}

