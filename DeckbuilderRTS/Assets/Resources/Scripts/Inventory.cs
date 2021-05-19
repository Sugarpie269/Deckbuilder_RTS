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

        // Update is called once per frame
        void Update()
        {

        }
    }
}

