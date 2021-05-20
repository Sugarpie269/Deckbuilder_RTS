using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DeckbuilderRTS
{
    public class InstantHealCard : ICard
    {
        private Vector3Int Cost;
        private float Value;

        public InstantHealCard()
        {
            this.Cost = new Vector3Int(1, 0, 0);
            this.Value = 3.0f;
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
    }
}

