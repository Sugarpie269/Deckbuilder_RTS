using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace DeckbuilderRTS
{
    public class LightningStrikerCard : ICard
    {
        [SerializeField]
        private Object LightningStrikerPrefab;
        private float SummonDistance = 1.5f;
        private float LightningStrikerDirection;
        private float LightningStrikerSpeed = 3.0f;

        // These are the references to the card's information within the CardDisplayLibrary gameObject. ~Liam
        private CardInfo Info;

        public LightningStrikerCard(Object prefab)
        {
            this.LightningStrikerPrefab = prefab;

            // Instantiate each piece of information about the card. ~Liam
            var gameMaster = GameObject.Find("GameController");
            var gameController = gameMaster.GetComponent<GameController>();

            this.Info = gameController.GetCardInfo("Card_LightningStriker");
        }

        // Returns a struct of card information, for use in the UI. ~Liam
        public CardInfo GetCardInfo()
        {
            return this.Info;
        }

        public void OnCardPlayed(GameObject player, Vector2 target)
        {
            var playerController = player.GetComponent<PlayerController>();
            var playerPos = player.transform.position;
            var lightningStrikerDirection = playerController.GetMousePosition();

            var leafbladePos = new Vector3(playerPos.x + lightningStrikerDirection.x * this.SummonDistance, playerPos.y + lightningStrikerDirection.y * this.SummonDistance, player.transform.position.z);
            var newLightningStriker = Object.Instantiate(this.LightningStrikerPrefab) as GameObject;
            newLightningStriker.transform.position = leafbladePos;

            var leafbladeController = newLightningStriker.GetComponent<LightningStrikerController>();
            var leafbladeVelocity = new Vector2(this.LightningStrikerSpeed * lightningStrikerDirection.x, this.LightningStrikerSpeed * lightningStrikerDirection.y);
            leafbladeController.SetAttributes(this.Info.CardPower, leafbladeVelocity);

            Physics2D.IgnoreCollision(player.GetComponent<BoxCollider2D>(), newLightningStriker.GetComponent<BoxCollider2D>());
            GameObject.Destroy(newLightningStriker, 5f);
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

