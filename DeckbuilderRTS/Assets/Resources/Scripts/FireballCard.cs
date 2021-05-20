using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DeckbuilderRTS
{
    public class FireballCard : ICard
    {
        private Vector3Int Cost;
        private Object FireballPrefab;
        private float SummonDistance = 3.0f;
        [SerializeField] private Texture2D CardImage;

        public FireballCard(Object prefab)
        {
            this.FireballPrefab = prefab;
            this.Cost = new Vector3Int(1, 0, 0);
            this.CardImage = Resources.Load<Texture2D>("Sprites/FireballCard_1000x1500");
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

        // Returns the image of this card, for use in the UI. ~Liam
        public Texture2D GetCardImage()
        {
            return this.CardImage;
        }
    }
}

