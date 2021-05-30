using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DeckbuilderRTS
{
    public class InstantHealCard : ICard
    {
        private Vector3Int Cost;
        private float Value;
        [SerializeField] private Texture2D CardImage;

        public InstantHealCard()
        {
            this.Cost = new Vector3Int(1, 0, 0);
            this.Value = 3.0f;
            this.CardImage = Resources.Load<Texture2D>("Sprites/InstantHealCard_1000x1500");
        }

        public Vector3Int GetCost()
        {
            return this.Cost;
        }

        public void OnCardPlayed(GameObject player, Vector2 target)
        {
            player.GetComponent<PlayerController>().ApplyHealing(this.Value);
        }

        // This returns true if the card should be removed from the deck after use. ~Jackson.
        public bool ShouldBeDestroyed()
        {
            return false;
        }

        // This returns true if the card can be removed from the deck after use. ~Jackson.
        public bool CanBeDestroyed()
        {
            return true;
        }

        // Returns the image of this card, for use in the UI. ~Liam
        public Texture2D GetCardImage()
        {
            return this.CardImage;
        }
    }
}

