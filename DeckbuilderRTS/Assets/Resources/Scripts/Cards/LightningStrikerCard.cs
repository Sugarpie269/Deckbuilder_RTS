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

        private float Direction;
        private float Length = 100.0f;
        private float delay = 0.5f;

        private float Lifetime = 1.5f;

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

            //var lightningStrikerPos2 = new Vector3(playerPos.x + lightningStrikerDirection.x * this.SummonDistance, playerPos.y + lightningStrikerDirection.y * this.SummonDistance, player.transform.position.z);
            
            // Load Lighting Striker at mouse's position.
            var lightningStrikerPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            lightningStrikerPos = new Vector3(lightningStrikerPos.x, lightningStrikerPos.y, player.transform.position.z);
        
            
            var newLightningStriker = Object.Instantiate(this.LightningStrikerPrefab) as GameObject;
            newLightningStriker.transform.position = lightningStrikerPos;
                
            // Set Rotation
            var vec = Input.mousePosition - Camera.main.WorldToScreenPoint(player.transform.position);
            var angle = (Mathf.Atan2(vec.y, vec.x) * Mathf.Rad2Deg) + 90;


            var lightningStrikerController = newLightningStriker.GetComponent<LightningStrikerController>();
                
            //var lightningStrikerVelocity = new Vector2(this.LightningStrikerSpeed * lightningStrikerDirection.x, this.LightningStrikerSpeed * lightningStrikerDirection.y);
            lightningStrikerController.SetAttributes(this.Info.CardPower, this.delay, this.Lifetime, angle);
            //lightningStrikerController.SetAttributes(2f, this.delay, this.Lifetime, angle);

            Physics2D.IgnoreCollision(player.GetComponent<BoxCollider2D>(), newLightningStriker.GetComponent<CircleCollider2D>());
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

