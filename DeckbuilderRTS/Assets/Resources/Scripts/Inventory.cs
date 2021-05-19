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
            this.CardSlot1.OnCardPlayed(this.gameObject, new Vector2(0.0f, 0.0f));
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}

