using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DeckbuilderRTS
{
    public class FireballCard : ICard
    {
        private Object FireballPrefab;
        private float SummonDistance = 5.0f;

        public FireballCard(Object prefab)
        {
            this.FireballPrefab = prefab;
        }

        public void OnCardPlayed(GameObject player, Vector2 target)
        {
            var newFireball = Object.Instantiate(this.FireballPrefab) as GameObject;
            newFireball.transform.position = new Vector3(player.transform.position.x + this.SummonDistance, player.transform.position.y, player.transform.position.z);

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

