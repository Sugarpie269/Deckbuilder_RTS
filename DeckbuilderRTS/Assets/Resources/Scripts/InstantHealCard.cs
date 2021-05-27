using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DeckbuilderRTS
{
    public class InstantHealCard : ICard
    {
        private string Name = "Instant Heal";
        private Vector3Int Cost;
        private uint level;
        private float Power; // for a healing card, power is the amount healed
        [SerializeField] private Sprite CardImage;

        public InstantHealCard()
        {
            this.Cost = new Vector3Int(1, 0, 0);
            this.Power = 3.0f;
            Texture2D tempTexture = Resources.Load<Texture2D>("Sprites/InstantHealCard_1000x1500");
            this.CardImage = Sprite.Create(tempTexture, new Rect(0f, 0f, tempTexture.width, tempTexture.height), new Vector2(0.5f, 0.5f));
        }

        public void OnCardPlayed(GameObject player, Vector2 target)
        {
            player.GetComponent<PlayerController>().ApplyHealing(this.Power);
        }

        public string GetName()
        {
            return this.Name;
        }

        public Vector3 GetCost()
        {
            return this.Cost;
        }

        public int GetPower()
        {
            return (int)this.Power;
        }
        public int GetStrength()
        {
            return 0;
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
        public Sprite GetCardImage()
        {
            return this.CardImage;
        }
    }
}

